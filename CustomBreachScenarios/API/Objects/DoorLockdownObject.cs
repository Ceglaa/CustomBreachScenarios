namespace CustomBreachScenarios.API.Objects
{
    using Exiled.API.Enums;

    /// <summary>
    /// Door Lockdown Object.
    /// </summary>
    public class DoorLockdownObject
    {
        /// <summary>
        /// Gets or sets Lockdown time.
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Gets or sets Lockdown chance.
        /// </summary>
        public int Chance { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Exiled.API.Enums.DoorType"/> to be Locked down.
        /// </summary>
        public DoorType DoorType { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Exiled.API.Enums.DoorLockType"/> that will be used on door.
        /// </summary>
        public DoorLockType DoorLockType { get; set; }
    }
}
