namespace CustomBreachScenarios.EventHandlers;

using Exiled.Events.EventArgs.Server;
using System.Collections.Generic;
using System.Linq;
using CustomBreachScenarios.API;
using Exiled.Events.EventArgs;
using MEC;

/// <summary>
/// Handles Exiled events.
/// </summary>
public sealed partial class Handler
{
    /// <summary>
    /// Gets or sets current loaded scenario.
    /// </summary>
    public static BreachScenario SelectedScenario { get; set; }

    /// <summary>
    /// Gets currently loaded Scenarios.
    /// </summary>
    public static List<BreachScenario> LoadedScenarios { get; internal set; } = new();

    /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnWaitingForPlayers()"/>
    public void OnWaitingForPlayer()
    {
        foreach (CoroutineHandle coroutine in BreachAPI.DelayedScpSpawnCoroutines)
        {
            Timing.KillCoroutines(coroutine);
        }

        BreachAPI.DelayedScpSpawnCoroutines.Clear();
        LoadedScenarios.Clear();

        LoadedScenarios = BreachAPI.GetAllScenarios(Plugin.CustomBreachScenariosPath).ToList();
        SelectedScenario = BreachAPI.DrawScenario(LoadedScenarios);
    }

    /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted()"/>
    public void OnRoundStarted() => BreachAPI.PlayScenario(SelectedScenario);

    /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRespawningTeam(RespawningTeamEventArgs)"/>
    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (SelectedScenario is null)
        {
            return;
        }

        switch (ev.NextKnownTeam)
        {
            case Respawning.SpawnableTeamType.NineTailedFox when !SelectedScenario.CustomConditions.CanNtfSpawn:
                ev.NextKnownTeam = SelectedScenario.CustomConditions.CanChiSpawn ? Respawning.SpawnableTeamType.ChaosInsurgency : ev.NextKnownTeam;
                ev.IsAllowed = SelectedScenario.CustomConditions.CanChiSpawn;
                break;
            case Respawning.SpawnableTeamType.ChaosInsurgency when !SelectedScenario.CustomConditions.CanChiSpawn:
                ev.NextKnownTeam = SelectedScenario.CustomConditions.CanNtfSpawn ? Respawning.SpawnableTeamType.NineTailedFox : ev.NextKnownTeam;
                ev.IsAllowed = SelectedScenario.CustomConditions.CanNtfSpawn;
                break;
        }
    }

    /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundEnded(RoundEndedEventArgs)"/>
    public void OnRoundEnded(RoundEndedEventArgs ev)
    {
        foreach (CoroutineHandle coroutines in BreachAPI.DelayedScpSpawnCoroutines)
        {
            Timing.KillCoroutines(coroutines);
        }

        BreachAPI.DelayedScpSpawnCoroutines.Clear();
        LoadedScenarios.Clear();
    }
}