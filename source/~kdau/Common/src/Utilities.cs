/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://gitlab.com/kdau/predictivemods
**
*************************************************/

using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PredictiveCore
{
	public class WorldUnreadyException : InvalidOperationException
	{
		public WorldUnreadyException ()
			: base ("The world is not ready.")
		{}
	}

	public interface IConfig
	{
		bool InaccuratePredictions { get; }
	}
	public delegate IConfig ConfigProvider ();

	public static class Utilities
	{
		// Parses a list of three console arguments (year, season, day) as an
		// SDate. If the list is empty, returns the current date.
		public static SDate ArgsToSDate (List<string> args)
		{
			switch (args.Count)
			{
			case 0:
				return SDate.Now ();
			case 3:
				break;
			default:
				throw new ArgumentException ("Wrong number of arguments.");
			}

			if (!int.TryParse (args[0], out int year) || year < 1)
			{
				throw new ArgumentException ($"Invalid year '{args[0]}', must be a number 1 or higher.");
			}

			string season = args[1];
			if (Utility.getSeasonNumber (season) == -1)
			{
				throw new ArgumentException ($"Invalid season '{args[1]}', must be 'spring', 'summer', 'fall' or 'winter'.");
			}

			if (!int.TryParse (args[2], out int day) || day < 1 || day > 28)
			{
				throw new ArgumentException ($"Invalid day '{args[2]}', must be a number from 1 to 28.");
			}

			return new SDate (year, season, day);
		}

		// Returns the first day of the next season after a given date.
		public static SDate GetNextSeasonStart (SDate date)
		{
			return date.Season switch
			{
				"spring" => new SDate (1, "summer", date.Year),
				"summer" => new SDate (1, "fall", date.Year),
				"fall"   => new SDate (1, "winter", date.Year),
				"winter" => new SDate (1, "spring", date.Year + 1),
				_        => throw new ArgumentOutOfRangeException ("invalid season"),
			};
		}

		// Returns the CultureInfo for the current game language.
		public static CultureInfo GetCurrentCulture ()
		{
			string langCode = Game1.content.LanguageCodeString
				(Game1.content.GetCurrentLanguage ());
			return new CultureInfo (langCode);
		}

		// Returns the localized name of the day of the week for a given date.
		public static string GetLocalizedDayOfWeek (SDate date,
			bool shortName = false)
		{
			if (shortName)
			{
				return Game1.shortDayDisplayNameFromDayOfSeason (date.Day);
			}
			else
			{
				// February 1999 was a Stardew month: 28 days starting on Monday
				return new DateTime (1999, 2, date.Day).ToString ("dddd",
					GetCurrentCulture ());
			}
		}

		// Limits prediction calculations to 50 years from start date.
		internal const int MaxHorizon = 28 * 4 * 50;

		internal static IMonitor Monitor;
		internal static IModHelper Helper;

		private static ConfigProvider ConfigProvider;
		internal static IConfig Config => ConfigProvider ();

		public static void Initialize (IMod mod, ConfigProvider config)
		{
			if (Monitor != null)
				return;
			Monitor = mod.Monitor;
			Helper = mod.Helper;
			ConfigProvider = config;

			if (new SemanticVersion (Game1.version).IsOlderThan ("1.4.0") ||
				!new SemanticVersion (Game1.version).IsOlderThan ("1.5.0"))
			{
				Monitor.Log ($"This mod version was not designed for game version {Game1.version}. Predictions will be inaccurate until the mod is updated.", LogLevel.Alert);
			}

			// If multiple mods are consuming PredictiveCore, only add the
			// console commands in one of them (arbitrarily, PublicAccessTV).
			bool addConsoleCommands =
				mod.ModManifest.UniqueID == "kdau.PublicAccessTV" ||
				!Helper.ModRegistry.IsLoaded ("kdau.PublicAccessTV");

			Garbage.Initialize (addConsoleCommands);
			Geodes.Initialize (addConsoleCommands);
			// TODO: ItemFinder.Initialize (addConsoleCommands);
			Mining.Initialize (addConsoleCommands);
			Movies.Initialize (addConsoleCommands);
			NightEvents.Initialize (addConsoleCommands);
			// TODO: Shopping.Initialize (addConsoleCommands);
			// TODO: Tailoring.Initialize (addConsoleCommands);
			Trains.Initialize (addConsoleCommands);
		}

		internal static void CheckWorldReady ()
		{
			if (!Context.IsWorldReady)
				throw new WorldUnreadyException ();
		}
	}
}
