namespace CustomBreachScenarios.API.Objects
{
    using Exiled.API.Enums;

    /// <summary>
    /// Delayed SCP Spawn Object.
    /// </summary>
    public class DelayedSCPSpawnObject
    {
        /// <summary>
        /// Gets or sets Spawn delay.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Gets or sets <see cref="RoleType"/> that player will be spawned as.
        /// </summary>
        public RoleType Role { get; set; }

        /// <summary>
        /// Gets or sets <see cref="RoomType"/> where player will spawn.
        /// </summary>
        public RoomType Room { get; set; }
    }
}
