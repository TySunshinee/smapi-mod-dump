/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/ImJustMatt/ExpandedStorage
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using ExpandedStorage.Framework;
using Harmony;
using StardewHack;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using SDVObject = StardewValley.Object;

namespace ExpandedStorage
{
    internal class ModEntry : HackWithConfig<ModEntry, ModConfig>
    {
        private static int ScrollTop
        {
            get => _scrollTop.Value;
            set => _scrollTop.Value = value;
        }
        private static readonly PerScreen<int> _scrollTop = new PerScreen<int>();
        
        /// <summary>
        /// Returns the amount to offset the InventoryMenu by depending on if there is an overflow of items to display.
        /// </summary>
        /// <param name="inventoryMenu">The Inventory Menu to base scrolling on.</param>
        /// <returns>The number of slot items to offset inventory by.</returns>
        public static int GetScrollTop(InventoryMenu inventoryMenu) =>
            ScrollTop <= 12 * Math.Ceiling(inventoryMenu.actualInventory.Count / 12f) - inventoryMenu.capacity
                ? ScrollTop
                : 0;
        
        public override void HackEntry(IModHelper helper)
        {
            DataLoader.Init(helper, Monitor);
            ChestPatches.init(Monitor);

            // Events
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.World.ObjectListChanged += OnObjectListChanged;
            
            // Patches
            if (!config.AllowModdedCapacity)
                return;
            
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.Input.MouseWheelScrolled += OnMouseWheelScrolled;
            
            harmony.Patch(
                original: AccessTools.Method(typeof(Chest), nameof(Chest.GetActualCapacity)),
                prefix: new HarmonyMethod(typeof(ChestPatches), nameof(ChestPatches.GetActualCapacity_Prefix)));
            
            Patch(() => new ItemGrabMenu(null, false, false, null, null, null, null, false, false, false, false, false,
                    0, null, 0, null),
                ItemGrabMenu_ctor);
            Patch((InventoryMenu im) => im.leftClick(0,0,null,false),
                InventoryMenu_leftClick);
            Patch((InventoryMenu im) => im.rightClick(0, 0, null, false, false),
                InventoryMenu_rightClick);
            Patch((InventoryMenu im) => im.hover(0, 0, null),
                InventoryMenu_hover);
            Patch((InventoryMenu im) => im.draw(null, 0, 0, 0),
                InventoryMenu_draw);
        }
        /// <summary>
        /// Converts vanilla chests to expanded, if necessary.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private static void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            Utility.ForAllLocations(delegate(GameLocation location)
            {
                foreach (var itemPosition in location.Objects.Pairs
                    .Where(c =>
                        c.Value is Chest &&
                        c.Value.ShouldBeExpandedStorage() &&
                        !c.Value.IsExpandedStorage()))
                {
                    var pos = itemPosition.Key;
                    var obj = itemPosition.Value;
                    location.Objects[pos] = obj.ToExpandedStorage();
                }
            });
        }
        /// <summary>
        /// Resets scrolling when storage is closed or a different one is accessed.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private static void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.OldMenu is ItemGrabMenu && e.NewMenu is null)
                ScrollTop = 0;
        }
        /// <summary>
        /// Scrolls inventory menu when there is an overflow of items.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private static void OnMouseWheelScrolled(object sender, MouseWheelScrolledEventArgs e)
        {
            if (!(Game1.activeClickableMenu is ItemGrabMenu {ItemsToGrabMenu: { } inventoryMenu}))
                return;
            if (e.Delta < 0 && ScrollTop < inventoryMenu.actualInventory.Count - inventoryMenu.capacity)
                ScrollTop += inventoryMenu.capacity / inventoryMenu.rows;
            else if (e.Delta > 0 && ScrollTop > 0)
                ScrollTop -= inventoryMenu.capacity / inventoryMenu.rows;
        }
        /// <summary>
        /// Converts objects to modded storage when placed in the world.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
            if (e.Added.Count() != 1)
                return;

            var itemPosition = e.Added.Single();
            var pos = itemPosition.Key;
            var obj = itemPosition.Value;

            if (!obj.ShouldBeExpandedStorage() || obj.IsExpandedStorage())
                return;
            
            Monitor.VerboseLog($"OnObjectListChanged: Converting to Expanded Storage Chest");
            e.Location.objects[pos] = obj.ToExpandedStorage();
        }
        /// <summary>
        /// Loads default chest InventoryMenu when storage has modded capacity.
        /// </summary>
        private void ItemGrabMenu_ctor()
        {
            var code = FindCode(
                Instructions.Isinst(typeof(Chest)),
                Instructions.Callvirt(typeof(Chest), nameof(Chest.GetActualCapacity)),
                Instructions.Ldc_I4_S(36)
            );
            var pos = code.Follow(3);
            code[3] = Instructions.Bge(AttachLabel(pos[0]));
        }
        /// <summary>
        /// Offsets displayed slots by the scrolled amount.
        /// </summary>
        private void InventoryMenu_leftClick()
        {
            OffsetSlotNumber();
        }
        private void InventoryMenu_rightClick()
        {
            OffsetSlotNumber();
        }
        private void InventoryMenu_hover()
        {
            OffsetSlotNumber();
        }
        private void OffsetSlotNumber()
        {
            // Convert.ToInt32(c.name) => Convert.ToInt32(c.name) + ModEntry.ScrollTop
            FindCode(
                Instructions.Ldfld(typeof(ClickableComponent), nameof(ClickableComponent.name)),
                Instructions.Call(typeof(Convert), nameof(Convert.ToInt32), typeof(string))
            ).Append(
                Instructions.Ldarg_0(),
                Instructions.Call(typeof(ModEntry), nameof(GetScrollTop), typeof(InventoryMenu)),
                Instructions.Add()
            );
        }
        /// <summary>
        /// Displays the correct item when InventoryMenu is scrolled.
        /// </summary>
        private void InventoryMenu_draw()
        {
            var code = FindCode(
                Instructions.Ldfld(typeof(InventoryMenu), nameof(InventoryMenu.actualInventory)),
                Instructions.Callvirt_get(typeof(ICollection<Item>), nameof(ICollection<Item>.Count))
            );
            code.Append(
                Instructions.Ldarg_0(),
                Instructions.Call(typeof(ModEntry), nameof(GetScrollTop), typeof(InventoryMenu)),
                Instructions.Sub()
            );
            
            var k = generator.DeclareLocal(typeof(int));
            for (var i = 0; i < 7; i++)
            {
                code = code.FindNext(
                    Instructions.Ldfld(typeof(InventoryMenu), nameof(InventoryMenu.actualInventory)),
                    OpCodes.Ldloc_S
                );
                code.Append(
                    Instructions.Ldarg_0(),
                    Instructions.Call(typeof(ModEntry), nameof(GetScrollTop), typeof(InventoryMenu)),
                    Instructions.Add()
                );
            }
        }
    }
}