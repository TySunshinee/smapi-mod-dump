/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Sakorona/SDVMods
**
*************************************************/

using StardewModdingAPI;

namespace TwilightShards.TheStarsIncline
{
    public class TheStarsIncline : Mod
    {
        private readonly int UniqueBaseID = 49134570;

        private AstrologicalSigns ZodiacData;
        //It's the age of aquarius. The age of aquarius~~~

        public override void Entry(IModHelper helper)
        {
            //woo!
            Helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
            var ZodiacData = new AstrologicalSigns();
            
        }

        private void GameLoop_DayStarted(object sender, StardewModdingAPI.Events.DayStartedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
