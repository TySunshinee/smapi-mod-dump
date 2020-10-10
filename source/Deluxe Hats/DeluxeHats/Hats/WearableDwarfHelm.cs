/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/domsim1/stardew-valley-deluxe-hats-mod
**
*************************************************/

using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Locations;

namespace DeluxeHats.Hats
{
    public static class WearableDwarfHelm
    {
        public const string Name = "Wearable Dwarf Helm";
        public const string Description = "You can Understand Dwarves.\nWhen entering a new level of a mine gain Mad Dwarf King buff:\n+4 Mining\n+2 Speed\n+1 Attack.";
        private static string locaction;
        public static void Activate()
        {
            Game1.player.canUnderstandDwarves = true;
            HatService.OnUpdateTicked = (e) =>
            {
                if (Game1.player.hasMenuOpen || !Game1.player.canMove || !Game1.game1.IsActive || Game1.eventUp)
                {
                    return;
                }

                if (!Game1.currentLocation.name.Contains("UndergroundMine"))
                {
                    return;
                }

                if (!string.IsNullOrEmpty(locaction) && Game1.currentLocation.name == locaction)
                {
                    return;
                }

                locaction = Game1.currentLocation.name;

                Buff dwarfBuff = Game1.buffsDisplay.otherBuffs.FirstOrDefault(x => x.which == HatService.BuffId);
                if (dwarfBuff == null)
                {
                    dwarfBuff = new Buff(
                    farming: 0,
                    fishing: 0,
                    mining: 4,
                    digging: 0,
                    luck: 0,
                    foraging: 0,
                    crafting: 0,
                    maxStamina: 0,
                    magneticRadius: 0,
                    speed: 2,
                    defense: 0,
                    attack: 1,
                    minutesDuration: 1,
                    source: "Deluxe Hats",
                    displaySource: Name)
                    {
                        which = HatService.BuffId,
                    };
                    dwarfBuff.description = "Mad Dwarf King\n+4 Mining\n+2 Speed\n+1 Attack";
                    Game1.buffsDisplay.addOtherBuff(dwarfBuff);
                }
                dwarfBuff.millisecondsDuration = 21510;
            };
        }

        public static void Disable()
        {
            var mus = (LibraryMuseum)Game1.getLocationFromName("ArchaeologyHouse");
            if (mus != null)
            {
                var museumItems = new HashSet<int>(mus.museumPieces.Values);
                if (museumItems.Contains(96) && (museumItems.Contains(97) && museumItems.Contains(98)) && museumItems.Contains(99))
                {
                    Game1.player.canUnderstandDwarves = true;
                }
                else
                {
                    Game1.player.canUnderstandDwarves = false;
                }
            }
            Buff dwarfBuff = Game1.buffsDisplay.otherBuffs.FirstOrDefault(x => x.which == HatService.BuffId);
            if (dwarfBuff != null)
            {
                dwarfBuff.millisecondsDuration = 0;
            }
        }
    }
}
