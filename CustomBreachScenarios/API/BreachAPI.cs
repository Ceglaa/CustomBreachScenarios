namespace CustomBreachScenarios.API;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using CustomBreachScenarios.API.Objects;

using PlayerRoles;
using Exiled.API.Features;
using Exiled.Loader;

using MEC;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Method class for CustomBreachScenario.
/// </summary>
public static class BreachAPI
{
    /// <summary>
    /// Gets or sets List of all current <see cref="DelaySpawnScp"/> coroutines.
    /// </summary>
    public static List<CoroutineHandle> DelayedScpSpawnCoroutines { get; set; } = new List<CoroutineHandle>();

    /// <summary>
    /// Draws Breach Scenario from <paramref name="inputList"/> list.
    /// </summary>
    /// <param name="inputList">List from all scenarios are outputed.</param>
    /// <returns>Selected <see cref="BreachScenario"/> or null if not found.</returns>
    public static BreachScenario DrawScenario(IEnumerable<BreachScenario> inputList)
    {
        int randomValue = Random.Range(1, 101);
        return inputList.OrderBy(x => x.Chance).FirstOrDefault(x => x.Chance >= randomValue);
    }

    /// <summary>
    /// Deserializes all scenarios from <paramref name="directoryPath"/>.
    /// </summary>
    /// <param name="directoryPath">Directory path from all scenarios are readed.</param>
    /// <returns><see cref="IEnumerable{BreachScenario}"/>.</returns>
    public static IEnumerable<BreachScenario> GetAllScenarios(string directoryPath)
    {
        return Directory.EnumerateFiles(directoryPath)
            .Select(file => Loader.Deserializer.Deserialize<BreachScenario>(File.ReadAllText(file)))
            .ToList();
    }

    /// <summary>
    /// Plays scneario.
    /// </summary>
    /// <param name="scenario">Scenario to be played.</param>
    public static void PlayScenario(BreachScenario scenario)
    {
        if (scenario is null)
        {
            return;
        }

        if (scenario.AutoNuke.Chance >= Random.Range(1, 101))
        {
            Timing.CallDelayed(scenario.AutoNuke.Delay, Warhead.Start);
        }

        foreach (TimedCassieObject timedCassieObject in scenario.Cassies)
        {
            Cassie.DelayedMessage(timedCassieObject.Announcement, timedCassieObject.Delay, false, timedCassieObject.IsNoisy);
        }

        foreach (Door door in Door.List)
        {
            if (scenario.OpenedDoors.TryGetValue(door.Type, out int chance))
            {
                if (chance > Random.Range(1, 101))
                {
                    door.IsOpen = true;
                }
            }
        }

        foreach (ZoneColorObject zoneColor in scenario.ZoneColors)
        {
            foreach (Room room in Room.List)
            {
                if (zoneColor.ZoneType == room.Zone)
                {
                    Timing.CallDelayed(zoneColor.Delay, () => ChangeRoomColorInternal(room, zoneColor));
                }
            }
        }

        foreach (DelayedScpSpawnObject spawnObject in scenario.DelayedScpSpawns)
        {
            Timing.CallDelayed(spawnObject.Delay, () =>
            {
                DelayedScpSpawnCoroutines.Add(Timing.CallDelayed(spawnObject.Delay, () => Timing.RunCoroutine(DelaySpawnScp(spawnObject))));
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

        foreach (string command in scenario.Commands)
        {
            GameCore.Console.singleton.TypeCommand(command, Server.Host.Sender);
        }
    }

    /// <summary>
    /// Tries to spawn repeatedly one of spectators as provided SCP after delay.
    /// </summary>
    /// <param name="spawnObject">DelayedSCPSpawnObject.</param>
    /// <returns>Float.</returns>
    public static IEnumerator<float> DelaySpawnScp(DelayedScpSpawnObject spawnObject)
    {
        while (Round.IsStarted)
        {
            yield return Timing.WaitUntilTrue(() => Player.List.Any(x => x.Role == RoleTypeId.Spectator));
            List<Player> spectators = Player.Get(RoleTypeId.Spectator).ToList();
            if (spectators.Count > 0)
            {
                spectators[Random.Range(0, spectators.Count)].Role.Set(spawnObject.Role);
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
        List<Door> doors = Door.List.Where(x => x.Type == doorLockdownObject.DoorType).ToList();

        foreach (Door door in doors)
        {
            if (doorLockdownObject.Chance < Random.Range(1, 101))
                continue;

            door.ChangeLock(doorLockdownObject.DoorLockType);
            if (doorLockdownObject.Time > 0)
            {
                Timing.CallDelayed(doorLockdownObject.Time, () => door.Unlock());
            }
        }
    }

    /// <summary>
    /// Changes color of the room after set delay.
    /// </summary>
    /// <param name="room">Room in which the color will be changed.</param>
    /// <param name="zoneColorObject">Information about color and delay after the room's color will be changed.</param>
    public static void ChangeRoomColorInternal(Room room, ZoneColorObject zoneColorObject)
    {
        room.ResetColor();
        Color newcolor = new Color(zoneColorObject.R, zoneColorObject.G, zoneColorObject.B, zoneColorObject.A);
        room.Color = newcolor;

        if (zoneColorObject.Time > 0)
        {
            Timing.CallDelayed(zoneColorObject.Time, () => room.ResetColor());
        }
    }
}