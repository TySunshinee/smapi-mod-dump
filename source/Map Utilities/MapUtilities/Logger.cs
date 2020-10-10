/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/mjSurber/Map-Utilities
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace MapUtilities
{
    public static class Logger
    {
        public static IMonitor monitor;

        public static void log(string log, LogLevel level = LogLevel.Info)
        {
            if (monitor == null)
                return;
            monitor.Log(log, level);
        }
    }
}
