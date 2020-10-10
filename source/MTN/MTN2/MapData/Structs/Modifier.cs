/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Dawilly/MTN2
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTN2.MapData {
    public struct Modifier {
        public float Additive { get; set; }
        public float Multipler { get; set; }

        public Modifier(float Value, bool Option = false) {
            Additive = (!Option) ? Value : 0.0f;
            Multipler = (Option) ? Value : 0.0f;
        }

        public Modifier(float ValueAdd, float ValueMult) {
            Additive = ValueAdd;
            Multipler = ValueMult;
        }
    }
}
