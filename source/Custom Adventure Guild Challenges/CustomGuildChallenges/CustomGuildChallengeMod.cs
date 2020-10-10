/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/defenthenation/StardewMod-CustomSlayerChallenges
**
*************************************************/

using CustomGuildChallenges.API;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI.Events;

namespace CustomGuildChallenges
{
    /// Mod entry - handles all events and linking custom objects to game state
    public class CustomGuildChallengeMod : Mod
    {
        protected ISaveAnywhereAPI saveAnywhereAPI;
        protected AdventureGuild adventureGuild;
        protected ConfigChallengeHelper challengeHelper;

        internal static IList<ChallengeInfo> VanillaChallenges;

        internal static CustomGuildChallengeMod Instance;


        public ModConfig Config { get; set; }

        /// <summary>
        ///     Sets up the mod and adds commands to the console
        /// </summary>
        /// <param name="helper"></param>
        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();
            challengeHelper = new ConfigChallengeHelper(Helper, Config, Monitor);
            CustomGuildChallengeMod.Instance = this;

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            helper.ConsoleCommands.Add("player_setkills", "Update kill count for a monster type", (command, arguments) =>
            {
                if (arguments.Length != 2)
                {
                    Monitor.Log("Usage: player_setkills \"Monster Name\" integerKillCount ", LogLevel.Warn);
                }
                else if (!int.TryParse(arguments[1], out int killCount))
                {
                    Monitor.Log("Invalid kill count. Use an integer, like 50 or 100. Example: player_setkills \"Green Slime\" 100 ", LogLevel.Warn);
                }
                else
                {
                    int before = Game1.player.stats.getMonstersKilled(arguments[0]);
                    Game1.player.stats.specificMonstersKilled[arguments[0]] = killCount;

                    Monitor.Log(arguments[0] + " kills changed from " + before + " to " + killCount, LogLevel.Info);
                }
            });

            helper.ConsoleCommands.Add("player_getkills", "Get kill count for monster type", (command, arguments) =>
            {
                if(arguments.Length == 0)
                {
                    Monitor.Log("Usage: player_getkills \"Monster Name\"", LogLevel.Warn);
                }
                else
                {
                    Monitor.Log(arguments[0] + "'s killed: " + Game1.player.stats.getMonstersKilled(arguments[0]), LogLevel.Info);
                }
            });

            helper.ConsoleCommands.Add("player_giveitem", "See mod README for item number info", (command, arguments) =>
            {
                int itemStack = 1;

                if (arguments.Length < 2)
                {
                    Monitor.Log("Usage: player_giveitem itemType itemNumber [itemStackCount - optional]", LogLevel.Warn);
                }
                else if (!int.TryParse(arguments[0], out int itemType) || !int.TryParse(arguments[1], out int itemNumber) || (arguments.Length == 3 && !int.TryParse(arguments[2], out itemStack)))
                {
                    Monitor.Log("Invalid item number. Use an integer, like 50 or 100. Example: player_giveitem 0 100 5", LogLevel.Warn);
                }
                else
                {
                    var item = challengeHelper.customAdventureGuild.CreateReward(itemType, itemNumber, itemStack);

                    if (item == null)
                    {
                        Monitor.Log("Invalid item numbers: " + itemType + " " + itemNumber + ". No item was spawned.", LogLevel.Warn);
                    }
                    else
                    {
                        Game1.player.addItemsByMenuIfNecessary(new Item[] { item }.ToList());
                        Monitor.Log("Item " + item.DisplayName + " given to player.", LogLevel.Info);
                    }
                }
            });

            helper.ConsoleCommands.Add("player_getallkills", "Display all kills for all monsters", (command, arguments) =>
            {
                foreach(var item in Game1.player.stats.specificMonstersKilled)
                {
                    Monitor.Log(item.Key + "'s killed: " + item.Value);
                }
            });

            helper.ConsoleCommands.Add("toggle_monsterskilledinfo", "Turn debug statement of monster kill on or off", (command, arguments) =>
            {
                Config.DebugMonsterKills = !Config.DebugMonsterKills;

                string status = Config.DebugMonsterKills ? "Enabled" : "Disabled";
                Monitor.Log("Monsters killed debug info " + status);
            });

            string log = Config.CustomChallengesEnabled ?
                "Initialized (" + Config.Challenges.Count + " custom challenges loaded)" :
                "Initialized (Vanilla challenges loaded)";

            Monitor.Log(log, LogLevel.Debug);
        }

        /// <summary>
        ///     Returns API object to add and remove challenges and update Gil's dialogue
        /// </summary>
        /// <returns>ConfigChallengeHelper</returns>
        public override object GetApi()
        {
            return challengeHelper;
        }

        /// <summary>Raised after the game is launched, right before the first update tick. This happens once per game session (unrelated to loading saves). All mods are loaded and initialised at this point, so this is a good time to set up mod integrations.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // Normalise config
            if (Config.Challenges == null || Config.Challenges.Count == 0)
            {
                Config = new ModConfig
                {
                    CustomChallengesEnabled = false,
                    CountKillsOnFarm = false,
                    DebugMonsterKills = false,
                    Challenges = GetVanillaSlayerChallenges().ToList(),
                    GilNoRewardDialogue = Game1.content.LoadString("Characters\\Dialogue\\Gil:ComeBackLater"),
                    GilSleepingDialogue = Game1.content.LoadString("Characters\\Dialogue\\Gil:Snoring"),
                    GilSpecialGiftDialogue = "Thanks for cleanin' up all those monsters. Figured you deserved somethin' extra special."
                };
                Helper.WriteConfig(Config);
            }
            else if (!Config.CustomChallengesEnabled)
            {
                Config.Challenges = GetVanillaSlayerChallenges().ToList(); // Use vanilla challenges but do not overwrite the config
            }

            // Verify config
            for (int i = 0; i < Config.Challenges.Count; i++)
            {
                for (int j = 0; j < Config.Challenges[i].MonsterNames.Count; j++)
                {
                    if (!Monsters.MonsterList.Contains(Config.Challenges[i].MonsterNames[j]))
                    {
                        Monitor.Log("Warning: Invalid monster name '" + Config.Challenges[i].MonsterNames[j] +
                                    "' found. " + Config.Challenges[i].ChallengeName + " challenge will display but " +
                                    "cannot be completed until monster name is fixed! ", LogLevel.Warn);
                    }
                }

                // TODO: Validate items on startup
            }

            VanillaChallenges = GetVanillaSlayerChallenges();

            // Integrate: Save Anywhere
            if (Helper.ModRegistry.IsLoaded("Omegasis.SaveAnywhere"))
            {
                saveAnywhereAPI = Helper.ModRegistry.GetApi<ISaveAnywhereAPI>("Omegasis.SaveAnywhere");
                Monitor.Log("SaveAnywhere integration running", LogLevel.Trace);

                saveAnywhereAPI.BeforeSave += challengeHelper.PresaveData;
                saveAnywhereAPI.AfterSave += challengeHelper.InjectGuild;
                saveAnywhereAPI.AfterLoad += challengeHelper.InjectGuild;
            }
        }

        /// <summary>
        ///     Returns a list of the vanilla challenges
        /// </summary>
        /// <returns>Vanilla Challenge Info List</returns>
        public static IList<ChallengeInfo> GetVanillaSlayerChallenges()
        {
            var slimeChallenge = new ChallengeInfo()
            {
                ChallengeName = "Slimes",
                RequiredKillCount = 1000,
                MonsterNames = { Monsters.GreenSlime, Monsters.FrostJelly, Monsters.Sludge },
                RewardType = (int)ItemType.Ring,
                RewardItemNumber = (int)Rings.SlimeCharmerRing
            };

            var shadowChallenge = new ChallengeInfo()
            {
                ChallengeName = "Void Spirits",
                RequiredKillCount = 150,
                MonsterNames = { Monsters.ShadowGuy, Monsters.ShadowShaman, Monsters.ShadowBrute },
                RewardType = (int)ItemType.Ring,
                RewardItemNumber = (int)Rings.SavageRing
            };

            var skeletonChallenge = new ChallengeInfo()
            {
                ChallengeName = "Skeletons",
                RequiredKillCount = 50,
                MonsterNames = { Monsters.Skeleton, Monsters.SkeletonMage, Monsters.SkeletonWarrior },
                RewardType = (int)ItemType.Hat,
                RewardItemNumber = (int)Hats.SkeletonMask
            };

            var caveInsectsChallenge = new ChallengeInfo()
            {
                ChallengeName = "Cave Insects",
                RequiredKillCount = 125,
                MonsterNames = { Monsters.Bug, Monsters.Grub, Monsters.Fly, Monsters.MutantGrub, Monsters.MutantFly },
                RewardType = (int)ItemType.Weapon,
                RewardItemNumber = (int)Weapons.InsectHead
            };

            var duggyChallenge = new ChallengeInfo()
            {
                ChallengeName = "Duggies",
                RequiredKillCount = 30,
                MonsterNames = { Monsters.Duggy },
                RewardType = (int)ItemType.Hat,
                RewardItemNumber = (int)Hats.HardHat
            };

            var batChallenge = new ChallengeInfo()
            {
                ChallengeName = "Bats",
                RequiredKillCount = 200,
                MonsterNames = { Monsters.Bat, Monsters.FrostBat, Monsters.LavaBat, Monsters.IridiumBat },
                RewardType = (int)ItemType.Ring,
                RewardItemNumber = (int)Rings.VampireRing
            };

            var dustSpiritChallenge = new ChallengeInfo()
            {
                ChallengeName = "Dust Spirits",
                RequiredKillCount = 500,
                MonsterNames = { Monsters.DustSpirit },
                RewardType = (int)ItemType.Ring,
                RewardItemNumber = (int)Rings.BurglarsRing
            };

            var rockCrabChallenge = new ChallengeInfo()
            {
                ChallengeName = "Rock Crabs",
                RequiredKillCount = 60,
                MonsterNames = { Monsters.RockCrab, Monsters.LavaCrab, Monsters.IridiumCrab },
                RewardType = (int)ItemType.Ring,
                RewardItemNumber = (int)Rings.CrabshellRing
            };

            var mummyChallenge = new ChallengeInfo()
            {
                ChallengeName = "Mummy",
                RequiredKillCount = 100,
                MonsterNames = { Monsters.Mummy },
                RewardType = (int)ItemType.Hat,
                RewardItemNumber = (int)Hats.ArcaneHat
            };

            var pepperRexChallenge = new ChallengeInfo()
            {
                ChallengeName = "Pepper Rex",
                RequiredKillCount = 50,
                MonsterNames = { Monsters.PepperRex },
                RewardType = (int)ItemType.Hat,
                RewardItemNumber = (int)Hats.KnightsHelmet
            };

            var serpentChallenge = new ChallengeInfo()
            {
                ChallengeName = "Serpent",
                RequiredKillCount = 250,
                MonsterNames = { Monsters.Serpent },
                RewardType = (int)ItemType.Ring,
                RewardItemNumber = (int)Rings.NapalmRing
            };

            return new List<ChallengeInfo>()
            {
                slimeChallenge,
                shadowChallenge,
                skeletonChallenge,
                caveInsectsChallenge,
                duggyChallenge,
                batChallenge,
                dustSpiritChallenge,
                rockCrabChallenge,
                mummyChallenge,
                pepperRexChallenge,
                serpentChallenge
            };
        }
    }
}
