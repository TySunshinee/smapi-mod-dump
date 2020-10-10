/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/aedenthorn/StardewValleyMods
**
*************************************************/

using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;

namespace TransparentObjects
{
    public class ModConfig
    {
        public bool EnableMod { get; set; } = true;
        public float MinTransparency { get; set; } = 0.1f;
        public int TransparencyMaxDistance { get; set; } = 192;

    }
}