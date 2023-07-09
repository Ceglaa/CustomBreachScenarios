namespace CustomBreachScenarios.API;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using CustomBreachScenarios.API.Objects;
using Exiled.API.Enums;

/// <summary>
/// Breach Scenario Object.
/// </summary>
[Serializable]
public class BreachScenario
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BreachScenario"/> class.
    /// </summary>
    public BreachScenario()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BreachScenario"/> class.
    /// </summary>
    /// <param name="name">Scenario name.</param>
    public BreachScenario(string name) => Name = name;

    /// <summary>
    /// Gets Breach Scenario name.
    /// </summary>
    public string Name { get; internal set; } = "Default";

    /// <summary>
    /// Gets Breach Scenario chance.
    /// </summary>
    public int Chance { get; internal set; }

    /// <summary>
    /// Gets commands to execute.
    /// </summary>
    public List<string> Commands { get; internal set; } = new List<string>();

    /// <summary>
    /// Gets the <see cref="AutoNukeObject"/>.
    /// </summary>
    public AutoNukeObject AutoNuke { get; internal set; } = new AutoNukeObject();

    /// <summary>
    /// Gets the <see cref="CustomConditionsObject"/>.
    /// </summary>
    public CustomConditionsObject CustomConditions { get; internal set; } = new CustomConditionsObject();

    /// <summary>
    /// Gets list of <see cref="TimedCassieObject"/>.
    /// </summary>
    public List<TimedCassieObject> Cassies { get; internal set; } = new List<TimedCassieObject>();

    /// <summary>
    /// Gets list of <see cref="DelayedScpSpawnObject"/>.
    /// </summary>
    public List<DelayedScpSpawnObject> DelayedScpSpawns { get; internal set; } = new List<DelayedScpSpawnObject>();

    /// <summary>
    /// Gets list of <see cref="DoorLockdownObject"/>.
    /// </summary>
    public List<DoorLockdownObject> DoorLockdowns { get; internal set; } = new List<DoorLockdownObject>();

    /// <summary>
    /// Gets list of <see cref="BlackoutObject"/>.
    /// </summary>
    public List<BlackoutObject> Blackouts { get; internal set; } = new List<BlackoutObject>();

    /// <summary>
    /// Gets list of <see cref="ZoneColorObject"/>.
    /// </summary>
    public List<ZoneColorObject> ZoneColors { get; internal set; } = new List<ZoneColorObject>();

    /// <summary>
    /// Gets Dictionary of <see cref="DoorType"/> and <see cref="int"/>.
    /// </summary>
    public Dictionary<DoorType, int> OpenedDoors { get; internal set; } = new Dictionary<DoorType, int>();
}