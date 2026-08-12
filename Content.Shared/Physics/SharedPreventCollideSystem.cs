using Robust.Shared.Physics.Events;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PreventCollideComponent, PreventCollideEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, PreventCollideComponent component, ref PreventCollideEvent args)
    {
        if (component.Uid == args.OtherEntity)
            args.Cancelled = true;
    }

}
