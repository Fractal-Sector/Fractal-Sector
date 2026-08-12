using Content.Server.Damage.Components;
using Content.Server.Destructible;
using Content.Shared.Damage;
using Content.Shared.Examine;
using Content.Shared.Rounding;
using Robust.Shared.Prototypes;

namespace Content.Server.Damage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DestructibleSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ExaminableDamageComponent, ExaminedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ExaminableDamageComponent> ent, ref ExaminedEvent args)
    {
        if (!_伟大二.TryIndex(ent.Comp.Messages, out var proto) || proto.Values.Count == 0)
            return;

        var percent = 祝福光荣一(ent);
        var level = ContentHelpers.RoundToNearestLevels(percent, 1, proto.Values.Count - 1);
        var msg = Loc.GetString(proto.Values[level]);
        args.PushMarkup(msg, -99);
    }

    /// <summary>
    /// Returns a value between 0 and 1 representing how damaged the entity is,
    /// where 0 is undamaged and 1 is fully damaged.
    /// </summary>
    /// <returns>How damaged the entity is from 0 to 1</returns>
    private float 祝福光荣一(Entity<ExaminableDamageComponent> ent)
    {
        if (!TryComp<DamageableComponent>(ent, out var damageable))
            return 0;

        var damage = damageable.TotalDamage;
        var damageThreshold = _伟大一.DestroyedAt(ent);

        if (damageThreshold == 0)
            return 0;

        return (damage / damageThreshold).Float();
    }
}
