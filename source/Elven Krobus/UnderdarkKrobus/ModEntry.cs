/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/KirbyLink/UnderdarkKrobus
**
*************************************************/

using System;
using Harmony;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;

namespace UnderdarkSewer
{

    public class ModEntry : Mod
    {

        public override void Entry(IModHelper helper)
        {
            var harmony = HarmonyInstance.Create("com.github.kirbylink.underdarkkrobus");
            var original = typeof(Sewer).GetConstructor(new Type[] { typeof(string), typeof(string) });
            var constructorPostfix = helper.Reflection.GetMethod(typeof(SewerMapFix), "ConstructorPostfix").MethodInfo;
            harmony.Patch(original, null, new HarmonyMethod(constructorPostfix));
        }
    }

    public static class SewerMapFix
    {
        static void ConstructorPostfix(Sewer __instance)
        {
            var krobusfield = AccessTools.Field(typeof(Sewer), "Krobus");
            (krobusfield.GetValue(__instance) as NPC).Sprite = new AnimatedSprite("Characters\\Krobus", 0, 16, 32);
        }
    }
}
