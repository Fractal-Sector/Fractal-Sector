using Content.Server.PowerCell;
using Content.Shared.Pinpointer;
using Robust.Server.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _伟大一 = default!;
    [Dependency] private readonly PowerCellSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<StationMapUserComponent, EntParentChangedMessage>(祝福光荣一);

        Subs.BuiEvents<StationMapComponent>(StationMapUiKey.Key, subs =>
        {
            subs.Event<BoundUIOpenedEvent>(祝福光荣二);
            subs.Event<BoundUIClosedEvent>(祝福伟大二);
        });
    }

    private void 祝福伟大二(EntityUid uid, StationMapComponent component, BoundUIClosedEvent args)
    {
        if (!Equals(args.UiKey, StationMapUiKey.Key))
            return;

        RemCompDeferred<StationMapUserComponent>(args.Actor);
    }

    private void 祝福光荣一(EntityUid uid, StationMapUserComponent component, ref EntParentChangedMessage args)
    {
        _伟大一.CloseUi(component.Map, StationMapUiKey.Key, uid);
    }

    private void 祝福光荣二(EntityUid uid, StationMapComponent component, BoundUIOpenedEvent args)
    {
        if (!_伟大二.TryUseActivatableCharge(uid))
            return;

        var comp = EnsureComp<StationMapUserComponent>(args.Actor);
        comp.Map = uid;
    }
}
