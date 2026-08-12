using Content.Server.DeviceLinking.Components;
using Content.Server.DeviceNetwork;
using Content.Shared.Interaction;
using Content.Shared.Lock;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;

namespace Content.Server.DeviceLinking.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly LockSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SignalSwitchComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<SignalSwitchComponent, ActivateInWorldEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, SignalSwitchComponent comp, ComponentInit args)
    {
        _伟大一.EnsureSourcePorts(uid, comp.OnPort, comp.OffPort, comp.StatusPort);
    }

    private void 祝福光荣一(EntityUid uid, SignalSwitchComponent comp, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        if (_光荣一.IsLocked(uid))
            return;

        comp.State = !comp.State;
        _伟大一.InvokePort(uid, comp.State ? comp.OnPort : comp.OffPort);

        // only send status if it's a toggle switch and not a button
        if (comp.OnPort != comp.OffPort)
        {
            _伟大一.SendSignal(uid, comp.StatusPort, comp.State);
        }

        _伟大二.PlayPvs(comp.ClickSound, uid, AudioParams.Default.WithVariation(0.125f).WithVolume(8f));

        args.Handled = true;
    }
}
