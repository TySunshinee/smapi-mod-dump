/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/KabigonFirst/StardewValleyMods
**
*************************************************/

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kisekae.Menu {
    interface IAutoComponent {
        string m_name { get; set; }
        bool m_visible { get; set; }
        void draw(SpriteBatch b);
        bool containsPoint(int x, int y);
    }
}
