namespace CustomBreachScenarios.API.Objects;

/// <summary>
/// Timed Cassie Object.
/// </summary>
public class TimedCassieObject
{
    /// <summary>
    /// Gets or sets delay after Cassie will be executed.
    /// </summary>
    public int Delay { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Cassie is noisy or not.
    /// </summary>
    public bool IsNoisy { get; set; } = true;

    /// <summary>
    /// Gets or sets Cassie announcement content.
    /// </summary>
    public string Announcement { get; set; }
}