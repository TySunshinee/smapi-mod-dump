/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/danvolchek/StardewMods
**
*************************************************/

using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley.TerrainFeatures;
using System;

namespace WindEffects.Framework.Shakers
{
    internal class GrassShaker : IShaker
    {
        private readonly Grass grass;
        private readonly bool left;

        public GrassShaker(Grass grass, bool left)
        {
            this.grass = grass;
            this.left = left;
        }

        public void Shake(IReflectionHelper helper, Vector2 tile)
        {
            helper.GetMethod(grass, "shake").Invoke(3f * (float)Math.PI / 32f, (float)Math.PI / 80f, this.left);
            
            // works, but too loud
            // Game1.currentLocation.localSoundAt("grassyStep", tile);
        }
    }
}
