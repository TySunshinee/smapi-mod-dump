/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Entoarox/StardewMods
**
*************************************************/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Entoarox.Framework.UI
{
    public class ClickableHeartsComponent : BaseInteractiveMenuComponent
    {
        /*********
        ** Fields
        *********/
        protected static readonly Rectangle HeartEmpty = new Rectangle(218, 428, 7, 6);
        protected static readonly Rectangle HeartFull = new Rectangle(211, 428, 7, 6);
        protected int _Value;
        protected bool Hovered;
        protected int MaxValue;
        protected int OldValue;


        /*********
        ** Accessors
        *********/
        public int Value
        {
            get => this._Value;
            set => this._Value = Math.Min(Math.Max(0, value), this.MaxValue);
        }

        public event ValueChanged<int> Handler;


        /*********
        ** Public methods
        *********/
        public ClickableHeartsComponent(Point position, int value, int maxValue, ValueChanged<int> handler = null)
        {
            if (maxValue % 2 != 0)
                maxValue++;
            this.SetScaledArea(new Rectangle(position.X, position.Y, 8 * (maxValue / 2), ClickableHeartsComponent.HeartEmpty.Height));
            this.MaxValue = maxValue;
            this.Value = value;
            this.OldValue = this.Value;
            if (handler != null)
                this.Handler += handler;
        }

        public override void HoverIn(Point p, Point o)
        {
            this.Hovered = true;
        }

        public override void HoverOut(Point p, Point o)
        {
            this.Hovered = false;
        }

        public override void LeftUp(Point p, Point o)
        {
            this.Value = (int)Math.Round((p.X - (this.Area.X + o.X)) / 4D / Game1.pixelZoom);
            if (this.OldValue == this.Value)
                return;
            this.OldValue = this.Value;
            this.Handler?.Invoke(this, this.Parent, this.Parent.GetAttachedMenu(), this.Value);
        }

        public override void Draw(SpriteBatch b, Point o)
        {
            if (!this.Visible)
                return;
            for (int c = 0; c < this.MaxValue / 2; c++)
            {
                b.Draw(Game1.mouseCursors, new Vector2(o.X + this.Area.X + Game1.pixelZoom + c * BaseMenuComponent.Zoom8, o.Y + this.Area.Y), new Rectangle(ClickableHeartsComponent.HeartEmpty.X, ClickableHeartsComponent.HeartEmpty.Y, 7, 6), Color.White, 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
            }

            for (int c = 0; c < this.Value; c++)
            {
                b.Draw(Game1.mouseCursors, new Vector2(o.X + this.Area.X + Game1.pixelZoom + c * BaseMenuComponent.Zoom4, o.Y + this.Area.Y), new Rectangle(ClickableHeartsComponent.HeartFull.X + (c % 2 == 0 ? 0 : 4), ClickableHeartsComponent.HeartFull.Y, c % 2 == 0 ? 4 : 3, 6), Color.White * (this.Hovered ? 0.5f : 1), 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
            }

            if (!this.Hovered)
                return;
            int value = Math.Min(this.MaxValue, (int)Math.Round((Game1.getMouseX() - (this.Area.X + o.X)) / 4D / Game1.pixelZoom));
            for (int c = 0; c < value; c++)
            {
                b.Draw(Game1.mouseCursors, new Vector2(o.X + this.Area.X + Game1.pixelZoom + c * BaseMenuComponent.Zoom4, o.Y + this.Area.Y), new Rectangle(ClickableHeartsComponent.HeartFull.X + (c % 2 == 0 ? 0 : 4), ClickableHeartsComponent.HeartFull.Y, c % 2 == 0 ? 4 : 3, 6), Color.White, 0, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 1f);
            }
        }
    }
}
