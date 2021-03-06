/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Esca-MMC/FarmTypeManager
**
*************************************************/

using StardewModdingAPI;

namespace FarmTypeManager
{
    public partial class ModEntry : Mod
    {
        //a subclass of "SpawnArea" specifically for large object generation, including settings for which object types to spawn & a one-time switch to find and respawn pre-existing objects
        private class LargeObjectSpawnArea : SpawnArea
        {
            public string[] ObjectTypes { get; set; } = new string[] { "Stump" };
            public bool FindExistingObjectLocations { get; set; } = true;
            public int PercentExtraSpawnsPerSkillLevel { get; set; } = 0;
            public string RelatedSkill { get; set; } = "Foraging";

            //default constructor, providing settings for hardwood stump respawning (roughly similar to the Forest Farm)
            public LargeObjectSpawnArea()
                : base()
            {
                MapName = "Farm";
                MinimumSpawnsPerDay = 999;
                MaximumSpawnsPerDay = 999;
                IncludeTerrainTypes = new string[0];
                StrictTileChecking = "Maximum";
            }
        }
    }
}