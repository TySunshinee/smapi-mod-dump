/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/TehPers/StardewValleyMods
**
*************************************************/

using Microsoft.Xna.Framework;
using StardewValley;

namespace TehPers.CoreMod.Items.Machines {
    internal readonly struct LocationPosition {
        public GameLocation Location { get; }
        public Vector2 Position { get; }

        public LocationPosition(GameLocation location, Vector2 position) {
            this.Location = location;
            this.Position = position;
        }
    }
}