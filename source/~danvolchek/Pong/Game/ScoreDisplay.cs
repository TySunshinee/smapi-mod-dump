/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/danvolchek/StardewMods
**
*************************************************/

using Microsoft.Xna.Framework.Graphics;
using Pong.Framework.Common;
using Pong.Framework.Game.States;
using StardewValley.BellsAndWhistles;

namespace Pong.Game
{
    internal class ScoreDisplay : IUpdateable, IDrawable
    {
        private readonly ScoreState state;

        public ScoreDisplay(ScoreState state)
        {
            this.state = state;
        }

        public void UpdateScore(bool playerWon)
        {
            if (playerWon)
                this.state.PlayerOneScore++;
            else
                this.state.PlayerTwoScore++;
        }

        public void Draw(SpriteBatch b)
        {
            SpriteText.drawString(b, $"{this.state.PlayerOneScore} - {this.state.PlayerTwoScore}", 50, 50, 999999, -1, 999999, 1f, 0.88f, false, -1, "", SpriteText.color_White);
        }

        public void Reset()
        {
            this.state.Reset();
        }

        public void Update()
        {
        }
    }
}
