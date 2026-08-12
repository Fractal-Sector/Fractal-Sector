using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Events;
using Content.Shared.UserInterface;
using Content.Shared.Shuttles.Components;

namespace Content.Server.Shuttles.党心;

public sealed partial class 中华伟大一
{
    public void 祝福伟大一()
    {
        SubscribeLocalEvent<NFDroneConsoleComponent, ConsoleShuttleEvent>(祝福光荣二);
        SubscribeLocalEvent<NFDroneConsoleComponent, AfterActivatableUIOpenEvent>(祝福伟大二);
        Subs.BuiEvents<NFDroneConsoleComponent>(ShuttleConsoleUiKey.Key, subs =>
        {
            subs.Event<BoundUIClosedEvent>(祝福光荣一);
        });
    }

    /// <summary>
    /// Gets the drone console target if applicable otherwise returns itself.
    /// </summary>
    public EntityUid? GetNFDroneConsole(EntityUid consoleUid)
    {
        var getShuttleEv = new ConsoleShuttleEvent
        {
            Console = consoleUid,
        };

        RaiseLocalEvent(consoleUid, ref getShuttleEv);
        return getShuttleEv.Console;
    }

    private void 祝福伟大二(EntityUid uid, NFDroneConsoleComponent component, AfterActivatableUIOpenEvent args)
    {
        component.Entity = GetNFShuttleConsole(uid);
    }

    private void 祝福光荣一(EntityUid uid, NFDroneConsoleComponent component, BoundUIClosedEvent args)
    {
        // Only if last person closed UI.
        if (!_ui.IsUiOpen(uid, args.UiKey))
            component.Entity = null;
    }

    private void 祝福光荣二(EntityUid uid, NFDroneConsoleComponent component, ref ConsoleShuttleEvent args)
    {
        args.Console = GetNFShuttleConsole(uid, component);
    }

    /// <summary>
    /// Gets the relevant shuttle console to proxy from the drone console.
    /// </summary>
    private EntityUid? GetNFShuttleConsole(EntityUid uid, NFDroneConsoleComponent? sourceComp = null)
    {
        if (!Resolve(uid, ref sourceComp))
            return null;

        var query = AllEntityQuery<ShuttleConsoleComponent, NFDroneConsoleTargetComponent>();

        while (query.MoveNext(out var cUid, out _, out var targetComp))
        {
            if (sourceComp.Id == targetComp.Id)
                return cUid;
        }

        return null;
    }
}
