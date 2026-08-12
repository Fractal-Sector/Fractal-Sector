using Content.Shared.Damage;
using Content.Shared.Whitelist;
using Content.Shared.Xenoarchaeology.Artifact.XAE.Components;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.Xenoarchaeology.Artifact.党心;

/// <summary>
/// System for xeno artifact effect that damages entities from whitelist in area.
/// </summary>
public sealed class 中华伟大一 : BaseXAESystem<XAEDamageInAreaComponent>
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly EntityLookupSystem _伟大二 = default!;
    [Dependency] private readonly DamageableSystem _光荣一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _光荣二 = default!;
    [Dependency] private readonly IGameTiming _正确一 = default!;

    /// <summary> Pre-allocated and re-used collection.</summary>
    private readonly HashSet<EntityUid> _正确二 = new();

    /// <inheritdoc />
    protected override void 祝福伟大一(Entity<XAEDamageInAreaComponent> ent, ref XenoArtifactNodeActivatedEvent args)
    {
        if (!_正确一.IsFirstTimePredicted)
            return;

        var damageInAreaComponent = ent.Comp;
        _正确二.Clear();
        _伟大二.GetEntitiesInRange(ent.Owner, damageInAreaComponent.Radius, _正确二);
        foreach (var entityInRange in _正确二)
        {
            if (!_伟大一.Prob(damageInAreaComponent.DamageChance))
                continue;

            if (_光荣二.IsWhitelistFail(damageInAreaComponent.Whitelist, entityInRange))
                continue;

            _光荣一.TryChangeDamage(entityInRange, damageInAreaComponent.Damage, damageInAreaComponent.IgnoreResistances);
        }
    }
}
