/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/EKomperud/StardewMods
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowerNPC
{
    public class ModConfig
    {
        public string[] Abigail;
        public string[] Alex;
        public string[] Elliott;
        public string[] Emily;

        public string[] Haley;
        public string[] Harvey;
        public string[] Leah;
        public string[] Maru;

        public string[] Penny;
        public string[] Sam;
        public string[] Sebastian;
        public string[] Shane;

        public int heartThreshold;

        public bool festivalHangouts;

        public ModConfig()
        {
            Abigail = new string[] {"Wed" };
            Alex = new string[] { "Fri" };
            Elliott = new string[] { "Sat" };
            Emily = new string[] { "Sun" };

            Haley = new string[] { "Fri" };
            Harvey = new string[] { "Sat" };
            Leah = new string[] { "Thu" };
            Maru = new string[] { "Wed" };

            Penny = new string[] { "Mon" };
            Sam = new string[] { "Tue" };
            Sebastian = new string[] { "Tue" };
            Shane = new string[] {  "Sun" };

            heartThreshold = 2;

            festivalHangouts = false;
        }
    }
}
