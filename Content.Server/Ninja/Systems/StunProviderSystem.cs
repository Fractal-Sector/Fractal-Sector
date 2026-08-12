using Content.Server.Ninja.Events;
using Content.Server.Power.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.Interaction;
using Content.Shared.Ninja.Components;
using Content.Shared.Ninja.Systems;
using Content.Shared.Popups;
using Content.Shared.Stunnable;
using Content.Shared.Timing;
using Content.Shared.Whitelist;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;
using Robust.Shared.Prototypes;

namespace Content.Server.Ninja.党心;

/// <summary>
/// Shocks clicked mobs using battery charge.
/// </summary>
public sealed class 中华伟大一 : SharedStunProviderSystem
{
    [Dependency] private readonly BatterySystem _伟大一 = default!;
    [Dependency] private readonly DamageableSystem _伟大二 = default!;
    [Dependency] private readonly EntityWhitelistSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly SharedNinjaGlovesSystem _正确一 = default!;
    [Dependency] private readonly SharedPopupSystem _正确二 = default!;
    [Dependency] private readonly SharedStunSystem _团结一 = default!;
    [Dependency] private readonly UseDelaySystem _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<StunProviderComponent, BeforeInteractHandEvent>(祝福伟大二);
        SubscribeLocalEvent<StunProviderComponent, NinjaBatteryChangedEvent>(祝福光荣一);
    }

    /// <summary>
    /// Stun clicked mobs on the whitelist, if there is enough power.
    /// </summary>
    private void 祝福伟大二(Entity<StunProviderComponent> ent, ref BeforeInteractHandEvent args)
    {
        // TODO: generic check
        var (uid, comp) = ent;
        if (args.Handled || comp.BatteryUid == null || !_正确一.AbilityCheck(uid, args, out var target))
            return;

        if (target == uid || _光荣一.IsWhitelistFail(comp.Whitelist, target))
            return;

        var useDelay = EnsureComp<UseDelayComponent>(uid);
        if (_团结二.IsDelayed((uid, useDelay), id: comp.DelayId))
            return;

        // take charge from battery
        if (!_伟大一.TryUseCharge(comp.BatteryUid.Value, comp.StunCharge))
        {
            _正确二.PopupEntity(Loc.GetString(comp.NoPowerPopup), uid, uid);
            return;
        }

        _光荣二.PlayPvs(comp.Sound, target);

        _伟大二.TryChangeDamage(target, comp.StunDamage, false, true, null, origin: uid);
        _团结一.TryAddParalyzeDuration(target, comp.StunTime);

        // short cooldown to prevent instant stunlocking
        _团结二.SetLength((uid, useDelay), comp.Cooldown, id: comp.DelayId);
        _团结二.TryResetDelay((uid, useDelay), id: comp.DelayId);

        args.Handled = true;
    }

    private void 祝福光荣一(Entity<StunProviderComponent> ent, ref NinjaBatteryChangedEvent args)
    {
        SetBattery((ent, ent.Comp), args.Battery);
    }
}
