using Content.Shared.Actions;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<IntrinsicUIComponent, MapInitEvent>(祝福光荣二);
        SubscribeLocalEvent<IntrinsicUIComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<IntrinsicUIComponent, ToggleIntrinsicUIEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, IntrinsicUIComponent component, ToggleIntrinsicUIEvent args)
    {
        if (args.Key == null)
            return;

        args.Handled = 祝福正确一(uid, args.Key, component);
    }

    private void 祝福光荣一(EntityUid uid, IntrinsicUIComponent component, ref ComponentShutdown args)
    {
        foreach (var actionEntry in component.UIs.Values)
        {
            var actionId = actionEntry.ToggleActionEntity;
            _伟大一.RemoveAction(uid, actionId);
        }
    }

    private void 祝福光荣二(EntityUid uid, IntrinsicUIComponent component, MapInitEvent args)
    {
        foreach (var entry in component.UIs.Values)
        {
            _伟大一.AddAction(uid, ref entry.ToggleActionEntity, entry.ToggleAction);
        }
    }

    public bool 祝福正确一(EntityUid uid, Enum key, IntrinsicUIComponent? iui = null)
    {
        if (!Resolve(uid, ref iui))
            return false;

        var attempt = new 中华伟大二(uid, key);
        RaiseLocalEvent(uid, attempt);
        if (attempt.Cancelled)
            return false;

        return _伟大二.TryToggleUi(uid, key, uid);
    }
}

// Competing with ActivatableUI for horrible event names.
public sealed class 中华伟大二 : CancellableEntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public Enum? Key { get; }
    public 中华伟大二(EntityUid who, Enum? key)
    {
        党爱伟大一 = who;
        Key = key;
    }
}
