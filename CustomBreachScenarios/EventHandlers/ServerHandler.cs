namespace CustomBreachScenarios
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomBreachScenarios.API;
    using Exiled.Events.EventArgs;
    using MEC;

    /// <summary>
    /// Handles Exiled events.
    /// </summary>
    internal sealed partial class Handler
    {
        /// <summary>
        /// Current loaded scenario.
        /// </summary>
        public static BreachScenario SelectedScenario;

        /// <summary>
        /// Gets or sets currently loaded Scenarios.
        /// </summary>
        public static List<BreachScenario> LoadedScenarios { get; internal set; } = new List<BreachScenario>();

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnWaitingForPlayers"/>
        public void OnWaitingForPlayer()
        {
            foreach (CoroutineHandle coroutines in BreachAPI.DelayedSCPSpawnCoroutines)
            {
                Timing.KillCoroutines(coroutines);
            }

            BreachAPI.DelayedSCPSpawnCoroutines.Clear();
            LoadedScenarios.Clear();

            LoadedScenarios = BreachAPI.GetAllScenarios(Plugin.CustomBreachScenariosPath).ToList();
            SelectedScenario = BreachAPI.DrawScenario(LoadedScenarios);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted"/>
        public void OnRoundStarted()
        {
            BreachAPI.PlayScenario(SelectedScenario);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRespawningTeam"/>
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
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
                if (SelectedScenario.CustomConditions.CanChiSpawn)
                {
                    ev.NextKnownTeam = Respawning.SpawnableTeamType.NineTailedFox;
                    return;
                }

                ev.IsAllowed = false;
            }
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.RoundEnded"/>
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (CoroutineHandle coroutines in BreachAPI.DelayedSCPSpawnCoroutines)
            {
                Timing.KillCoroutines(coroutines);
            }

            BreachAPI.DelayedSCPSpawnCoroutines.Clear();
            LoadedScenarios.Clear();
        }
    }
}
