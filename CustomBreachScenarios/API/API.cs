namespace CustomBreachScenarios.API
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CustomBreachScenarios.API.Objects;
    using Exiled.API.Features;
    using Exiled.Loader;
    using MapEditorReborn.API.Features;
    using MEC;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Method class for CustomBreachScenario.
    /// </summary>
    public static class API
    {
        /// <summary>
        /// Gets or sets List of all current <see cref="DelaySpawnSCP"/> coroutines.
        /// </summary>
        public static List<CoroutineHandle> DelayedSCPSpawnCoroutines { get; set; } = new List<CoroutineHandle>();

        /// <summary>
        /// Gets currently loaded Scenarios.
        /// </summary>
        public static List<BreachScenario> LoadedScenarios { get; internal set; } = new List<BreachScenario>();

        /// <summary>
        /// Gets or sets current <see cref="BreachScenario"/>.
        /// </summary>
        public static BreachScenario CurrentScenario { get; set; }

        /// <summary>
        /// Draws Breach Scenario from <see cref="LoadedScenarios"/> list.
        /// </summary>
        /// <returns><see cref="BreachScenario"/>.</returns>
        public static BreachScenario DrawScenario()
        {
            return LoadedScenarios.FirstOrDefault(x => x.Chance >= Random.Range(1, 101));
        }

        /// <summary>
        /// Deserializes all scenarios and adds them to <see cref="LoadedScenarios"/> list.
        /// </summary>
        public static void GetAllScenarios()
        {
            foreach (string path in Directory.GetFiles(Plugin.CustomBreachScenariosPath))
            {
                LoadedScenarios.Add(Loader.Deserializer.Deserialize<BreachScenario>(File.ReadAllText(path)));
            }
        }

        /// <summary>
        /// Plays <see cref="CurrentScenario"/>.
        /// </summary>
        public static void PlayScenario()
        {
            if (CurrentScenario == null)
            {
                return;
            }

            if (CurrentScenario.AutoNuke.Chance >= Random.Range(1, 101))
            {
                Timing.CallDelayed(CurrentScenario.AutoNuke.Delay, () => Warhead.Start());
            }

            foreach (TimedCassieObject timedCassieObject in CurrentScenario.Cassies)
            {
                Cassie.DelayedMessage(timedCassieObject.Announcement, timedCassieObject.Delay, false, timedCassieObject.IsNoisy);
            }

            if (Plugin.IsMapeditorLoaded)
            {
                if (!string.IsNullOrEmpty(CurrentScenario.MapName))
                {
                    MapUtils.LoadMap(MapUtils.GetMapByName(CurrentScenario.MapName));
                }
            }

            foreach (Door door in Map.Doors)
            {
                if (CurrentScenario.OpenedDoors.TryGetValue(door.Type, out int chance))
                {
                    if (chance > Random.Range(1, 101))
                    {
                        door.IsOpen = true;
                    }
                }
            }

            foreach (Room room in Map.Rooms)
            {
                if (CurrentScenario.ZoneColors.TryGetValue(room.Zone, out ZoneColorObject color))
                {
                    room.ResetColor();
                    Color newcolor = new Color(color.R, color.G, color.B, color.A);
                    room.Color = newcolor;
                }
            }

            foreach (DelayedSCPSpawnObject spawnObject in CurrentScenario.DelayedSCPSpawns)
            {
                Timing.CallDelayed(spawnObject.Delay, () =>
                {
                    DelayedSCPSpawnCoroutines.Add(Timing.CallDelayed(spawnObject.Delay, () => Timing.RunCoroutine(DelaySpawnSCP(spawnObject))));
                });
            }

            foreach (DoorLockdownObject doorLockdownObject in CurrentScenario.DoorLockdowns)
            {
                ProcessTimedLockdown(doorLockdownObject);
            }

            foreach (BlackoutObject blackoutObject in CurrentScenario.Blackouts)
            {
                if (blackoutObject.Chance >= Random.Range(1, 101))
                {
                    Timing.CallDelayed(blackoutObject.Delay, () => Map.TurnOffAllLights(blackoutObject.Time, blackoutObject.Zones));
                }
            }
        }

        /// <summary>
        /// Tries to spawn repeatedly one of spectators as SCP after delay.
        /// </summary>
        /// <param name="spawnObject">DelayedSCPSpawnObject.</param>
        /// <returns>Float.</returns>
        public static IEnumerator<float> DelaySpawnSCP(DelayedSCPSpawnObject spawnObject)
        {
            while (Round.IsStarted)
            {
                List<Player> spectators = Player.Get(RoleType.Spectator).ToList();
                if (spectators.Count > 0)
                {
                    spectators[Random.Range(0, spectators.Count)].SetRole(spawnObject.Role);
                    yield break;
                }

                yield return Timing.WaitForSeconds(Plugin.Singleton.Config.DelayedSpawnInterval);
            }
        }

        /// <summary>
        /// Processes timed lockdown of door.
        /// </summary>
        /// <param name="doorLockdownObject">DoorLockdownObject.</param>
        public static void ProcessTimedLockdown(DoorLockdownObject doorLockdownObject)
        {
            List<Door> doors = Map.Doors.Where(x => x.Type == doorLockdownObject.DoorType).ToList();

            foreach (Door door in doors)
            {
                if (door.Type == doorLockdownObject.DoorType)
                {
                    if (doorLockdownObject.Chance >= Random.Range(1, 101))
                    {
                        door.ChangeLock(doorLockdownObject.DoorLockType);
                        if (doorLockdownObject.Time > 0)
                        {
                            Timing.CallDelayed(doorLockdownObject.Time, () => door.Unlock());
                        }
                    }
                }
            }
        }
    }
}
