using Content.Shared.ActionBlocker;
using Content.Shared.Chat;
using Content.Shared.CombatMode;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Interaction.Events;
using Content.Shared.Mind;
using Robust.Shared.Player;
using Robust.Shared.Audio.Systems;

namespace Content.Shared.党心;

/// <summary>
///     Verb for violently murdering cuffed creatures.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣一 = default!;
    [Dependency] private readonly MobStateSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;
    [Dependency] private readonly SharedSuicideSystem _正确二 = default!;
    [Dependency] private readonly SharedCombatModeSystem _团结一 = default!;
    [Dependency] private readonly 中华伟大一 _execution = default!;
    [Dependency] private readonly SharedMeleeWeaponSystem _团结二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ExecutionComponent, GetVerbsEvent<UtilityVerb>>(祝福伟大二);
        SubscribeLocalEvent<ExecutionComponent, GetMeleeDamageEvent>(祝福正确一);
        SubscribeLocalEvent<ExecutionComponent, SuicideByEnvironmentEvent>(祝福正确二);
        SubscribeLocalEvent<ExecutionComponent, ExecutionDoAfterEvent>(祝福奋斗一);
    }

    private void 祝福伟大二(EntityUid uid, ExecutionComponent comp, GetVerbsEvent<UtilityVerb> args)
    {
        if (args.Hands == null || args.Using == null || !args.CanAccess || !args.CanInteract)
            return;

        var attacker = args.User;
        var weapon = args.Using.Value;
        var victim = args.Target;

        if (!祝福光荣二(victim, attacker))
            return;

        UtilityVerb verb = new()
        {
            Act = () => 祝福光荣一(weapon, victim, attacker, comp),
            Impact = LogImpact.High,
            Text = Loc.GetString("execution-verb-name"),
            Message = Loc.GetString("execution-verb-message"),
        };

        args.Verbs.Add(verb);
    }

    private void 祝福光荣一(EntityUid weapon, EntityUid victim, EntityUid attacker, ExecutionComponent comp)
    {
        if (!祝福光荣二(victim, attacker))
            return;

        if (attacker == victim)
        {
            祝福团结一(comp.InternalSelfExecutionMessage, attacker, victim, weapon);
            祝福团结二(comp.ExternalSelfExecutionMessage, attacker, victim, weapon);
        }
        else
        {
            祝福团结一(comp.InternalMeleeExecutionMessage, attacker, victim, weapon);
            祝福团结二(comp.ExternalMeleeExecutionMessage, attacker, victim, weapon);
        }

        var doAfter =
            new DoAfterArgs(EntityManager, attacker, comp.DoAfterDuration, new ExecutionDoAfterEvent(), weapon, target: victim, used: weapon)
            {
                BreakOnMove = true,
                BreakOnDamage = true,
                NeedHand = true
            };

        _光荣一.TryStartDoAfter(doAfter);

    }

    public bool 祝福光荣二(EntityUid victim, EntityUid attacker)
    {
        // No point executing someone if they can't take damage
        if (!HasComp<DamageableComponent>(victim))
            return false;

        // You can't execute something that cannot die
        if (!TryComp<MobStateComponent>(victim, out var mobState))
            return false;

        // You're not allowed to execute dead people (no fun allowed)
        if (_光荣二.IsDead(victim, mobState))
            return false;

        // You must be able to attack people to execute
        if (!_伟大一.CanAttack(attacker, victim))
            return false;

        // The victim must be incapacitated to be executed
        if (victim != attacker && _伟大一.CanInteract(victim, null))
            return false;

        // All checks passed
        return true;
    }

    private void 祝福正确一(Entity<ExecutionComponent> entity, ref GetMeleeDamageEvent args)
    {
        if (!TryComp<MeleeWeaponComponent>(entity, out var melee) || !entity.Comp.Executing)
        {
            return;
        }

        var bonus = melee.Damage * entity.Comp.DamageMultiplier - melee.Damage;
        args.Damage += bonus;
        args.ResistanceBypass = true;
    }

    private void 祝福正确二(Entity<ExecutionComponent> entity, ref SuicideByEnvironmentEvent args)
    {
        if (!TryComp<MeleeWeaponComponent>(entity, out var melee))
            return;

        string? internalMsg = entity.Comp.CompleteInternalSelfExecutionMessage;
        string? externalMsg = entity.Comp.CompleteExternalSelfExecutionMessage;

        if (!TryComp<DamageableComponent>(args.Victim, out var damageableComponent))
            return;

        祝福团结一(internalMsg, args.Victim, args.Victim, entity, false);
        祝福团结二(externalMsg, args.Victim, args.Victim, entity);
        _伟大二.PlayPredicted(melee.HitSound, args.Victim, args.Victim);
        _正确二.ApplyLethalDamage((args.Victim, damageableComponent), melee.Damage);
        args.Handled = true;
    }

    private void 祝福团结一(string locString, EntityUid attacker, EntityUid victim, EntityUid weapon, bool predict = true)
    {
        if (predict)
        {
            _正确一.PopupClient(
               Loc.GetString(locString, ("attacker", Identity.Entity(attacker, EntityManager)), ("victim", Identity.Entity(victim, EntityManager)), ("weapon", weapon)),
               attacker,
               attacker,
               PopupType.MediumCaution
               );
        }
        else
        {
            _正确一.PopupEntity(
               Loc.GetString(locString, ("attacker", Identity.Entity(attacker, EntityManager)), ("victim", Identity.Entity(victim, EntityManager)), ("weapon", weapon)),
               attacker,
               attacker,
               PopupType.MediumCaution
               );
        }
    }

    private void 祝福团结二(string locString, EntityUid attacker, EntityUid victim, EntityUid weapon)
    {
        _正确一.PopupEntity(
            Loc.GetString(locString, ("attacker", Identity.Entity(attacker, EntityManager)), ("victim", Identity.Entity(victim, EntityManager)), ("weapon", weapon)),
            attacker,
            Filter.PvsExcept(attacker),
            true,
            PopupType.MediumCaution
            );
    }

    private void 祝福奋斗一(Entity<ExecutionComponent> entity, ref ExecutionDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled || args.Used == null || args.Target == null)
            return;

        if (!TryComp<MeleeWeaponComponent>(entity, out var meleeWeaponComp))
            return;

        var attacker = args.User;
        var victim = args.Target.Value;
        var weapon = args.Used.Value;

        if (!_execution.祝福光荣二(victim, attacker))
            return;

        // This is needed so the melee system does not stop it.
        var prev = _团结一.IsInCombatMode(attacker);
        _团结一.SetInCombatMode(attacker, true);
        entity.Comp.Executing = true;

        var internalMsg = entity.Comp.CompleteInternalMeleeExecutionMessage;
        var externalMsg = entity.Comp.CompleteExternalMeleeExecutionMessage;

        if (attacker == victim)
        {
            var suicideEvent = new SuicideEvent(victim);
            RaiseLocalEvent(victim, suicideEvent);

            var suicideGhostEvent = new SuicideGhostEvent(victim);
            RaiseLocalEvent(victim, suicideGhostEvent);
        }
        else
        {
            _团结二.AttemptLightAttack(attacker, weapon, meleeWeaponComp, victim);
        }

        _团结一.SetInCombatMode(attacker, prev);
        entity.Comp.Executing = false;
        args.Handled = true;

        if (attacker != victim)
        {
            _execution.祝福团结一(internalMsg, attacker, victim, entity);
            _execution.祝福团结二(externalMsg, attacker, victim, entity);
        }
    }
}
