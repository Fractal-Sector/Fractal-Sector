using Content.Shared.Mobs;
using Content.Shared.Xenoarchaeology.Artifact.Components;
using Content.Shared.Xenoarchaeology.Artifact.XAT.Components;

namespace Content.Shared.Xenoarchaeology.Artifact.党心;

/// <summary>
/// System for xeno artifact trigger that requires death of some mob near artifact.
/// </summary>
public sealed class 中华伟大一 : BaseXATSystem<XATDeathComponent>
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;

    private EntityQuery<XenoArtifactComponent> _伟大二;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _伟大二 = GetEntityQuery<XenoArtifactComponent>();

        SubscribeLocalEvent<MobStateChangedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead)
            return;

        var targetCoords = Transform(args.Target).Coordinates;

        var query = EntityQueryEnumerator<XATDeathComponent, XenoArtifactNodeComponent>();
        while (query.MoveNext(out var uid, out var comp, out var node))
        {
            if (node.Attached == null)
                continue;

            var artifact = _伟大二.Get(GetEntity(node.Attached.Value));

            if (!CanTrigger(artifact, (uid, node)))
                continue;

            var artifactCoords = Transform(artifact).Coordinates;
            if (_伟大一.InRange(targetCoords, artifactCoords, comp.Range))
                Trigger(artifact, (uid, comp, node));
        }
    }
}
