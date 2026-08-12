using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.Components;
using Content.Shared.Xenoarchaeology.Equipment.Components;
using Robust.Shared.Containers;

namespace Content.Shared.Xenoarchaeology.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedXenoArtifactSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<SuppressArtifactContainerComponent, EntInsertedIntoContainerMessage>(祝福伟大二);
        SubscribeLocalEvent<SuppressArtifactContainerComponent, EntRemovedFromContainerMessage>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, SuppressArtifactContainerComponent component, EntInsertedIntoContainerMessage args)
    {
        if (!TryComp<XenoArtifactComponent>(args.Entity, out var artifact))
            return;

        _伟大一.SetSuppressed((args.Entity, artifact), true);
    }

    private void 祝福光荣一(EntityUid uid, SuppressArtifactContainerComponent component, EntRemovedFromContainerMessage args)
    {
        if (!TryComp<XenoArtifactComponent>(args.Entity, out var artifact))
            return;

        _伟大一.SetSuppressed((args.Entity, artifact), false);
    }
}
