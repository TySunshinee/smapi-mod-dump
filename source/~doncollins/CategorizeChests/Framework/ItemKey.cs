/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/doncollins/StardewValleyMods
**
*************************************************/

using Newtonsoft.Json;

namespace StardewValleyMods.CategorizeChests.Framework
{
    /// <summary>
    /// A key uniquely identifying a single kind of item selectable as the
    /// contents of a chest.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)] // TODO: can some of these annotations be left out?
    class ItemKey
    {
        [JsonProperty] public readonly ItemType ItemType;

        [JsonProperty] public readonly int ObjectIndex;

        [JsonConstructor]
        public ItemKey(ItemType itemType, int parentSheetIndex)
        {
            ItemType = itemType;
            ObjectIndex = parentSheetIndex;
        }

        public override int GetHashCode()
        {
            return ((int) ItemType) * 10000 + ObjectIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemKey itemKey
                   && itemKey.ItemType == ItemType
                   && itemKey.ObjectIndex == ObjectIndex;
        }
    }
}