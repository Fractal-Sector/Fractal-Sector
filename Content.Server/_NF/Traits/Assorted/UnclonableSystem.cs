using Content.Shared.Cloning.Events;

namespace Content.Server._NF.Traits.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<UnclonableComponent, CloningAttemptEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<UnclonableComponent> ent, ref CloningAttemptEvent args)
    {
        args.Cancelled = true;
    }
}
