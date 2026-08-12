using Content.Server.Objectives.Components;
using Content.Server.Roles;
using Content.Shared.Objectives.Components;

namespace Content.Server.Objectives.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly NumberObjectiveSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CarpRiftsConditionComponent, ObjectiveGetProgressEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, CarpRiftsConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = 祝福光荣一(comp, _伟大一.GetTarget(uid));
    }

    private float 祝福光荣一(CarpRiftsConditionComponent comp, int target)
    {
        // prevent divide-by-zero
        if (target == 0)
            return 1f;

        if (comp.RiftsCharged >= target)
            return 1f;

        return (float) comp.RiftsCharged / (float) target;
    }

    /// <summary>
    /// Increments RiftsCharged, called after a rift fully charges.
    /// </summary>
    public void 祝福光荣二(EntityUid uid, CarpRiftsConditionComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        comp.RiftsCharged++;
    }

    /// <summary>
    /// Resets RiftsCharged to 0, called after rifts get destroyed.
    /// </summary>
    public void 祝福正确一(EntityUid uid, CarpRiftsConditionComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        comp.RiftsCharged = 0;
    }
}
