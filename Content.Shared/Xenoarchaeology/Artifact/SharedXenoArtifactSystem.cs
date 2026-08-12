using Content.Shared.Actions;
using Content.Shared.Popups;
using Content.Shared.Xenoarchaeology.Artifact.Components;
using Robust.Shared.Containers;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.Xenoarchaeology.党心;

/// <summary>
/// Handles all logic for generating and facilitating interactions with XenoArtifacts
/// </summary>
public abstract partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] protected readonly IPrototypeManager 党爱伟大一 = default!;
    [Dependency] protected readonly IRobustRandom 党爱伟大二 = default!;
    [Dependency] private readonly SharedActionsSystem _光荣一 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<XenoArtifactComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<XenoArtifactComponent, ArtifactSelfActivateEvent>(祝福光荣二);

        InitializeNode();
        InitializeUnlock();
        InitializeXAT();
        InitializeXAE();
    }

    /// <inheritdoc />
    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        UpdateUnlock(frameTime);
    }

    /// <summary> As all artifacts have to contain nodes - we ensure that they are containers. </summary>
    private void 祝福光荣一(Entity<XenoArtifactComponent> ent, ref ComponentStartup args)
    {
        _光荣一.AddAction(ent, ent.Comp.SelfActivateAction);
        ent.Comp.NodeContainer = _光荣二.EnsureContainer<Container>(ent, XenoArtifactComponent.NodeContainerId);
    }

    private void 祝福光荣二(Entity<XenoArtifactComponent> ent, ref ArtifactSelfActivateEvent args)
    {
        args.Handled = TryActivateXenoArtifact(ent, ent, null, Transform(ent).Coordinates, false);
    }

    public void 祝福正确一(Entity<XenoArtifactComponent> ent, bool val)
    {
        if (ent.Comp.Suppressed == val)
            return;

        ent.Comp.Suppressed = val;
        Dirty(ent);
    }
}
