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
using Microsoft.Xna.Framework.Graphics;
using Pong.Framework.Common;
using Pong.Framework.Menus.Elements;

namespace Pong.Framework.Extensions
{
    internal static class IDrawableExtensions
    {
        public static void DrawBorder(this IBoundable boundable, SpriteBatch b)
        {
            b.Draw(AssetManager.SquareTexture, new Rectangle(boundable.Bounds.X - StaticTextElement.HighlightWidth, boundable.Bounds.Y - StaticTextElement.HighlightWidth, StaticTextElement.HighlightWidth / 2, boundable.Bounds.Height + StaticTextElement.HighlightWidth), Color.White);
            b.Draw(AssetManager.SquareTexture, new Rectangle(boundable.Bounds.X - StaticTextElement.HighlightWidth, boundable.Bounds.Y - StaticTextElement.HighlightWidth, boundable.Bounds.Width + StaticTextElement.HighlightWidth, StaticTextElement.HighlightWidth / 2), Color.White);
            b.Draw(AssetManager.SquareTexture, new Rectangle(boundable.Bounds.X + boundable.Bounds.Width, boundable.Bounds.Y - StaticTextElement.HighlightWidth, StaticTextElement.HighlightWidth / 2, boundable.Bounds.Height + StaticTextElement.HighlightWidth), Color.White);
            b.Draw(AssetManager.SquareTexture, new Rectangle(boundable.Bounds.X - StaticTextElement.HighlightWidth, boundable.Bounds.Y + boundable.Bounds.Height, boundable.Bounds.Width + (int)(StaticTextElement.HighlightWidth * 1.5), StaticTextElement.HighlightWidth / 2), Color.White);
        }
    }
}
