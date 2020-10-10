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
    internal class BushShaker : IShaker
    {
        private readonly Bush bush;
        private readonly bool left;

        public BushShaker(Bush bush, bool left)
        {
            this.bush = bush;
            this.left = left;
        }

        public void Shake(IReflectionHelper helper, Vector2 tile)
        {
            // can't just call shake because it drops items. We don't want to drop items.
            // see Bush::shake for the logic this replicates

            // already shaking
            if (helper.GetField<float>(this.bush, "maxShake").GetValue() != 0)
                return;

            helper.GetField<bool>(this.bush, "shakeLeft").SetValue(this.left);
            helper.GetField<float>(this.bush, "maxShake").SetValue((float)Math.PI / 128f);
        }
    }
}
