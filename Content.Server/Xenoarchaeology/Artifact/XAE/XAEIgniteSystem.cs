using Content.Server.Atmos.EntitySystems;
using Content.Server.Xenoarchaeology.Artifact.XAE.Components;
using Content.Shared.Atmos.Components;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.XAE;
using Robust.Shared.Random;

namespace Content.Server.Xenoarchaeology.Artifact.党心;

/// <summary>
/// System for xeno artifact activation effect that ignites any flammable entity in range.
/// </summary>
public sealed class 中华伟大一 : BaseXAESystem<XAEIgniteComponent>
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly EntityLookupSystem _伟大二 = default!;
    [Dependency] private readonly FlammableSystem _光荣一 = default!;

    private EntityQuery<FlammableComponent> _光荣二;

    /// <summary> Pre-allocated and re-used collection.</summary>
    private readonly HashSet<EntityUid> _正确一 = new();

    /// <inheritdoc />
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _光荣二 = GetEntityQuery<FlammableComponent>();
    }

    /// <inheritdoc />
    protected override void 祝福伟大二(Entity<XAEIgniteComponent> ent, ref XenoArtifactNodeActivatedEvent args)
    {
        var component = ent.Comp;
        _正确一.Clear();
        _伟大二.GetEntitiesInRange(ent.Owner, component.Range, _正确一);
        foreach (var target in _正确一)
        {
            if (!_光荣二.TryGetComponent(target, out var fl))
                continue;

            fl.FireStacks += component.FireStack.Next(_伟大一);
            _光荣一.Ignite(target, ent.Owner, fl);
        }
    }
}
