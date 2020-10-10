/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/ChroniclerCherry/stardew-valley-mods
**
*************************************************/

using StardewModdingAPI;
using System;

namespace UpgradeEmptyCabins
{
    public partial class UpgradeCabins
    {
        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            var api = Helper.ModRegistry.GetApi<GenericModConfigMenuAPI>("spacechase0.GenericModConfigMenu");

            if (api == null)
                return;

            api.RegisterModConfig(ModManifest, () => _config = new Config(), () => Helper.WriteConfig(_config));
            api.RegisterSimpleOption(ModManifest, "Instance Build", "Whether cabins are instantly upgraded", () => _config.InstantBuild, val => _config.InstantBuild = val);
        }

        private class Config
        {
            public bool InstantBuild { get; set; } = false;
        }
    }

    public interface GenericModConfigMenuAPI
    {
        void RegisterModConfig(IManifest mod, Action revertToDefault, Action saveToFile);
        void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<bool> optionGet, Action<bool> optionSet);
    }
}