namespace CustomBreachScenarios.EventHandlers;

using System.Linq;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

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

        if (SelectedScenario.DelayedScpSpawns.Any(x => x.Role == ev.NewRole))
        {
            ev.NewRole = RoleTypeId.ClassD;
        }
    }

    /// <inheritdoc cref="Exiled.Events.Handlers.Player.OnTriggeringTesla(TriggeringTeslaEventArgs)"/>
    public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
    {
        if (SelectedScenario.CustomConditions.TeslasDisabled)
        {
            ev.IsAllowed = false;
            ev.IsTriggerable = false;
        }
    }
}