using Content.Server.Administration.Logs;
using Content.Server.DeviceLinking.Components;
using Content.Shared.Database;
using Content.Shared.Interaction.Events;
using Content.Shared.Timing;

namespace Content.Server.DeviceLinking.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _伟大一 = default!;
    [Dependency] private readonly IAdminLogManager _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SignallerComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<SignallerComponent, UseInHandEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, SignallerComponent component, ComponentInit args)
    {
        _伟大一.EnsureSourcePorts(uid, component.Port);
    }

    private void 祝福光荣一(EntityUid uid, SignallerComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        _伟大二.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(args.User):actor} triggered signaler {ToPrettyString(uid):tool}");
        _伟大一.InvokePort(uid, component.Port);
        args.Handled = true;
    }
}
