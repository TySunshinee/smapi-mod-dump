/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/ChroniclerCherry/stardew-valley-mods
**
*************************************************/

using ShopTileFramework.ItemPriceAndStock;
using System.Collections.Generic;

namespace ShopTileFramework.Data
{
    abstract class ItemShopModel
    {
        public string ShopName { get; set; }
        public string StoreCurrency { get; set; } = "Money";
        public List<int> CategoriesToSellHere { get; set; } = null;
        public string PortraitPath { get; set; } = null;
        public string Quote { get; set; } = null;
        public int ShopPrice { get; set; } = -1;
        public int MaxNumItemsSoldInStore { get; set; } = int.MaxValue;
        public double DefaultSellPriceMultipler { set => DefaultSellPriceMultiplier = value; }
        public double DefaultSellPriceMultiplier { get; set; } = 1;
        public Dictionary<double, string[]> PriceMultiplierWhen { get; set; } = null;
        public ItemStock[] ItemStocks { get; set; }
        public string[] When { get; set; } = null;
        public string ClosedMessage { get; set; } = null;
        public Dictionary<string, string> LocalizedQuote { get; set; }
        public Dictionary<string, string> LocalizedClosedMessage { get; set; }
    }
}
