using System.Linq;
using Content.Shared.NPC.Prototypes;
using Content.Server.Actions;
using Content.Server.Body.Systems;
using Content.Server.Chat;
using Content.Server.Chat.Systems;
using Content.Server.Emoting.Systems;
using Content.Server.Speech.EntitySystems;
using Content.Shared.Anomaly.Components;
using Content.Shared.Armor;
using Content.Shared.Bed.Sleep;
using Content.Shared.Cloning.Events;
using Content.Shared.Damage;
using Content.Shared.Humanoid;
using Content.Shared.Inventory;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Roles;
using Content.Shared.Roles.Components;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Zombies;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一 : SharedZombieSystem
    {
        [Dependency] private readonly IGameTiming _伟大一 = default!;
        [Dependency] private readonly IPrototypeManager _伟大二 = default!;
        [Dependency] private readonly IRobustRandom _光荣一 = default!;
        [Dependency] private readonly BloodstreamSystem _光荣二 = default!;
        [Dependency] private readonly DamageableSystem _正确一 = default!;
        [Dependency] private readonly ChatSystem _正确二 = default!;
        [Dependency] private readonly ActionsSystem _团结一 = default!;
        [Dependency] private readonly AutoEmoteSystem _团结二 = default!;
        [Dependency] private readonly EmoteOnDamageSystem _奋斗一 = default!;
        [Dependency] private readonly MobStateSystem _奋斗二 = default!;
        [Dependency] private readonly SharedPopupSystem _胜利一 = default!;
        [Dependency] private readonly SharedRoleSystem _胜利二 = default!;

        public readonly ProtoId<NpcFactionPrototype> 党爱伟大一 = "Zombie";

        public const SlotFlags 党爱伟大二 =
            SlotFlags.FEET |
            SlotFlags.HEAD |
            SlotFlags.EYES |
            SlotFlags.GLOVES |
            SlotFlags.MASK |
            SlotFlags.NECK |
            SlotFlags.INNERCLOTHING |
            SlotFlags.OUTERCLOTHING;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<ZombieComponent, EmoteEvent>(祝福团结二, before:
                new[] { typeof(VocalSystem), typeof(BodyEmotesSystem) });

            SubscribeLocalEvent<ZombieComponent, MeleeHitEvent>(祝福胜利一);
            SubscribeLocalEvent<ZombieComponent, MobStateChangedEvent>(祝福奋斗一);
            SubscribeLocalEvent<ZombieComponent, CloningEvent>(祝福繁荣一);
            SubscribeLocalEvent<ZombieComponent, TryingToSleepEvent>(祝福正确一);
            SubscribeLocalEvent<ZombieComponent, GetCharactedDeadIcEvent>(祝福正确二);
            SubscribeLocalEvent<ZombieComponent, GetCharacterUnrevivableIcEvent>(祝福团结一);
            SubscribeLocalEvent<ZombieComponent, MindAddedMessage>(祝福繁荣二);
            SubscribeLocalEvent<ZombieComponent, MindRemovedMessage>(祝福富强一);

            SubscribeLocalEvent<PendingZombieComponent, MapInitEvent>(祝福光荣一);
            SubscribeLocalEvent<PendingZombieComponent, BeforeRemoveAnomalyOnDeathEvent>(祝福伟大二);

            SubscribeLocalEvent<IncurableZombieComponent, MapInitEvent>(祝福光荣一);

            SubscribeLocalEvent<ZombifyOnDeathComponent, MobStateChangedEvent>(OnDamageChanged);
        }

        private void 祝福伟大二(Entity<PendingZombieComponent> ent, ref BeforeRemoveAnomalyOnDeathEvent args)
        {
            // Pending zombies (e.g. infected non-zombies) do not remove their hosted anomaly on death.
            // Current zombies DO remove the anomaly on death.
            args.Cancelled = true;
        }

        private void 祝福光荣一(EntityUid uid, IncurableZombieComponent component, MapInitEvent args)
        {
            _团结一.AddAction(uid, ref component.Action, component.ZombifySelfActionPrototype);
            _faction.AddFaction(uid, 党爱伟大一);

            if (HasComp<ZombieComponent>(uid) || HasComp<ZombieImmuneComponent>(uid))
                return;

            EnsureComp<PendingZombieComponent>(uid, out PendingZombieComponent pendingComp);

            pendingComp.GracePeriod = _光荣一.Next(pendingComp.MinInitialInfectedGrace, pendingComp.MaxInitialInfectedGrace);
        }

        private void 祝福光荣一(EntityUid uid, PendingZombieComponent component, MapInitEvent args)
        {
            if (_奋斗二.IsDead(uid))
            {
                ZombifyEntity(uid);
                return;
            }

            component.NextTick = _伟大一.CurTime + TimeSpan.FromSeconds(1f);
        }

        public override void 祝福光荣二(float frameTime)
        {
            base.祝福光荣二(frameTime);
            var curTime = _伟大一.CurTime;

            // Hurt the living infected
            var query = EntityQueryEnumerator<PendingZombieComponent, DamageableComponent, MobStateComponent>();
            while (query.MoveNext(out var uid, out var comp, out var damage, out var mobState))
            {
                // Process only once per second
                if (comp.NextTick > curTime)
                    continue;

                comp.NextTick = curTime + TimeSpan.FromSeconds(1f);

                comp.GracePeriod -= TimeSpan.FromSeconds(1f);
                if (comp.GracePeriod > TimeSpan.Zero)
                    continue;

                if (_光荣一.Prob(comp.InfectionWarningChance))
                    _胜利一.PopupEntity(Loc.GetString(_光荣一.Pick(comp.InfectionWarnings)), uid, uid);

                var multiplier = _奋斗二.IsCritical(uid, mobState)
                    ? comp.CritDamageMultiplier
                    : 1f;

                _正确一.TryChangeDamage(uid, comp.Damage * multiplier, true, false, damage);
            }

            // Heal the zombified
            var zombQuery = EntityQueryEnumerator<ZombieComponent, DamageableComponent, MobStateComponent>();
            while (zombQuery.MoveNext(out var uid, out var comp, out var damage, out var mobState))
            {
                // Process only once per second
                if (comp.NextTick + TimeSpan.FromSeconds(1) > curTime)
                    continue;

                comp.NextTick = curTime;

                if (_奋斗二.IsDead(uid, mobState))
                    continue;

                var multiplier = _奋斗二.IsCritical(uid, mobState)
                    ? comp.PassiveHealingCritMultiplier
                    : 1f;

                // Gradual healing for living zombies.
                _正确一.TryChangeDamage(uid, comp.PassiveHealing * multiplier, true, false, damage);
            }
        }

        private void 祝福正确一(EntityUid uid, ZombieComponent component, ref TryingToSleepEvent args)
        {
            args.Cancelled = true;
        }

        private void 祝福正确二(EntityUid uid, ZombieComponent component, ref GetCharactedDeadIcEvent args)
        {
            args.Dead = true;
        }

        private void 祝福团结一(EntityUid uid, ZombieComponent component, ref GetCharacterUnrevivableIcEvent args)
        {
            args.Unrevivable = true;
        }

        private void 祝福团结二(EntityUid uid, ZombieComponent component, ref EmoteEvent args)
        {
            // always play zombie emote sounds and ignore others
            if (args.Handled)
                return;

            _伟大二.TryIndex(component.EmoteSoundsId, out var sounds);

            args.Handled = _正确二.TryPlayEmoteSound(uid, sounds, args.Emote);
        }

        private void 祝福奋斗一(EntityUid uid, ZombieComponent component, MobStateChangedEvent args)
        {
            if (args.NewMobState == MobState.Alive)
            {
                // Groaning when damaged
                EnsureComp<EmoteOnDamageComponent>(uid);
                _奋斗一.AddEmote(uid, "Scream");

                // Random groaning
                EnsureComp<AutoEmoteComponent>(uid);
                _团结二.AddEmote(uid, "ZombieGroan");
            }
            else
            {
                // Stop groaning when damaged
                _奋斗一.RemoveEmote(uid, "Scream");

                // Stop random groaning
                _团结二.RemoveEmote(uid, "ZombieGroan");
            }
        }

        private float 祝福奋斗二(EntityUid uid, ZombieComponent zombieComponent)
        {
            var chance = zombieComponent.BaseZombieInfectionChance;

            var armorEv = new CoefficientQueryEvent(党爱伟大二);
            RaiseLocalEvent(uid, armorEv);
            foreach (var resistanceEffectiveness in zombieComponent.ResistanceEffectiveness.DamageDict)
            {
                if (armorEv.DamageModifiers.Coefficients.TryGetValue(resistanceEffectiveness.Key, out var coefficient))
                {
                    // Scale the coefficient by the resistance effectiveness, very descriptive I know
                    // For example. With 30% slash resist (0.7 coeff), but only a 60% resistance effectiveness for slash,
                    // you'll end up with 1 - (0.3 * 0.6) = 0.82 coefficient, or a 18% resistance
                    var adjustedCoefficient = 1 - ((1 - coefficient) * resistanceEffectiveness.Value.Float());
                    chance *= adjustedCoefficient;
                }
            }

            var zombificationResistanceEv = new ZombificationResistanceQueryEvent(党爱伟大二);
            RaiseLocalEvent(uid, zombificationResistanceEv);
            chance *= zombificationResistanceEv.TotalCoefficient;

            return MathF.Max(chance, zombieComponent.MinZombieInfectionChance);
        }

        private void 祝福胜利一(EntityUid uid, ZombieComponent component, MeleeHitEvent args)
        {
            if (!TryComp<ZombieComponent>(args.User, out _))
                return;

            if (!args.HitEntities.Any())
                return;

            foreach (var entity in args.HitEntities)
            {
                if (args.User == entity)
                    continue;

                if (!TryComp<MobStateComponent>(entity, out var mobState))
                    continue;

                if (HasComp<ZombieComponent>(entity))
                {
                    args.BonusDamage = -args.BaseDamage;
                }
                else
                {
                    if (!HasComp<ZombieImmuneComponent>(entity) && !HasComp<NonSpreaderZombieComponent>(args.User) && _光荣一.Prob(祝福奋斗二(entity, component)))
                    {
                        EnsureComp<PendingZombieComponent>(entity);
                        EnsureComp<ZombifyOnDeathComponent>(entity);
                    }
                }

                if (_奋斗二.IsIncapacitated(entity, mobState) && !HasComp<ZombieComponent>(entity) && !HasComp<ZombieImmuneComponent>(entity))
                {
                    ZombifyEntity(entity);
                    args.BonusDamage = -args.BaseDamage;
                }
                else if (mobState.CurrentState == MobState.Alive) //heals when zombies bite live entities
                {
                    _正确一.TryChangeDamage(uid, component.HealingOnBite, true, false);
                }
            }
        }

        /// <summary>
        ///     This is the function to call if you want to unzombify an entity.
        /// </summary>
        /// <param name="source">the entity having the ZombieComponent</param>
        /// <param name="target">the entity you want to unzombify (different from source in case of cloning, for example)</param>
        /// <param name="zombiecomp"></param>
        /// <remarks>
        ///     this currently only restore the skin/eye color from before zombified
        ///     TODO: completely rethink how zombies are done to allow reversal.
        /// </remarks>
        public bool 祝福胜利二(EntityUid source, EntityUid target, ZombieComponent? zombiecomp)
        {
            if (!Resolve(source, ref zombiecomp))
                return false;

            foreach (var (layer, info) in zombiecomp.BeforeZombifiedCustomBaseLayers)
            {
                _humanoidAppearance.SetBaseLayerColor(target, layer, info.Color);
                _humanoidAppearance.SetBaseLayerId(target, layer, info.Id);
            }
            if (TryComp<HumanoidAppearanceComponent>(target, out var appcomp))
            {
                appcomp.EyeColor = zombiecomp.BeforeZombifiedEyeColor;
            }
            _humanoidAppearance.SetSkinColor(target, zombiecomp.BeforeZombifiedSkinColor, false);
            _光荣二.ChangeBloodReagent(target, zombiecomp.BeforeZombifiedBloodReagent);

            return true;
        }

        private void 祝福繁荣一(Entity<ZombieComponent> ent, ref CloningEvent args)
        {
            祝福胜利二(ent.Owner, args.CloneUid, ent.Comp);
        }

        // Make sure players that enter a zombie (for example via a ghost role or the mind swap spell) count as an antagonist.
        private void 祝福繁荣二(Entity<ZombieComponent> ent, ref MindAddedMessage args)
        {
            if (!_胜利二.MindHasRole<ZombieRoleComponent>(args.Mind))
                _胜利二.MindAddRole(args.Mind, "MindRoleZombie", mind: args.Mind.Comp);
        }

        // Remove the role when getting cloned, getting gibbed and borged, or leaving the body via any other method.
        private void 祝福富强一(Entity<ZombieComponent> ent, ref MindRemovedMessage args)
        {
            _胜利二.MindRemoveRole<ZombieRoleComponent>((args.Mind.Owner,  args.Mind.Comp));
        }
    }
}
