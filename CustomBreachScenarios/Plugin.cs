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
    using Exiled.API.Interfaces;
    using Exiled.Loader;
    using UnityEngine;
    using Player = Exiled.Events.Handlers.Player;
    using Server = Exiled.Events.Handlers.Server;
    using CustomBreachScenarios.EventHandlers;

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

        /// <inheritdoc cref="Handler"/>
        private Handler _handler;

        /// <inheritdoc/>
        public override string Author => "Cegla";

        /// <inheritdoc/>
        public override string Name => "CustomBreachScenarios";

        /// <inheritdoc/>
        public override string Prefix => "CustomBreachScenarios";

        /// <inheritdoc/>
        public override PluginPriority Priority => PluginPriority.Medium;

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(1, 0, 1);

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Singleton = this;

            if (!Directory.Exists(CustomBreachScenariosPath))
            {
                Directory.CreateDirectory(CustomBreachScenariosPath);
            }

            CreateExampleScenario();

            _handler = new Handler();

            Server.WaitingForPlayers += _handler.OnWaitingForPlayer;
            Server.RoundStarted += _handler.OnRoundStarted;
            Server.RespawningTeam += _handler.OnRespawningTeam;
            Server.RoundEnded += _handler.OnRoundEnded;

            Player.ChangingRole += _handler.OnChangingRole;

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= _handler.OnWaitingForPlayer;
            Server.RoundStarted -= _handler.OnRoundStarted;
            Server.RespawningTeam -= _handler.OnRespawningTeam;
            Server.RoundEnded -= _handler.OnRoundEnded;

            Player.ChangingRole -= _handler.OnChangingRole;

            Singleton = null;
            _handler = null;

            base.OnDisabled();
        }

        private static void CreateExampleScenario()
        {
            if (Directory.GetFiles(CustomBreachScenariosPath).ToList().Count != 0)
                return;

            BreachScenario example = new("example")
            {
                Chance = 0,
                Name = "example",
                Cassies = new List<TimedCassieObject>
                {
                    new()
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
                ZoneColors = new List<ZoneColorObject>()
                {
                    new()
                    {
                        ZoneType = ZoneType.LightContainment,
                        Delay = 10,
                        Time = 70,
                        R = 1,
                        G = 0,
                        B = 0,
                        A = 0,
                    },
                },
                CustomConditions = new CustomConditionsObject(),
                AutoNuke = new AutoNukeObject
                {
                    Chance = 100,
                    Delay = 1800,
                },
                DelayedSCPSpawns = new List<DelayedSCPSpawnObject>
                {
                    new()
                    {
                        Delay = 120,
                        Role = RoleType.Scp096,
                        Room = RoomType.Hcz096,
                    },
                },
                DoorLockdowns = new List<DoorLockdownObject>
                {
                    new()
                    {
                        Chance = 50,
                        DoorLockType = DoorLockType.SpecialDoorFeature,
                        DoorType = DoorType.GateA,
                        Time = 120,
                    },
                },
                Blackouts = new List<BlackoutObject>
                {
                    new()
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
    }
}
