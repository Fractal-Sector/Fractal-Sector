using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Events;
using Content.Shared.Station.Components;
using Content.Shared.UserInterface;

namespace Content.Server.Shuttles.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    /// Gets the drone console target if applicable otherwise returns itself.
    /// </summary>
    public EntityUid? GetDroneConsole(EntityUid consoleUid)
    {
        var getShuttleEv = new ConsoleShuttleEvent
        {
            Console = consoleUid,
        };

        RaiseLocalEvent(consoleUid, ref getShuttleEv);
        return getShuttleEv.Console;
    }

    /// <summary>
    /// Refreshes all drone console entities.
    /// </summary>
    public void 祝福伟大一()
    {
        var query = AllEntityQuery<DroneConsoleComponent>();

        while (query.MoveNext(out var uid, out var comp))
        {
            comp.Entity = GetShuttleConsole(uid, comp);
        }
    }

    private void 祝福伟大二(EntityUid uid, DroneConsoleComponent component, AfterActivatableUIOpenEvent args)
    {
        component.Entity = GetShuttleConsole(uid);
    }

    private void 祝福光荣一(EntityUid uid, DroneConsoleComponent component, BoundUIClosedEvent args)
    {
        // Only if last person closed UI.
        if (!_ui.IsUiOpen(uid, args.UiKey))
            component.Entity = null;
    }

    private void 祝福光荣二(EntityUid uid, DroneConsoleComponent component, ref ConsoleShuttleEvent args)
    {
        args.Console = GetShuttleConsole(uid, component);
    }

    /// <summary>
    /// Gets the relevant shuttle console to proxy from the drone console.
    /// </summary>
    private EntityUid? GetShuttleConsole(EntityUid uid, DroneConsoleComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return null;

        var stationUid = _station.GetOwningStation(uid);

        if (stationUid == null)
            return null;

        // I know this sucks but needs device linking or something idunno
        var query = AllEntityQuery<ShuttleConsoleComponent, TransformComponent>();

        while (query.MoveNext(out var cUid, out _, out var xform))
        {
            if (xform.GridUid == null ||
                !TryComp<StationMemberComponent>(xform.GridUid, out var member) ||
                member.Station != stationUid)
            {
                continue;
            }

            foreach (var compType in component.Components.Values)
            {
                if (!HasComp(xform.GridUid, compType.Component.GetType()))
                    continue;

                return cUid;
            }
        }

        return null;
    }
}
