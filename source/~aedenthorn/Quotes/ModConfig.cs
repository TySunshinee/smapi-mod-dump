/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/aedenthorn/StardewValleyMods
**
*************************************************/

using Microsoft.Xna.Framework;
using Netcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quotes
{
    public class ModConfig
    {
        public bool EnableMod { get; set; } = true;
        public Color QuoteColor { get; set; } = Color.DarkSeaGreen;
        public int QuoteCharPerLine { get; set; } = 50;
        public float QuoteDurationPerLineMult { get; set; } = 1f;
        public int QuoteFadeMult { get; set; } = 1;
        public string AuthorPrefix { get; set; } = "-- ";
        public bool ClickToDispelQuote { get; set; } = true;
        public bool RandomQuote { get; set; } = false;
        public int QuoteWidth { get; set; } = 1000;
        public int LineSpacing { get; set; } = 10;
    }
}
