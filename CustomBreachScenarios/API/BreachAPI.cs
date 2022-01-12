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
    public static class BreachAPI
    {
        /// <summary>
        /// Gets or sets List of all current <see cref="DelaySpawnSCP"/> coroutines.
        /// </summary>
        public static List<CoroutineHandle> DelayedSCPSpawnCoroutines { get; set; } = new List<CoroutineHandle>();

        /// <summary>
        /// Draws Breach Scenario from <paramref name="inputList"/> list.
        /// </summary>
        /// <param name="inputList">List from all scenarios are outputed.</param>
        /// <returns><see cref="BreachScenario"/>.</returns>
        public static BreachScenario DrawScenario(List<BreachScenario> inputList)
        {
            return inputList.FirstOrDefault(x => x.Chance >= Random.Range(1, 101));
        }

        /// <summary>
        /// Deserializes all scenarios from <paramref name="directoryPath"/>.
        /// </summary>
        /// <param name="directoryPath">Directory path from all scenarios are readed.</param>
        /// <returns><see cref="IEnumerable{BreachScenario}"/>.</returns>
        public static IEnumerable<BreachScenario> GetAllScenarios(string directoryPath)
        {
            List<BreachScenario> scenarios = new List<BreachScenario>();
            foreach (string file in Directory.GetFiles(directoryPath))
            {
                scenarios.Add(Loader.Deserializer.Deserialize<BreachScenario>(File.ReadAllText(file)));
            }

            return scenarios;
        }

        /// <summary>
        /// Plays scneario.
        /// </summary>
        /// <param name="scenario">Scenario to be played.</param>
        public static void PlayScenario(BreachScenario scenario)
        {
            if (scenario == null)
            {
                return;
            }

            if (scenario.AutoNuke.Chance >= Random.Range(1, 101))
            {
                Timing.CallDelayed(scenario.AutoNuke.Delay, () => Warhead.Start());
            }

            foreach (TimedCassieObject timedCassieObject in scenario.Cassies)
            {
                Cassie.DelayedMessage(timedCassieObject.Announcement, timedCassieObject.Delay, false, timedCassieObject.IsNoisy);
            }

            if (Plugin.IsMapeditorLoaded)
            {
                if (!string.IsNullOrEmpty(scenario.MapName))
                {
                    MapUtils.LoadMap(MapUtils.GetMapByName(scenario.MapName));
                }
            }

            foreach (Door door in Map.Doors)
            {
                if (scenario.OpenedDoors.TryGetValue(door.Type, out int chance))
                {
                    if (chance > Random.Range(1, 101))
                    {
                        door.IsOpen = true;
                    }
                }
            }

            foreach (Room room in Map.Rooms)
            {
                if (scenario.ZoneColors.TryGetValue(room.Zone, out ZoneColorObject color))
                {
                    room.ResetColor();
                    Color newcolor = new Color(color.R, color.G, color.B, color.A);
                    room.Color = newcolor;
                }
            }

            foreach (DelayedSCPSpawnObject spawnObject in scenario.DelayedSCPSpawns)
            {
                Timing.CallDelayed(spawnObject.Delay, () =>
                {
                    DelayedSCPSpawnCoroutines.Add(Timing.CallDelayed(spawnObject.Delay, () => Timing.RunCoroutine(DelaySpawnSCP(spawnObject))));
                });
            }

            foreach (DoorLockdownObject doorLockdownObject in scenario.DoorLockdowns)
            {
                ProcessTimedLockdown(doorLockdownObject);
            }

            foreach (BlackoutObject blackoutObject in scenario.Blackouts)
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
