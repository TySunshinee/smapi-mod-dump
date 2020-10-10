/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/JPANv2/Stardew-Valley-Tree-Changes
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace TreeChanges.TreeChanges
{
    public interface SpaceCoreAPI
    {
        string[] GetCustomSkills();
        int GetLevelForCustomSkill(Farmer farmer, string skill);
    }
}
