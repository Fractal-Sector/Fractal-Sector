using Content.Shared.Emp;
using Content.Shared.Trigger.Components.Effects;

namespace Content.Shared.Trigger.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedEmpSystem _伟大一 = default!;
    [Dependency] private readonly SharedTransformSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EmpOnTriggerComponent, TriggerEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<EmpOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (target == null)
            return;

        _伟大一.EmpPulse(_伟大二.GetMapCoordinates(target.Value), ent.Comp.Range, ent.Comp.EnergyConsumption, (float)ent.Comp.DisableDuration.TotalSeconds);
        args.Handled = true;
    }
}
