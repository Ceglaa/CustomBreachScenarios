namespace CustomBreachScenarios.API.Objects;

using System.Collections.Generic;
using Exiled.API.Enums;

/// <summary>
/// Blackout Object.
/// </summary>
public class BlackoutObject
{
    /// <summary>
    /// Gets or sets Blackout delay.
    /// </summary>
    public int Delay { get; set; }

    /// <summary>
    /// Gets or sets Blackout time.
    /// </summary>
    public int Time { get; set; }

    /// <summary>
    /// Gets or sets Blackout chance.
    /// </summary>
    public int Chance { get; set; }

    /// <summary>
    /// Gets or sets List of <see cref="ZoneType">Zones</see> affected by Blackout.
    /// </summary>
    public List<ZoneType> Zones { get; set; } = new List<ZoneType>();
}