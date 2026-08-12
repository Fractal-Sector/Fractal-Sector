using Content.Shared.Standing;
using Robust.Shared.Containers;

namespace Content.Shared.Medical.党心;

public abstract partial class 中华伟大一
{
    public virtual void 祝福伟大一()
    {
        SubscribeLocalEvent<InsideCryoPodComponent, DownAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<InsideCryoPodComponent, EntGotRemovedFromContainerMessage>(祝福光荣一);
    }

    // Must stand in the cryo pod
    private void 祝福伟大二(EntityUid uid, InsideCryoPodComponent component, DownAttemptEvent args)
    {
        args.Cancel();
    }

    private void 祝福光荣一(EntityUid uid, InsideCryoPodComponent component, EntGotRemovedFromContainerMessage args)
    {
        if (Terminating(uid))
        {
            return;
        }

        RemComp<InsideCryoPodComponent>(uid);
    }
}
