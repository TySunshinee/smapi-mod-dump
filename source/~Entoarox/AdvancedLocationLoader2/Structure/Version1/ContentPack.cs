/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Entoarox/StardewMods
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entoarox.AdvancedLocationLoader2.Structure.Version1
{
    class ContentPack
    {
#pragma warning disable CS0649
        public int Version;
        public string[] Includes=new string[0];
        public MapFileLink[] Locations = new MapFileLink[0];
        public MapFileLink[] Tilesheets = new MapFileLink[0];
        public Warp[] Warps = new Warp[0];
        public Patch[] Patches = new Patch[0];
        public TileEdit[] TileEdits = new TileEdit[0];
        public Shop[] Shops = new Shop[0];
#pragma warning restore CS0649
    }
}
