/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Ilyaki/BattleRoyalley
**
*************************************************/

using StardewValley;

namespace BattleRoyale
{
    class EventDisabler : Patch
	{
		protected override PatchDescriptor GetPatchDescriptor() => new PatchDescriptor(typeof(GameLocation), "checkForEvents");

		public static bool Prefix()
		{
			return false;// !ModEntry.game.IsGameInProgress;
		}
	}
}
