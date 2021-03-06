/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/danvolchek/StardewMods
**
*************************************************/

using Microsoft.Xna.Framework;
using Pong.Framework.Enums;
using Pong.Framework.Game;
using Pong.Framework.Menus;
using Pong.Game.Controllers;
using StardewValley;
using System;

namespace Pong.Game
{
    internal class Paddle : Collider, INonReactiveDrawableCollideable, IResetable
    {
        private readonly IPaddleController controller;

        private readonly Side side;

        public Paddle(IPaddleController controller, Side side) : base(true)
        {
            this.Width = Game1.tileSize * 5;
            this.Height = Game1.tileSize / 2;

            this.controller = controller;
            this.side = side;

            this.ResetPos();
        }

        public override void Update()
        {
            this.controller.Update();
            this.XPos += this.controller.GetMovement(this.XPos, this.Width);
        }

        public CollideInfo GetCollideInfo(IReactiveDrawableCollideable other)
        {
            Rectangle otherPos = other.Bounds;
            return new CollideInfo(Orientation.Horizontal, Math.Max(0, (otherPos.X + otherPos.Width / 2.0 - this.XPos) / this.Width));
        }

        private void ResetPos()
        {
            this.XPos = (Menu.ScreenWidth - this.Width) / 2;
            this.YPos = this.side == Side.Bottom ? Menu.ScreenHeight - this.Height : 0;
        }

        public void Resize()
        {
            this.ResetPos();
        }

        public void Reset()
        {
            this.ResetPos();
        }
    }
}
