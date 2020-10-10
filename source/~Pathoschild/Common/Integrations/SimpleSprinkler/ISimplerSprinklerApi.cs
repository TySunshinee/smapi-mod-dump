/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Pathoschild/StardewMods
**
*************************************************/

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pathoschild.Stardew.Common.Integrations.SimpleSprinkler
{
    /// <summary>The API provided by the Simple Sprinkler mod.</summary>
    public interface ISimplerSprinklerApi
    {
        /// <summary>Get the relative tile coverage for supported sprinkler IDs (additive to the game's default coverage).</summary>
        IDictionary<int, Vector2[]> GetNewSprinklerCoverage();
    }
}
