/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/hawkfalcon/Stardew-Mods
**
*************************************************/

using BetterJunimos.Utils;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace BetterJunimos.Abilities {
    public class HarvestBushesAbility : IJunimoAbility {
        public string AbilityName() {
            return "HarvestBushes";
        }

        public bool IsActionAvailable(Farm farm, Vector2 pos) {
            if (farm.terrainFeatures.ContainsKey(pos) && farm.terrainFeatures[pos] is Bush bush) {
                return bush.tileSheetOffset.Value == 1;
            }
            return false;
        }

        public bool PerformAction(Farm farm, Vector2 pos, JunimoHarvester junimo, Chest chest) {
            // Don't do anything, as the base junimo handles this already (see PatchHarvestAttemptToCustom)
            return true;
        }

        public int RequiredItem() {
            return 0;
        }
    }
}