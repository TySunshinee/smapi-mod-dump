/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Entoarox/StardewMods
**
*************************************************/

namespace Entoarox.Utilities.Internals.Helpers
{
    using Internals;
    public static class ModApi
    {
        public static bool IsLoadedAndApiAvailable<T>(string mod, out T api) where T : class
        {
            api = null;
            if(EntoUtilsMod.Instance.Helper.ModRegistry.IsLoaded(mod))
                api = EntoUtilsMod.Instance.Helper.ModRegistry.GetApi<T>(mod);
            return api != null;
        }
    }
}
