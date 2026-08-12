using Content.Server.Atmos.Piping.Binary.Components;
using Content.Server.DeviceLinking.Systems;
using Content.Shared.Atmos.Piping.Binary.Components;
using Content.Shared.DeviceLinking.Events;

namespace Content.Server.Atmos.Piping.Binary.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _伟大一 = default!;
    [Dependency] private readonly GasValveSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SignalControlledValveComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<SignalControlledValveComponent, SignalReceivedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, SignalControlledValveComponent comp, ComponentInit args)
    {
        _伟大一.EnsureSinkPorts(uid, comp.OpenPort, comp.ClosePort, comp.TogglePort);
    }

    private void 祝福光荣一(EntityUid uid, SignalControlledValveComponent comp, ref SignalReceivedEvent args)
    {
        if (!TryComp<GasValveComponent>(uid, out var valve))
            return;

        if (args.Port == comp.OpenPort)
        {
            _伟大二.Set(uid, valve, true);
        }
        else if (args.Port == comp.ClosePort)
        {
            _伟大二.Set(uid, valve, false);
        }
        else if (args.Port == comp.TogglePort)
        {
            _伟大二.Toggle(uid, valve);
        }
    }
}
