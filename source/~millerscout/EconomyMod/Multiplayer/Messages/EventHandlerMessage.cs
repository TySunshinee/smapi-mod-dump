/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/millerscout/StardewMillerMods
**
*************************************************/

namespace EconomyMod.Multiplayer.Messages
{
    public class EventHandlerMessage
    {
        public int tax;
        public bool isMale;

        public EventHandlerMessage(int tax, bool isMale)
        {
            this.tax = tax;
            this.isMale = isMale;
        }
    }
}
