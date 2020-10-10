/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/EpicBellyFlop45/StardewMods
**
*************************************************/

namespace MoreGrass.Patches
{
    /// <summary>Contains patches for patching code in the WinterGrass mod.</summary>
    internal class WinterGrassPatch
    {
        /// <summary>This is code that is ran in the Winter Grass mod, this needs to be disabled so this mod can handle textures properly.</summary>
        /// <returns>False meaning the original method will never get ran.</returns>
        internal static bool FixGrassColorPrefix()
        {
            return false;
        }
    }
}
