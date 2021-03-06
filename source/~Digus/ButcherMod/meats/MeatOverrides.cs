/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Digus/StardewValleyMods
**
*************************************************/

using Harmony;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalHusbandryMod.meats
{
    public class MeatOverrides
    {
        public static bool sellToStorePrice(StardewValley.Object __instance, ref int __result)
        {
            if (Game1.player.professions.Contains(0) && __instance.Category == -14)
            {
                float num = (float)(int)((double)__instance.Price * (1.0 + (double)__instance.Quality * 0.25));
                num *= 1.2f;
                __result = (int)num;
                return false;                
            }
            return true;            
        }

        public static bool isPotentialBasicShippedCategory(ref int index, ref string category, ref bool __result)
        {
            int.TryParse(category, out var categoryInt);
            if  (categoryInt == -14)
            {
                __result = true;
                return false;
            }

            return true;
        }

        public static bool countsForShippedCollection(StardewValley.Object __instance, ref bool __result)
        {
            if (__instance.Category == -14)
            {
                __result = true;
                return false;
            }

            return true;
        }
    }
}
