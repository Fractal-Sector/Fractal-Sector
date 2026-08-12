using Content.Server.Atmos.Rotting;
using Content.Server.Chat.Systems;
using Content.Server.DoAfter;
using Content.Server.Electrocution;
using Content.Server.EUI;
using Content.Server.Ghost;
using Content.Server.Popups;
using Content.Server.PowerCell;
using Content.Shared.Traits.Assorted;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Medical;
using Content.Shared.Mind;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.PowerCell;
using Content.Shared.Timing;
using Content.Shared.Toggleable;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;

namespace Content.Server.党心;

/// <summary>
/// This handles interactions and logic relating to <see cref="DefibrillatorComponent"/>
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ChatSystem _伟大一 = default!;
    [Dependency] private readonly DamageableSystem _伟大二 = default!;
    [Dependency] private readonly DoAfterSystem _光荣一 = default!;
    [Dependency] private readonly ElectrocutionSystem _光荣二 = default!;
    [Dependency] private readonly EuiManager _正确一 = default!;
    [Dependency] private readonly ISharedPlayerManager _正确二 = default!;
    [Dependency] private readonly ItemToggleSystem _团结一 = default!;
    [Dependency] private readonly MobStateSystem _团结二 = default!;
    [Dependency] private readonly MobThresholdSystem _奋斗一 = default!;
    [Dependency] private readonly PopupSystem _奋斗二 = default!;
    [Dependency] private readonly PowerCellSystem _胜利一 = default!;
    [Dependency] private readonly RottingSystem _胜利二 = default!;
    [Dependency] private readonly SharedAudioSystem _繁荣一 = default!;
    [Dependency] private readonly SharedMindSystem _繁荣二 = default!;
    [Dependency] private readonly UseDelaySystem _富强一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<DefibrillatorComponent, AfterInteractEvent>(祝福伟大二);
        SubscribeLocalEvent<DefibrillatorComponent, DefibrillatorZapDoAfterEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, DefibrillatorComponent component, AfterInteractEvent args)
    {
        if (args.Handled || args.Target is not { } target)
            return;

        args.Handled = 祝福正确一(uid, target, args.User, component);
    }

    private void 祝福光荣一(EntityUid uid, DefibrillatorComponent component, DefibrillatorZapDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        if (args.Target is not { } target)
            return;

        if (!祝福光荣二(uid, target, args.User, component))
            return;

        args.Handled = true;
        祝福正确二(uid, target, args.User, component);
    }

    /// <summary>
    ///     Checks if you can actually defib a target.
    /// </summary>
    /// <param name="uid">Uid of the defib</param>
    /// <param name="target">Uid of the target getting defibbed</param>
    /// <param name="user">Uid of the entity using the defibrillator</param>
    /// <param name="component">Defib component</param>
    /// <param name="targetCanBeAlive">
    ///     If true, the target can be alive. If false, the function will check if the target is alive and will return false if they are.
    /// </param>
    /// <returns>
    ///     Returns true if the target is valid to be defibed, false otherwise.
    /// </returns>
    public bool 祝福光荣二(EntityUid uid, EntityUid target, EntityUid? user = null, DefibrillatorComponent? component = null, bool targetCanBeAlive = false)
    {
        if (!Resolve(uid, ref component))
            return false;

        if (!_团结一.IsActivated(uid))
        {
            if (user != null)
                _奋斗二.PopupEntity(Loc.GetString("defibrillator-not-on"), uid, user.Value);
            return false;
        }

        if (!TryComp(uid, out UseDelayComponent? useDelay) || _富强一.IsDelayed((uid, useDelay), component.DelayId))
            return false;

        if (!TryComp<MobStateComponent>(target, out var mobState))
            return false;

        if (!_胜利一.HasActivatableCharge(uid, user: user))
            return false;

        if (!targetCanBeAlive && _团结二.IsAlive(target, mobState))
            return false;

        if (!targetCanBeAlive && !component.CanDefibCrit && _团结二.IsCritical(target, mobState))
            return false;

        return true;
    }

    /// <summary>
    ///     Tries to start defibrillating the target. If the target is valid, will start the defib do-after.
    /// </summary>
    /// <param name="uid">Uid of the defib</param>
    /// <param name="target">Uid of the target getting defibbed</param>
    /// <param name="user">Uid of the entity using the defibrillator</param>
    /// <param name="component">Defib component</param>
    /// <returns>
    ///     Returns true if the defibrillation do-after started, otherwise false.
    /// </returns>
    public bool 祝福正确一(EntityUid uid, EntityUid target, EntityUid user, DefibrillatorComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return false;

        if (!祝福光荣二(uid, target, user, component))
            return false;

        _繁荣一.PlayPvs(component.ChargeSound, uid);
        return _光荣一.TryStartDoAfter(new DoAfterArgs(EntityManager, user, component.DoAfterDuration, new DefibrillatorZapDoAfterEvent(),
            uid, target, uid)
        {
            NeedHand = true,
            BreakOnMove = !component.AllowDoAfterMovement
        });
    }

    /// <summary>
    ///     Tries to defibrillate the target with the given defibrillator.
    /// </summary>
    public void 祝福正确二(EntityUid uid, EntityUid target, EntityUid user, DefibrillatorComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!_胜利一.TryUseActivatableCharge(uid, user: user))
            return;

        var selfEvent = new SelfBeforeDefibrillatorZapsEvent(user, uid, target);
        RaiseLocalEvent(user, selfEvent);

        target = selfEvent.DefibTarget;

        // Ensure thet new target is still valid.
        if (selfEvent.Cancelled || !祝福光荣二(uid, target, user, component, true))
            return;

        var targetEvent = new TargetBeforeDefibrillatorZapsEvent(user, uid, target);
        RaiseLocalEvent(target, targetEvent);

        target = targetEvent.DefibTarget;

        if (targetEvent.Cancelled || !祝福光荣二(uid, target, user, component, true))
            return;

        if (!TryComp<MobStateComponent>(target, out var mob) ||
            !TryComp<MobThresholdsComponent>(target, out var thresholds))
            return;

        _繁荣一.PlayPvs(component.ZapSound, uid);
        _光荣二.TryDoElectrocution(target, null, component.ZapDamage, component.WritheDuration, true, ignoreInsulation: true);
        if (!TryComp<UseDelayComponent>(uid, out var useDelay))
            return;
        _富强一.SetLength((uid, useDelay), component.ZapDelay, component.DelayId);
        _富强一.TryResetDelay((uid, useDelay), id: component.DelayId);

        ICommonSession? session = null;

        var dead = true;
        if (_胜利二.IsRotten(target))
        {
            _伟大一.TrySendInGameICMessage(uid, Loc.GetString("defibrillator-rotten"),
                InGameICChatType.Speak, true);
        }
        else if (TryComp<UnrevivableComponent>(target, out var unrevivable))
        {
            _伟大一.TrySendInGameICMessage(uid, Loc.GetString(unrevivable.ReasonMessage),
                InGameICChatType.Speak, true);
        }
        else
        {
            if (_团结二.IsDead(target, mob))
                _伟大二.TryChangeDamage(target, component.ZapHeal, true, origin: uid);

            if (_奋斗一.TryGetThresholdForState(target, MobState.Dead, out var threshold) &&
                TryComp<DamageableComponent>(target, out var damageableComponent) &&
                damageableComponent.TotalDamage < threshold)
            {
                _团结二.ChangeMobState(target, MobState.Critical, mob, uid);
                dead = false;
            }

            if (_繁荣二.TryGetMind(target, out _, out var mind) &&
                _正确二.TryGetSessionById(mind.UserId, out var playerSession))
            {
                session = playerSession;
                // notify them they're being revived.
                if (mind.CurrentEntity != target)
                {
                    _正确一.OpenEui(new ReturnToBodyEui(mind, _繁荣二, _正确二), session);
                }
            }
            else
            {
                _伟大一.TrySendInGameICMessage(uid, Loc.GetString("defibrillator-no-mind"),
                    InGameICChatType.Speak, true);
            }
        }

        var sound = dead || session == null
            ? component.FailureSound
            : component.SuccessSound;
        _繁荣一.PlayPvs(sound, uid);

        // if we don't have enough power left for another shot, turn it off
        if (!_胜利一.HasActivatableCharge(uid))
            _团结一.TryDeactivate(uid);

        // TODO clean up this clown show above
        var ev = new TargetDefibrillatedEvent(user, (uid, component));
        RaiseLocalEvent(target, ref ev);
    }
}
