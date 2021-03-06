/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/aedenthorn/StardewValleyMods
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendlyDivorce
{
    public class ModConfig
    {
        public bool Enabled { get; set; } = true;
        public int PointsAfterDivorce { get; set; } = 2000;
        public bool ComplexDivorce { get; set; } = true;
    }
}
