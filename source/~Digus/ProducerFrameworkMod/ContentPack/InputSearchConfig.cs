/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Digus/StardewValleyMods
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerFrameworkMod.ContentPack
{
    public class InputSearchConfig
    {
        public int Range = -1;
        public List<string> InputIdentifier = new List<string>();
        public bool Crop;
        public bool ExcludeForageCrops;
        public bool GardenPot;
        public bool FruitTree;
        public bool BigCraftable;

    }
}
