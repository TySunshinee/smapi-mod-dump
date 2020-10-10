/*************************************************
**
** You're viewing a file in the SMAPI mod dump, which contains a copy of every open-source SMAPI mod
** for queries and analysis.
**
** This is *not* the original file, and not necessarily the latest version.
** Source repository: https://github.com/Pathoschild/StardewMods
**
*************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using Pathoschild.Stardew.Automate.Framework;
using Pathoschild.Stardew.Automate.Framework.Machines.Buildings;
using Pathoschild.Stardew.Automate.Framework.Models;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.Messages;
using Pathoschild.Stardew.Common.Utilities;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;

namespace Pathoschild.Stardew.Automate
{
    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod configuration.</summary>
        private ModConfig Config;

        /// <summary>The configured key bindings.</summary>
        private ModConfigKeys Keys;

        /// <summary>Constructs machine groups.</summary>
        private MachineGroupFactory Factory;

        /// <summary>Handles console commands from players.</summary>
        private CommandHandler CommandHandler;

        /// <summary>Whether to enable automation for the current save.</summary>
        private bool EnableAutomation => Context.IsMainPlayer;

        /// <summary>The machines to process.</summary>
        private readonly IDictionary<GameLocation, MachineGroup[]> ActiveMachineGroups = new Dictionary<GameLocation, MachineGroup[]>(new ObjectReferenceComparer<GameLocation>());

        /// <summary>The disabled machine groups (e.g. machines not connected to a chest).</summary>
        private readonly IDictionary<GameLocation, MachineGroup[]> DisabledMachineGroups = new Dictionary<GameLocation, MachineGroup[]>(new ObjectReferenceComparer<GameLocation>());

        /// <summary>The locations that should be reloaded on the next update tick.</summary>
        private readonly HashSet<GameLocation> ReloadQueue = new HashSet<GameLocation>(new ObjectReferenceComparer<GameLocation>());

        /// <summary>The number of ticks until the next automation cycle.</summary>
        private int AutomateCountdown;

        /// <summary>The current overlay being displayed, if any.</summary>
        private OverlayMenu CurrentOverlay;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides methods for interacting with the mod directory, such as read/writing a config file or custom JSON files.</param>
        public override void Entry(IModHelper helper)
        {
            // read data file
            const string dataPath = "assets/data.json";
            DataModel data = null;
            try
            {
                data = this.Helper.Data.ReadJsonFile<DataModel>(dataPath);
                if (data?.FloorNames == null)
                    this.Monitor.Log($"The {dataPath} file seems to be missing or invalid. Floor connectors will be disabled.", LogLevel.Error);
            }
            catch (Exception ex)
            {
                this.Monitor.Log($"The {dataPath} file seems to be invalid. Floor connectors will be disabled.\n{ex}", LogLevel.Error);
            }

            // read config
            this.Config = this.LoadConfig();

            // init
            this.Keys = this.Config.Controls.ParseControls(helper.Input, this.Monitor);
            this.Factory = new MachineGroupFactory(this.Config);
            this.Factory.Add(new AutomationFactory(
                connectors: this.Config.ConnectorNames,
                monitor: this.Monitor,
                reflection: helper.Reflection,
                data: data,
                betterJunimosCompat: this.Config.ModCompatibility.BetterJunimos && helper.ModRegistry.IsLoaded("hawkfalcon.BetterJunimos"),
                autoGrabberModCompat: this.Config.ModCompatibility.AutoGrabberMod && helper.ModRegistry.IsLoaded("Jotser.AutoGrabberMod"),
                pullGemstonesFromJunimoHuts: this.Config.PullGemstonesFromJunimoHuts
            ));
            this.CommandHandler = new CommandHandler(this.Monitor, this.Config, this.Factory, this.ActiveMachineGroups);

            // hook events
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.Player.Warped += this.OnWarped;
            helper.Events.World.BuildingListChanged += this.OnBuildingListChanged;
            helper.Events.World.LocationListChanged += this.OnLocationListChanged;
            helper.Events.World.ObjectListChanged += this.OnObjectListChanged;
            helper.Events.World.TerrainFeatureListChanged += this.OnTerrainFeatureListChanged;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Multiplayer.ModMessageReceived += this.OnModMessageReceived;

            // hook commands
            helper.ConsoleCommands.Add("automate", "Run commands from the Automate mod. Enter 'automate help' for more info.", this.CommandHandler.HandleCommand);

            // log info
            this.Monitor.VerboseLog($"Initialized with automation every {this.Config.AutomationInterval} ticks.");
        }

        /// <summary>Get an API that other mods can access. This is always called after <see cref="Entry" />.</summary>
        public override object GetApi()
        {
            return new AutomateAPI(this.Monitor, this.Factory, this.ActiveMachineGroups, this.DisabledMachineGroups);
        }


        /*********
        ** Private methods
        *********/
        /****
        ** Event handlers
        ****/
        /// <summary>The method invoked when the player loads a save.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            // disable if secondary player
            if (!this.EnableAutomation)
                this.Monitor.Log("Disabled automation (only the main player can automate machines in multiplayer mode).", LogLevel.Warn);

            // reset
            this.ActiveMachineGroups.Clear();
            this.DisabledMachineGroups.Clear();
            this.AutomateCountdown = this.Config.AutomationInterval;
            this.DisableOverlay();
            foreach (GameLocation location in CommonHelper.GetLocations())
                this.ReloadQueue.Add(location);
        }

        /// <summary>The method invoked when the player warps to a new location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnWarped(object sender, WarpedEventArgs e)
        {
            if (e.IsLocalPlayer)
                this.ResetOverlayIfShown();
        }

        /// <summary>The method invoked when a location is added or removed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLocationListChanged(object sender, LocationListChangedEventArgs e)
        {
            if (!this.EnableAutomation)
                return;

            this.Monitor.VerboseLog("Location list changed, reloading machines in affected locations.");

            try
            {
                // remove locations
                foreach (GameLocation location in e.Removed)
                {
                    this.ActiveMachineGroups.Remove(location);
                    this.DisabledMachineGroups.Remove(location);
                }

                // add locations
                foreach (GameLocation location in e.Added)
                    this.ReloadQueue.Add(location);
            }
            catch (Exception ex)
            {
                this.HandleError(ex, "updating locations");
            }
        }

        /// <summary>The method raised after buildings are added or removed in a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnBuildingListChanged(object sender, BuildingListChangedEventArgs e)
        {
            if (!this.EnableAutomation)
                return;

            if (e.Location is BuildableGameLocation buildableLocation && e.Added.Concat(e.Removed).Any(building => this.Factory.IsAutomatable(buildableLocation, new Vector2(building.tileX.Value, building.tileY.Value), building)))
            {
                this.Monitor.VerboseLog($"Building list changed in {e.Location.Name}, reloading its machines.");
                this.ReloadQueue.Add(e.Location);
            }
        }

        /// <summary>The method invoked when an object is added or removed to a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
            if (!this.EnableAutomation)
                return;

            if (e.Added.Concat(e.Removed).Any(obj => this.Factory.IsAutomatable(e.Location, obj.Key, obj.Value)))
            {
                this.Monitor.VerboseLog($"Object list changed in {e.Location.Name}, reloading its machines.");
                this.ReloadQueue.Add(e.Location);
            }
        }

        /// <summary>The method invoked when a terrain feature is added or removed to a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTerrainFeatureListChanged(object sender, TerrainFeatureListChangedEventArgs e)
        {
            if (!this.EnableAutomation)
                return;

            if (e.Added.Concat(e.Removed).Any(obj => this.Factory.IsAutomatable(e.Location, obj.Key, obj.Value)))
            {
                this.Monitor.VerboseLog($"Terrain feature list changed in {e.Location.Name}, reloading its machines.");
                this.ReloadQueue.Add(e.Location);
            }
        }

        /// <summary>The method invoked when the in-game clock time changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || !this.EnableAutomation)
                return;

            try
            {
                // handle delay
                this.AutomateCountdown--;
                if (this.AutomateCountdown > 0)
                    return;
                this.AutomateCountdown = this.Config.AutomationInterval;

                // reload machines if needed
                if (this.ReloadQueue.Any())
                {
                    foreach (GameLocation location in this.ReloadQueue)
                        this.ReloadMachinesIn(location);
                    this.ReloadQueue.Clear();

                    this.ResetOverlayIfShown();
                }

                // process machines
                foreach (MachineGroup group in this.GetActiveMachineGroups())
                    group.Automate();
            }
            catch (Exception ex)
            {
                this.HandleError(ex, "processing machines");
            }
        }

        /// <summary>The method invoked when the player presses a button.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                // toggle overlay
                if (Context.IsPlayerFree && this.Keys.ToggleOverlay.JustPressedUnique())
                {
                    if (this.CurrentOverlay != null)
                        this.DisableOverlay();
                    else
                        this.EnableOverlay();
                }
            }
            catch (Exception ex)
            {
                this.HandleError(ex, "handling key input");
            }
        }

        /// <summary>Raised after a mod message is received over the network.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            // update automation if chest options changed
            if (Context.IsMainPlayer && e.FromModID == "Pathoschild.ChestsAnywhere" && e.Type == nameof(AutomateUpdateChestMessage))
            {
                var message = e.ReadAs<AutomateUpdateChestMessage>();
                var location = Game1.getLocationFromName(message.LocationName);
                var player = Game1.getFarmer(e.FromPlayerID);

                string label = player != Game1.MasterPlayer
                    ? $"{player.Name}/{e.FromModID}"
                    : e.FromModID;

                if (location != null)
                {
                    this.Monitor.Log($"Received chest update from {label} for chest at {message.LocationName} ({message.Tile}), updating machines.");
                    this.ReloadQueue.Add(location);
                }
                else
                    this.Monitor.Log($"Received chest update from {label} for chest at {message.LocationName} ({message.Tile}), but no such location was found.");
            }
        }

        /****
        ** Methods
        ****/
        /// <summary>Read the config file, migrating legacy settings if applicable.</summary>
        private ModConfig LoadConfig()
        {
            // read raw config
            var config = this.Helper.ReadConfig<ModConfig>();
            bool changed = false;

            // normalize machine settings
            config.MachineOverrides = new Dictionary<string, ModConfigMachine>(config.MachineOverrides ?? new Dictionary<string, ModConfigMachine>(), StringComparer.OrdinalIgnoreCase);
            foreach (string key in config.MachineOverrides.Where(p => p.Value == null).Select(p => p.Key).ToArray())
            {
                config.MachineOverrides.Remove(key);
                changed = true;
            }

            // migrate legacy fields
            if (config.ExtensionFields != null)
            {
                // migrate AutomateShippingBin (1.10.4–1.17.3)
                try
                {
                    if (config.ExtensionFields.TryGetValue("AutomateShippingBin", out JToken raw))
                        config.GetOrAddMachineOverrides(ShippingBinMachine.ShippingBinId).Enabled = raw.ToObject<bool>();
                }
                catch (Exception ex)
                {
                    this.Monitor.Log($"Failed migrating legacy 'AutomateShippingBin' config field, ignoring previous value.\n\n{ex}", LogLevel.Warn);
                }

                // migrate MachinePriority field (1.17–1.17.3) to MachineSettings
                // (and fix wrong "ShippingBinMachine" default value)
                try
                {
                    if (config.ExtensionFields.TryGetValue("MachinePriority", out JToken raw))
                    {
                        var priorities = raw.ToObject<Dictionary<string, int>>() ?? new Dictionary<string, int>();
                        foreach (var pair in priorities)
                        {
                            string key = pair.Key == "ShippingBinMachine" ? ShippingBinMachine.ShippingBinId : pair.Key;
                            config.GetOrAddMachineOverrides(key).Priority = pair.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Monitor.Log($"Failed migrating legacy 'MachinePriority' config field, ignoring previous value.\n\n{ex}", LogLevel.Warn);
                }

                config.ExtensionFields.Clear();
                changed = true;
            }

            // resave changes
            if (changed)
                this.Helper.WriteConfig(config);

            return config;
        }

        /// <summary>Get the active machine groups in every location.</summary>
        private IEnumerable<MachineGroup> GetActiveMachineGroups()
        {
            foreach (KeyValuePair<GameLocation, MachineGroup[]> group in this.ActiveMachineGroups)
            {
                foreach (MachineGroup machineGroup in group.Value)
                    yield return machineGroup;
            }
        }

        /// <summary>Reload the machines in a given location.</summary>
        /// <param name="location">The location whose machines to reload.</param>
        private void ReloadMachinesIn(GameLocation location)
        {
            this.Monitor.VerboseLog($"Reloading machines in {location.Name}...");

            // get machine groups
            MachineGroup[] machineGroups = this.Factory.GetMachineGroups(location).ToArray();
            this.ActiveMachineGroups[location] = machineGroups.Where(p => p.HasInternalAutomation).ToArray();
            this.DisabledMachineGroups[location] = machineGroups.Where(p => !p.HasInternalAutomation).ToArray();

            // remove unneeded entries
            if (!this.ActiveMachineGroups[location].Any())
                this.ActiveMachineGroups.Remove(location);
            if (!this.DisabledMachineGroups[location].Any())
                this.DisabledMachineGroups.Remove(location);
        }

        /// <summary>Log an error and warn the user.</summary>
        /// <param name="ex">The exception to handle.</param>
        /// <param name="verb">The verb describing where the error occurred (e.g. "looking that up").</param>
        private void HandleError(Exception ex, string verb)
        {
            this.Monitor.Log($"Something went wrong {verb}:\n{ex}", LogLevel.Error);
            CommonHelper.ShowErrorMessage($"Huh. Something went wrong {verb}. The error log has the technical details.");
        }

        /// <summary>Disable the overlay, if shown.</summary>
        private void DisableOverlay()
        {
            this.CurrentOverlay?.Dispose();
            this.CurrentOverlay = null;
        }

        /// <summary>Enable the overlay.</summary>
        private void EnableOverlay()
        {
            this.CurrentOverlay ??= new OverlayMenu(this.Helper.Events, this.Helper.Input, this.Helper.Reflection, this.Factory.GetMachineGroups(Game1.currentLocation));
        }

        /// <summary>Reset the overlay if it's being shown.</summary>
        private void ResetOverlayIfShown()
        {
            if (this.CurrentOverlay != null)
            {
                this.DisableOverlay();
                this.EnableOverlay();
            }
        }
    }
}
