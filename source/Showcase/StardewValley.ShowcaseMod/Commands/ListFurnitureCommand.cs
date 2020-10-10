/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Igorious/Stardew_Valley_Showcase_Mod
**
*************************************************/

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Igorious.StardewValley.DynamicApi2;
using Igorious.StardewValley.DynamicApi2.Services;
using StardewModdingAPI;

namespace Igorious.StardewValley.ShowcaseMod.Commands
{
    public sealed class ListFurnitureCommand : ConsoleCommand
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Sorting
        {
            name = 1,
            id,
        }

        public ListFurnitureCommand(IMonitor monitor) : base(monitor, "list_furniture", "List furniture items from the game data.") { }

        public void Execute() => Execute(default(Sorting));

        public void Execute(Sorting sortingKey)
        {
            var furnitureData = DataService.Instance.GetFurniture().Select(kv => (ID: kv.Key, Name: kv.Value.Split('/').First()));
            var orderedFurniture = 
                (sortingKey == Sorting.name)? furnitureData.OrderBy(f => f.Name) : 
                (sortingKey == Sorting.id)? furnitureData.OrderBy(f => f.ID) :
                furnitureData;
            foreach (var furnitureInfo in orderedFurniture)
            {
                Info($"{furnitureInfo.ID,4}: {furnitureInfo.Name}");
            }
        }
    }
}