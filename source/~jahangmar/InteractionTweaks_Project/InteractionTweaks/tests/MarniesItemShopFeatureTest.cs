/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/jahangmar/StardewValleyMods
**
*************************************************/

// Copyright (c) 2019 Jahangmar
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.

using NUnit.Framework;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
namespace InteractionTweaks
{
    [TestFixture()]
    public class MarniesItemShopFeatureTest : MarniesItemShopFeature
    {
        private ShopMenu shopMenu;

        [SetUp]
        public void Init()
        {
            Game1.player = new Farmer();
            shopMenu = new ShopMenu(Utility.getAnimalShopStock());
            AddItems(shopMenu);
        }

        [Test()]
        public void ConvertResetItemsEmpty()
        {
            List<Item> items = new List<Item>(Game1.player.items);
            ConvertItems(shopMenu);
            ResetItems(shopMenu);
            for (int i=0; i< Game1.player.items.Count; i++)
             Assert.AreEqual(Game1.player.items[i], items[i]);
        }
    }
}
