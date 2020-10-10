/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/aEnigmatic/StardewValley
**
*************************************************/

namespace ConvenientChests.CategorizeChests.Framework.Persistence
{
    interface ISaveManager
    {
        void Save(string relativePath);
        void Load(string relativePath);
    }
}