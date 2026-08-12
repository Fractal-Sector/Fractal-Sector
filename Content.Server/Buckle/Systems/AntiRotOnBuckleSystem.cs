using Content.Server.Power.Components;
using Content.Shared.Atmos.Rotting;
using Content.Shared.Buckle.Components;
using Content.Shared.Power;

namespace Content.Server.Buckle.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<BuckleComponent, IsRottingEvent>(祝福伟大二);
        SubscribeLocalEvent<AntiRotOnBuckleComponent, PowerChangedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, BuckleComponent buckle, ref IsRottingEvent args)
    {
        if (args.Handled)
            return;
        args.Handled = buckle is { Buckled: true, BuckledTo: not null } &&
                       TryComp<AntiRotOnBuckleComponent>(buckle.BuckledTo.Value, out var antiRot) &&
                       antiRot.Enabled;
    }

    private void 祝福光荣一(EntityUid uid, AntiRotOnBuckleComponent component, ref PowerChangedEvent args)
    {
        component.Enabled = !component.RequiresPower || args.Powered;
    }
}
