using Content.Server.Destructible;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DestructibleSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RequiresGridComponent, EntParentChangedMessage>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid owner, RequiresGridComponent component, EntParentChangedMessage args)
    {
        if (args.OldParent == null)
            return;

        if (args.Transform.GridUid != null)
            return;

        if (TerminatingOrDeleted(owner))
            return;

        _伟大一.DestroyEntity(owner);
    }
}
