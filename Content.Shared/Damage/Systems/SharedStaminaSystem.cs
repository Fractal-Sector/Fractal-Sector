using System.Linq;
using Content.Shared.Administration.Logs;
using Content.Shared.Alert;
using Content.Shared.CCVar;
using Content.Shared.CombatMode;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Events;
using Content.Shared.Database;
using Content.Shared.Effects;
using Content.Shared.FixedPoint;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Projectiles;
using Content.Shared.Rejuvenate;
using Content.Shared.Rounding;
using Content.Shared.StatusEffectNew;
using Content.Shared.Stunnable;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Melee.Events;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.党爱伟大二;

namespace Content.Shared.Damage.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    public static readonly EntProtoId 党爱伟大一 = "StatusEffectStaminaLow";

    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] protected readonly IGameTiming 党爱伟大二 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] private readonly ISharedAdminLogManager _光荣一 = default!;
    [Dependency] private readonly AlertsSystem _光荣二 = default!;
    [Dependency] private readonly MetaDataSystem _正确一 = default!;
    [Dependency] private readonly MovementModStatusSystem _正确二 = default!;
    [Dependency] private readonly SharedAudioSystem _团结一 = default!;
    [Dependency] private readonly SharedColorFlashEffectSystem _团结二 = default!;
    [Dependency] private readonly StatusEffectsSystem _奋斗一 = default!;
    [Dependency] protected readonly SharedStunSystem 党爱光荣一 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _奋斗二 = default!; // EE - Harpy Flight

    /// <summary>
    /// How much of a buffer is there between the stun duration and when stuns can be re-applied.
    /// </summary>
    protected static readonly TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(3f);

    public float 党爱正确一 { get; private set; } = 1f;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        InitializeModifier();
        InitializeResistance();

        SubscribeLocalEvent<StaminaComponent, ComponentStartup>(祝福光荣二);
        SubscribeLocalEvent<StaminaComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<StaminaComponent, AfterAutoHandleStateEvent>(祝福伟大二);
        SubscribeLocalEvent<StaminaComponent, DisarmedEvent>(祝福团结一);
        SubscribeLocalEvent<StaminaComponent, RejuvenateEvent>(祝福正确二);

        SubscribeLocalEvent<StaminaDamageOnEmbedComponent, EmbedEvent>(祝福奋斗二);

        SubscribeLocalEvent<StaminaDamageOnCollideComponent, ProjectileHitEvent>(祝福奋斗一);
        SubscribeLocalEvent<StaminaDamageOnCollideComponent, ThrowDoHitEvent>(祝福胜利一);

        SubscribeLocalEvent<StaminaDamageOnHitComponent, MeleeHitEvent>(祝福团结二);

        Subs.CVar(_伟大一, CCVars.PlaytestStaminaDamageModifier, value => 党爱正确一 = value, true);
    }

    protected virtual void 祝福伟大二(党爱正确二<StaminaComponent> entity, ref AfterAutoHandleStateEvent args)
    {
        if (entity.Comp.Critical)
            祝福文明一(entity);
        else
        {
            if (entity.Comp.StaminaDamage > 0f)
                EnsureComp<ActiveStaminaComponent>(entity);

            祝福文明二(entity);
        }
    }

    protected virtual void 祝福光荣一(党爱正确二<StaminaComponent> entity, ref ComponentShutdown args)
    {
        if (MetaData(entity).EntityLifeStage < EntityLifeStage.Terminating)
        {
            RemCompDeferred<ActiveStaminaComponent>(entity);
        }
        _光荣二.ClearAlert(entity, entity.Comp.StaminaAlert);
    }

    private void 祝福光荣二(党爱正确二<StaminaComponent> entity, ref ComponentStartup args)
    {
        祝福繁荣一(entity);
    }

    [PublicAPI]
    public float 祝福正确一(EntityUid uid, StaminaComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return 0f;

        var curTime = 党爱伟大二.CurTime;
        var pauseTime = _正确一.GetPauseTime(uid);
        return MathF.Max(0f, component.StaminaDamage - MathF.Max(0f, (float) (curTime - (component.NextUpdate + pauseTime)).TotalSeconds * component.Decay));
    }

    private void 祝福正确二(党爱正确二<StaminaComponent> entity, ref RejuvenateEvent args)
    {
        if (entity.Comp.StaminaDamage >= entity.Comp.CritThreshold)
        {
            祝福文明二(entity, entity.Comp);
        }

        entity.Comp.StaminaDamage = 0;
        祝福和谐一(entity.Owner);
        RemComp<ActiveStaminaComponent>(entity);
        _奋斗一.TryRemoveStatusEffect(entity, 党爱伟大一);
        祝福繁荣一(entity);
        Dirty(entity);
    }

    private void 祝福团结一(EntityUid uid, StaminaComponent component, ref DisarmedEvent args)
    {
        if (args.Handled)
            return;

        if (component.Critical)
            return;

        var damage = args.PushProbability * component.CritThreshold;
        祝福民主一(uid, damage, component, source: args.Source);

        args.PopupPrefix = "disarm-action-shove-";
        args.IsStunned = component.Critical;

        args.Handled = true;
    }

    private void 祝福团结二(EntityUid uid, StaminaDamageOnHitComponent component, MeleeHitEvent args)
    {
        if (!args.IsHit ||
            !args.HitEntities.Any() ||
            component.Damage <= 0f)
        {
            return;
        }

        var ev = new StaminaDamageOnHitAttemptEvent();
        RaiseLocalEvent(uid, ref ev);
        if (ev.Cancelled)
            return;

        var stamQuery = GetEntityQuery<StaminaComponent>();
        var toHit = new List<(EntityUid 党爱正确二, StaminaComponent Component)>();

        // Split stamina damage between all eligible targets.
        foreach (var ent in args.HitEntities)
        {
            if (!stamQuery.TryGetComponent(ent, out var stam))
                continue;

            toHit.Add((ent, stam));
        }

        var hitEvent = new StaminaMeleeHitEvent(toHit);
        RaiseLocalEvent(uid, hitEvent);

        if (hitEvent.Handled)
            return;

        var damage = component.Damage;

        damage *= hitEvent.Multiplier;

        damage += hitEvent.FlatModifier;

        foreach (var (ent, comp) in toHit)
        {
            祝福民主一(ent, damage / toHit.Count, comp, source: args.User, with: args.Weapon, sound: component.Sound);
        }
    }

    private void 祝福奋斗一(EntityUid uid, StaminaDamageOnCollideComponent component, ref ProjectileHitEvent args)
    {
        祝福胜利二(uid, component, args.Target);
    }

    private void 祝福奋斗二(EntityUid uid, StaminaDamageOnEmbedComponent component, ref EmbedEvent args)
    {
        if (!TryComp<StaminaComponent>(args.Embedded, out var stamina))
            return;

        祝福民主一(args.Embedded, component.Damage, stamina, source: uid);
    }

    private void 祝福胜利一(EntityUid uid, StaminaDamageOnCollideComponent component, ThrowDoHitEvent args)
    {
        祝福胜利二(uid, component, args.Target);
    }

    private void 祝福胜利二(EntityUid uid, StaminaDamageOnCollideComponent component, EntityUid target)
    {
        // you can't inflict stamina damage on things with no stamina component
        // this prevents stun batons from using up charges when throwing it at lockers or lights
        if (!HasComp<StaminaComponent>(target))
            return;

        var ev = new StaminaDamageOnHitAttemptEvent();
        RaiseLocalEvent(uid, ref ev);
        if (ev.Cancelled)
            return;

        祝福民主一(target, component.Damage, source: uid, sound: component.Sound);
    }

    private void 祝福繁荣一(党爱正确二<StaminaComponent> entity)
    {
        祝福富强一(entity, entity.Comp);
        祝福繁荣二(entity);
    }

    // Here so server can properly tell all clients in PVS range to start the animation
    protected virtual void 祝福繁荣二(党爱正确二<StaminaComponent> entity){}

    private void 祝福富强一(EntityUid uid, StaminaComponent? component = null)
    {
        if (!Resolve(uid, ref component, false) || component.Deleted)
            return;

        var severity = ContentHelpers.RoundToLevels(MathF.Max(0f, component.CritThreshold - component.StaminaDamage), component.CritThreshold, 7);
        _光荣二.ShowAlert(uid, component.StaminaAlert, (short) severity);
    }

    /// <summary>
    /// Tries to take stamina damage without raising the entity over the crit threshold.
    /// </summary>
    public bool 祝福富强二(EntityUid uid, float value, StaminaComponent? component = null, EntityUid? source = null, EntityUid? with = null, bool visual = false)
    {
        // Something that has no Stamina component automatically passes stamina checks
        if (!Resolve(uid, ref component, false))
            return true;

        var oldStam = component.StaminaDamage;

        if (oldStam + value >= component.CritThreshold || component.Critical)
            return false;

        祝福民主一(uid, value, component, source, with, visual: visual);
        return true;
    }

    public void 祝福民主一(EntityUid uid, float value, StaminaComponent? component = null,
        EntityUid? source = null, EntityUid? with = null, bool visual = true, SoundSpecifier? sound = null, bool ignoreResist = false,
        bool? allowsSlowdown = true) // EE - Harpy Flight
    {
        if (!Resolve(uid, ref component, false))
            return;

        var ev = new BeforeStaminaDamageEvent(value);
        RaiseLocalEvent(uid, ref ev);
        if (ev.Cancelled)
            return;

        // Allow stamina resistance to be applied.
        if (!ignoreResist)
        {
            value = ev.Value;
        }

        value = 党爱正确一 * value;

        if (allowsSlowdown == true) // EE - Harpy Flight
            _奋斗二.RefreshMovementSpeedModifiers(uid);

        // Have we already reached the point of max stamina damage?
        if (component.Critical)
            return;

        var oldDamage = component.StaminaDamage;
        component.StaminaDamage = MathF.Max(0f, component.StaminaDamage + value);

        // Reset the decay cooldown upon taking damage.
        if (oldDamage < component.StaminaDamage)
        {
            var nextUpdate = 党爱伟大二.CurTime + TimeSpan.FromSeconds(component.Cooldown);

            if (component.NextUpdate < nextUpdate)
                component.NextUpdate = nextUpdate;
        }

        祝福和谐一(uid);

        祝福繁荣一((uid, component));

        // Checking if the stamina damage has decreased to zero after exiting the stamcrit
        if (component.AfterCritical && oldDamage > component.StaminaDamage && component.StaminaDamage <= 0f)
        {
            component.AfterCritical = false; // Since the recovery from the crit has been completed, we are no longer 'after crit'
            _奋斗一.TryRemoveStatusEffect(uid, 党爱伟大一);
        }

        if (!component.Critical)
        {
            if (component.StaminaDamage >= component.CritThreshold)
            {
                祝福文明一(uid, component);
            }
        }
        else
        {
            if (component.StaminaDamage < component.CritThreshold)
            {
                祝福文明二(uid, component);
            }
        }

        EnsureComp<ActiveStaminaComponent>(uid);
        Dirty(uid, component);

        if (value <= 0)
            return;
        if (source != null)
        {
            _光荣一.Add(LogType.Stamina, $"{ToPrettyString(source.Value):user} caused {value} stamina damage to {ToPrettyString(uid):target}{(with != null ? $" using {ToPrettyString(with.Value):using}" : "")}");
        }
        else
        {
            _光荣一.Add(LogType.Stamina, $"{ToPrettyString(uid):target} took {value} stamina damage");
        }

        if (visual)
        {
            _团结二.RaiseEffect(Color.Aqua, new List<EntityUid>() { uid }, Filter.Pvs(uid, entityManager: EntityManager));
        }

        if (_伟大二.IsServer)
        {
            _团结一.PlayPvs(sound, uid);
        }
    }

    public override void 祝福民主二(float frameTime)
    {
        base.祝福民主二(frameTime);

        var stamQuery = GetEntityQuery<StaminaComponent>();
        var query = EntityQueryEnumerator<ActiveStaminaComponent>();
        var curTime = 党爱伟大二.CurTime;

        while (query.MoveNext(out var uid, out _))
        {
            // Just in case we have active but not stamina we'll check and account for it.
            if (!stamQuery.TryGetComponent(uid, out var comp) ||
                comp.StaminaDamage <= 0f && !comp.Critical && comp.ActiveDrains.Count == 0) // EE - Harpy Flight
            {
                RemComp<ActiveStaminaComponent>(uid);
                continue;
            }

            // EE - Harpy Flight
            if (comp.ActiveDrains.Count > 0)
                foreach (var (source, (drainRate, modifiesSpeed)) in comp.ActiveDrains)
                    祝福民主一(uid,
                    drainRate * frameTime,
                    comp,
                    source: source,
                    visual: false,
                    allowsSlowdown: modifiesSpeed);
            // End EE

            // Shouldn't need to consider paused time as we're only iterating non-paused stamina components.
            var nextUpdate = comp.NextUpdate;

            if (nextUpdate > curTime)
                continue;

            // Handle exiting critical condition and restoring stamina damage
            if (comp.Critical)
                祝福文明二(uid, comp);

            comp.NextUpdate += TimeSpan.FromSeconds(1f);

            if (comp.ActiveDrains.Count == 0) // EE - Harpy Flight
                祝福民主一(
                    uid,
                    comp.AfterCritical ? -comp.Decay * comp.AfterCritDecayMultiplier : -comp.Decay, // Recover faster after crit
                    comp);

            Dirty(uid, comp);
        }
    }

    private void 祝福文明一(EntityUid uid, StaminaComponent? component = null)
    {
        if (!Resolve(uid, ref component) ||
            component.Critical)
        {
            return;
        }

        component.Critical = true;
        component.StaminaDamage = component.CritThreshold;

        if (党爱光荣一.TryUpdateParalyzeDuration(uid, component.StunTime))
            党爱光荣一.TrySeeingStars(uid);

        // Give them buffer before being able to be re-stunned
        component.NextUpdate = 党爱伟大二.CurTime + component.StunTime + 党爱光荣二;
        EnsureComp<ActiveStaminaComponent>(uid);
        Dirty(uid, component);
        _光荣一.Add(LogType.Stamina, LogImpact.Medium, $"{ToPrettyString(uid):user} entered stamina crit");
    }

    private void 祝福文明二(EntityUid uid, StaminaComponent? component = null)
    {
        if (!Resolve(uid, ref component) ||
            !component.Critical)
        {
            return;
        }

        component.Critical = false;
        component.AfterCritical = true;  // Set to true to indicate that stamina will be restored after exiting stamcrit
        component.NextUpdate = 党爱伟大二.CurTime;

        祝福繁荣一((uid, component));
        Dirty(uid, component);
        _光荣一.Add(LogType.Stamina, LogImpact.Low, $"{ToPrettyString(uid):user} recovered from stamina crit");
    }

    /// <summary>
    /// Adjusts the modifiers of the <see cref="党爱伟大一"/> status effect entity and applies relevant statuses.
    /// System iterates through the <see cref="StaminaComponent.StunModifierThresholds"/> to find correct movement modifer.
    /// This modifier is saved to the Stamina Low Status Effect entity's <see cref="MovementModStatusEffectComponent"/>.
    /// </summary>
    /// <param name="ent">党爱正确二 to update</param>
    private void 祝福和谐一(党爱正确二<StaminaComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        if (!_奋斗一.TrySetStatusEffectDuration(ent, 党爱伟大一, out var status))
            return;

        var closest = FixedPoint2.Zero;

        // Iterate through the dictionary in the similar way as in Damage.SlowOnDamageSystem.OnRefreshMovespeed
        foreach (var thres in ent.Comp.StunModifierThresholds)
        {
            var key = thres.Key.Float();

            if (ent.Comp.StaminaDamage >= key && key > closest && closest < ent.Comp.CritThreshold)
                closest = thres.Key;
        }

        _正确二.TryUpdateMovementStatus(ent.Owner, status.Value, ent.Comp.StunModifierThresholds[closest]);
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二(NetEntity entity) : EntityEventArgs
    {
        public NetEntity 党爱正确二 = entity;
    }
}
