/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/danvolchek/StardewMods
**
*************************************************/

using StardewModdingAPI;

namespace ChatCommands.Commands
{
    /// <summary>Command interface.</summary>
    internal interface ICommand
    {
        void Register(ICommandHelper helper);
    }
}
