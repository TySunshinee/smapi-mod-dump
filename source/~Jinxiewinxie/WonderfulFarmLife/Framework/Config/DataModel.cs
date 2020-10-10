/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Jinxiewinxie/StardewValleyMods
**
*************************************************/

using System;
using System.Collections.Generic;
using WonderfulFarmLife.Framework.Constants;

namespace WonderfulFarmLife.Framework.Config
{
    /// <summary>Configures the overrides applied to the farm maps.</summary>
    internal class DataModel
    {
        /*********
        ** Accessors
        *********/
        /// <summary>Short names for the tilesheets listed in the .tbin file (as alias => tilesheet ID). The one named 'default' should be used if no tilesheet is specified.</summary>
        public Dictionary<string, string> Tilesheets { get; set; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>The layout settings for each map.</summary>
        public Dictionary<FarmType, LayoutConfig[]> Layouts { get; set; }
    }
}
