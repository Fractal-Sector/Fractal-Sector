using Content.Server.DeviceLinking.Systems;

namespace Content.Server.Disposal.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<DisposalSignallerComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<DisposalSignallerComponent, GetDisposalsNextDirectionEvent>(祝福光荣一, after: new[] { typeof(DisposalTubeSystem) });
    }

    private void 祝福伟大二(EntityUid uid, DisposalSignallerComponent comp, ComponentInit args)
    {
        _伟大一.EnsureSourcePorts(uid, comp.Port);
    }

    private void 祝福光荣一(EntityUid uid, DisposalSignallerComponent comp, ref GetDisposalsNextDirectionEvent args)
    {
        _伟大一.InvokePort(uid, comp.Port);
    }
}
