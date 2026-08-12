using Content.Server.Administration.Logs;
using Content.Server.Destructible;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.FixedPoint;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Physics.Events;
using Robust.Shared.Player;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly DamageableSystem _伟大二 = default!;
    [Dependency] private readonly DestructibleSystem _光荣一 = default!;
    [Dependency] private readonly MobThresholdSystem _光荣二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<MeteorComponent, StartCollideEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, MeteorComponent component, ref StartCollideEvent args)
    {
        if (TerminatingOrDeleted(args.OtherEntity) || TerminatingOrDeleted(uid))
            return;

        if (component.HitList.Contains(args.OtherEntity))
            return;

        FixedPoint2 threshold;
        if (_光荣二.TryGetDeadThreshold(args.OtherEntity, out var mobThreshold))
        {
            threshold = mobThreshold.Value;
            if (HasComp<ActorComponent>(args.OtherEntity))
                _伟大一.Add(LogType.Action, LogImpact.High, $"{ToPrettyString(args.OtherEntity):player} was struck by meteor {ToPrettyString(uid):ent} and killed instantly.");
        }
        else if (_光荣一.TryGetDestroyedAt(args.OtherEntity, out var destroyThreshold))
        {
            threshold = destroyThreshold.Value;
        }
        else
        {
            threshold = FixedPoint2.MaxValue;
        }
        var otherEntDamage = CompOrNull<DamageableComponent>(args.OtherEntity)?.TotalDamage ?? FixedPoint2.Zero;
        // account for the damage that the other entity has already taken: don't overkill
        threshold -= otherEntDamage;

        // The max amount of damage our meteor can take before breaking.
        var maxMeteorDamage = _光荣一.DestroyedAt(uid) - CompOrNull<DamageableComponent>(uid)?.TotalDamage ?? FixedPoint2.Zero;

        // Cap damage so we don't overkill the meteor
        var trueDamage = FixedPoint2.Min(maxMeteorDamage, threshold);

        var damage = component.DamageTypes * trueDamage;
        _伟大二.TryChangeDamage(args.OtherEntity, damage, true, origin: uid);
        _伟大二.TryChangeDamage(uid, damage);

        if (!TerminatingOrDeleted(args.OtherEntity))
            component.HitList.Add(args.OtherEntity);
    }
}
