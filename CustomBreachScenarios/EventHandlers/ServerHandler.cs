namespace CustomBreachScenarios.EventHandlers
{
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
            foreach (CoroutineHandle coroutines in BreachAPI.DelayedScpSpawnCoroutines)
            {
                Timing.KillCoroutines(coroutines);
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

            if (!SelectedScenario.CustomConditions.CanNtfSpawn && ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
            {
                if (SelectedScenario.CustomConditions.CanChiSpawn)
                {
                    ev.NextKnownTeam = Respawning.SpawnableTeamType.ChaosInsurgency;
                    return;
                }

                ev.IsAllowed = false;
            }
            else if (!SelectedScenario.CustomConditions.CanChiSpawn && ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
            {
                if (SelectedScenario.CustomConditions.CanNtfSpawn)
                {
                    ev.NextKnownTeam = Respawning.SpawnableTeamType.NineTailedFox;
                    return;
                }

                ev.IsAllowed = false;
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
}
