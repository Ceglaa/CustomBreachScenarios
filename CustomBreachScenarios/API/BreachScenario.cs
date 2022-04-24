namespace CustomBreachScenarios.API
{
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
        /// Gets MapEditorReborn map name.
        /// </summary>
        [Description("Map name for MapEditorReborn plugin")]
        public string MapName { get; private set; }

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
        /// Gets list of <see cref="DelayedSCPSpawnObject"/>.
        /// </summary>
        public List<DelayedSCPSpawnObject> DelayedSCPSpawns { get; internal set; } = new List<DelayedSCPSpawnObject>();

        /// <summary>
        /// Gets list of <see cref="DoorLockdownObjects"/>.
        /// </summary>
        public List<DoorLockdownObject> DoorLockdowns { get; internal set; } = new List<DoorLockdownObject>();

        /// <summary>
        /// Gets list of <see cref="BlackoutObject"/>.
        /// </summary>
        public List<BlackoutObject> Blackouts { get; internal set; } = new List<BlackoutObject>();

        /// <summary>
        /// Gets Dictionary of <see cref="ZoneType"/> and <see cref="SerializableColor"/>.
        /// </summary>
        public List<ZoneColorObject> ZoneColors { get; internal set; } = new List<ZoneColorObject>();

        /// <summary>
        /// Gets Dictionary of <see cref="DoorType"/> and <see cref="int"/>.
        /// </summary>
        public Dictionary<DoorType, int> OpenedDoors { get; internal set; } = new Dictionary<DoorType, int>();
    }
}
