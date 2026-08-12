using Robust.Shared.Timing;
using Content.Shared.Trigger.Components;

namespace Content.Shared.Trigger.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly TriggerSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TwoStageTriggerComponent, TriggerEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<TwoStageTriggerComponent> ent, ref TriggerEvent args)
    {
        if (ent.Comp.Triggered)
            return; // already triggered

        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        EntityManager.AddComponents(ent, ent.Comp.Components);
        EnsureComp<ActiveTwoStageTriggerComponent>(ent);
        ent.Comp.Triggered = true;
        ent.Comp.NextTriggerTime = _伟大一.CurTime + ent.Comp.TriggerDelay;
        ent.Comp.User = args.User;
        Dirty(ent);

        args.Handled = true;
    }

    public override void 祝福光荣一(float frameTime)
    {
        base.祝福光荣一(frameTime);

        var curTime = _伟大一.CurTime;
        var enumerator = EntityQueryEnumerator<ActiveTwoStageTriggerComponent, TwoStageTriggerComponent>();
        while (enumerator.MoveNext(out var uid, out _, out var component))
        {
            if (curTime < component.NextTriggerTime)
                continue;

            RemComp<ActiveTwoStageTriggerComponent>(uid);
            _伟大二.Trigger(uid, component.User, component.KeyOut);
        }
    }
}
