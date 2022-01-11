namespace CustomBreachScenarios
{
    using Exiled.Events.EventArgs;
    using static API.API;

    /// <summary>
    /// Handles Exiled events.
    /// </summary>
    internal sealed partial class Handler
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnWaitingForPlayers"/>
        public void OnWaitingForPlayer()
        {
            LoadedScenarios.Clear();
            GetAllScenarios();
            CurrentScenario = DrawScenario();
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted"/>
        public void OnRoundStarted()
        {
            PlayScenario();
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRespawningTeam"/>
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (!CurrentScenario.CustomConditions.CanNtfSpawn && ev.NextKnownTeam == Respawning.SpawnableTeamType.NineTailedFox)
            {
                if (CurrentScenario.CustomConditions.CanChiSpawn)
                {
                    ev.NextKnownTeam = Respawning.SpawnableTeamType.ChaosInsurgency;
                    return;
                }

                ev.IsAllowed = false;
            }
            else if (!CurrentScenario.CustomConditions.CanChiSpawn && ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
            {
                if (CurrentScenario.CustomConditions.CanChiSpawn)
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
            LoadedScenarios.Clear();
        }
    }
}
