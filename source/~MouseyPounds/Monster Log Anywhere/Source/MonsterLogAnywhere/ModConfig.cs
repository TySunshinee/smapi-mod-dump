/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/MouseyPounds/stardew-mods
**
*************************************************/

using StardewModdingAPI;

namespace MonsterLogAnywhere
{
    class ModConfig
    {
        public SButton Monster_Log_Keybind { get; set; } = SButton.F6;
        public bool Change_Header_and_Footer { get; set; } = true;
        public string New_Header { get; set; } = "= Monster Eradication Goals =\n Help keep the valley safe! \n----------------------------";
        public string New_Footer { get; set; } = "  - See Gil to claim your rewards.";

    }
}
