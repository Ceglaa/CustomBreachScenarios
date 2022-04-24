namespace CustomBreachScenarios.EventHandlers
{
    using System.Linq;
    using Exiled.Events.EventArgs;

    /// <summary>
    /// Handles Exiled events.
    /// </summary>
    public sealed partial class Handler
    {
        /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnChangingRole(ChangingRoleEventArgs)"/>
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player is null)
                return;

            if (SelectedScenario is null)
                return;

            if (SelectedScenario.DelayedSCPSpawns.Any(x => x.Role == ev.NewRole))
            {
                ev.NewRole = RoleType.ClassD;
            }
        }
    }
}
