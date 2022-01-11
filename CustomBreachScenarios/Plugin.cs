namespace CustomBreachScenarios
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CustomBreachScenarios.API;
    using CustomBreachScenarios.API.Objects;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Loader;
    using UnityEngine;
    using Player = Exiled.Events.Handlers.Player;
    using Server = Exiled.Events.Handlers.Server;

    /// <summary>
    /// Main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        /// <summary>
        /// The <see langword="static"/> instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Singleton;

        /// <summary>
        /// Gets Custom breach scenario path.
        /// </summary>
        public static string CustomBreachScenariosPath { get; } = Path.Combine(Paths.Configs, "CustomBreachScenarios");

        /// <summary>
        /// Gets a value indicating whether if MapEditorReborn is loaded or not.
        /// </summary>
        public static bool IsMapeditorLoaded { get; private set; } = false;

        /// <inheritdoc/>
        private Handler handler;

        /// <inheritdoc/>
        public override string Author => "Cegla";

        /// <inheritdoc/>
        public override string Name => "CustomBreachScenarios";

        /// <inheritdoc/>
        public override string Prefix => "CustomBreachScenarios";

        /// <inheritdoc/>
        public override PluginPriority Priority => PluginPriority.Medium;

        /// <inheritdoc/>
        public override Version Version => new Version(1, 0, 0);

        /// <inheritdoc/>
        public override Version RequiredExiledVersion => new Version(4, 2, 2);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Singleton = this;

            if (Loader.Plugins.Any(x => x.Name == "MapEditorReborn"))
            {
                IsMapeditorLoaded = true;
            }

            if (!Directory.Exists(CustomBreachScenariosPath))
            {
                Directory.CreateDirectory(CustomBreachScenariosPath);
            }

            if (Directory.GetFiles(CustomBreachScenariosPath).ToList().Count == 0)
            {
                BreachScenario example = new BreachScenario("example")
                {
                    Chance = 0,
                    Name = "example",
                    Cassies = new List<TimedCassieObject>
                    {
                        new TimedCassieObject
                        {
                            Delay = 20,
                            IsNoisy = true,
                            Announcement = "test",
                        },
                    },
                    OpenedDoors = new Dictionary<DoorType, int>
                    {
                        { DoorType.HeavyContainmentDoor, 50 },
                    },
                    ZoneColors = new Dictionary<ZoneType, ZoneColorObject>()
                    {
                        { ZoneType.HeavyContainment, new ZoneColorObject(1, 0, 0, 1) },
                    },
                    CustomConditions = new CustomConditionsObject(),
                    AutoNuke = new AutoNukeObject
                    {
                        Chance = 100,
                        Delay = 1800,
                    },
                    DelayedSCPSpawns = new List<DelayedSCPSpawnObject>
                    {
                        new DelayedSCPSpawnObject
                        {
                            Delay = 120,
                            Role = RoleType.Scp096,
                            Room = RoomType.Hcz096,
                        },
                    },
                    DoorLockdowns = new List<DoorLockdownObject>
                    {
                        new DoorLockdownObject
                        {
                            Chance = 50,
                            DoorLockType = DoorLockType.SpecialDoorFeature,
                            DoorType = DoorType.GateA,
                            Time = 120,
                        },
                    },
                    Blackouts = new List<BlackoutObject>
                    {
                        new BlackoutObject
                        {
                            Delay = 100,
                            Time = 100,
                            Zones = new List<ZoneType>
                            {
                                ZoneType.Entrance,
                                ZoneType.LightContainment,
                            },
                        },
                    },
                };

                string path = Path.Combine(CustomBreachScenariosPath, $"{example.Name}.yml");

                File.WriteAllText(path, Loader.Serializer.Serialize(example));

                Log.Info("Creating example scenario...");
            }

            handler = new Handler();

            Server.WaitingForPlayers += handler.OnWaitingForPlayer;
            Server.RoundStarted += handler.OnRoundStarted;
            Server.RespawningTeam += handler.OnRespawningTeam;
            Server.RoundEnded += handler.OnRoundEnded;

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= handler.OnWaitingForPlayer;
            Server.RoundStarted -= handler.OnRoundStarted;
            Server.RespawningTeam -= handler.OnRespawningTeam;
            Server.RoundEnded -= handler.OnRoundEnded;

            Singleton = null;
            handler = null;

            base.OnDisabled();
        }
    }
}
