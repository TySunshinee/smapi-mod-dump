/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Pathoschild/StardewMods
**
*************************************************/

using Pathoschild.Stardew.Common;
using StardewModdingAPI;

namespace Pathoschild.Stardew.DebugMode.Framework
{
    /// <summary>A set of raw key bindings.</summary>
    internal class ModConfigRawKeys
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The keys which toggle debug mode.</summary>
        public string ToggleDebug { get; set; } = SButton.OemTilde.ToString();


        /*********
        ** Public fields
        *********/
        /// <summary>Get a parsed representation of the configured controls.</summary>
        /// <param name="input">The API for checking input state.</param>
        /// <param name="monitor">The monitor through which to log an error if a button value is invalid.</param>
        public ModConfigKeys ParseControls(IInputHelper input, IMonitor monitor)
        {
            return new ModConfigKeys(
                toggleDebug: CommonHelper.ParseButtons(this.ToggleDebug, input, monitor, nameof(this.ToggleDebug))
            );
        }
    }
}
