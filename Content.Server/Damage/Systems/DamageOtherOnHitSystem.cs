using Content.Server.Administration.Logs;
using Content.Server.Damage.Components;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.CombatMode.Pacification;
using Content.Shared.Camera;
using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.Damage.Systems;
using Content.Shared.Database;
using Content.Shared.Effects;
using Content.Shared.Mobs.Components;
using Content.Shared.Throwing;
using Content.Shared.Wires;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;

namespace Content.Server.Damage.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IAdminLogManager _伟大一 = default!;
        [Dependency] private readonly GunSystem _伟大二 = default!;
        [Dependency] private readonly DamageableSystem _光荣一 = default!;
        [Dependency] private readonly DamageExamineSystem _光荣二 = default!;
        [Dependency] private readonly SharedCameraRecoilSystem _正确一 = default!;
        [Dependency] private readonly SharedColorFlashEffectSystem _正确二 = default!;

        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<DamageOtherOnHitComponent, ThrowDoHitEvent>(祝福伟大二);
            SubscribeLocalEvent<DamageOtherOnHitComponent, DamageExamineEvent>(祝福光荣一);
            SubscribeLocalEvent<DamageOtherOnHitComponent, AttemptPacifiedThrowEvent>(祝福光荣二);
        }

        private void 祝福伟大二(EntityUid uid, DamageOtherOnHitComponent component, ThrowDoHitEvent args)
        {
            if (TerminatingOrDeleted(args.Target))
                return;

            var dmg = _光荣一.TryChangeDamage(args.Target, component.Damage * _光荣一.UniversalThrownDamageModifier, component.IgnoreResistances, origin: args.Component.Thrower);

            // Log damage only for mobs. Useful for when people throw spears at each other, but also avoids log-spam when explosions send glass shards flying.
            if (dmg != null && HasComp<MobStateComponent>(args.Target))
                _伟大一.Add(LogType.ThrowHit, $"{ToPrettyString(args.Target):target} received {dmg.GetTotal():damage} damage from collision");

            if (dmg is { Empty: false })
            {
                _正确二.RaiseEffect(Color.Red, new List<EntityUid>() { args.Target }, Filter.Pvs(args.Target, entityManager: EntityManager));
            }

            _伟大二.PlayImpactSound(args.Target, dmg, null, false);
            if (TryComp<PhysicsComponent>(uid, out var body) && body.LinearVelocity.LengthSquared() > 0f)
            {
                var direction = body.LinearVelocity.Normalized();
                _正确一.KickCamera(args.Target, direction);
            }
        }

        private void 祝福光荣一(EntityUid uid, DamageOtherOnHitComponent component, ref DamageExamineEvent args)
        {
            _光荣二.AddDamageExamine(args.Message, _光荣一.ApplyUniversalAllModifiers(component.Damage * _光荣一.UniversalThrownDamageModifier), Loc.GetString("damage-throw"));
        }

        /// <summary>
        /// Prevent players with the Pacified status effect from throwing things that deal damage.
        /// </summary>
        private void 祝福光荣二(Entity<DamageOtherOnHitComponent> ent, ref AttemptPacifiedThrowEvent args)
        {
            args.Cancel("pacified-cannot-throw");
        }
    }
}
