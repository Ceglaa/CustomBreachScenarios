namespace CustomBreachScenarios;

using System.ComponentModel;
using Exiled.API.Interfaces;

/// <summary>
/// The plugin's config.
/// </summary>
public sealed class Config : IConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether plugin is enabled or not.
    /// </summary>
    [Description("Is the plugin enabled.")]
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the plugin's debug feature is enabled or not.
    /// </summary>
    [Description("Is debug enabled")]
    public bool Debug { get; set; } = false;

    /// <summary>
    /// Gets a value of time between trying to delay spawn spectator.
    /// </summary>
    [Description("Delay between trying to spawn spectator as SCP if there's not any")]
    public int DelayedSpawnInterval { get; private set; } = 6;
}