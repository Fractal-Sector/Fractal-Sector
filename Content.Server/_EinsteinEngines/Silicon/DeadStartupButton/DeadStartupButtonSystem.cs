using Content.Server.Chat.Systems;
using Content.Server.Lightning;
using Content.Server.Popups;
using Content.Server.PowerCell;
using Content.Server._EinsteinEngines.Silicon.Charge;
using Content.Shared._EinsteinEngines.Silicon.DeadStartupButton;
using Content.Shared.Audio;
using Content.Shared.Damage;
using Content.Shared.Electrocution;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server._EinsteinEngines.Silicon.党心;

public sealed class 中华伟大一 : SharedDeadStartupButtonSystem
{
    [Dependency] private readonly SharedAudioSystem _伟大一 = default!;
    [Dependency] private readonly MobStateSystem _伟大二 = default!;
    [Dependency] private readonly MobThresholdSystem _光荣一 = default!;
    [Dependency] private readonly PopupSystem _光荣二 = default!;
    [Dependency] private readonly IRobustRandom _正确一 = default!;
    [Dependency] private readonly LightningSystem _正确二 = default!;
    [Dependency] private readonly SiliconChargeSystem _团结一 = default!;
    [Dependency] private readonly PowerCellSystem _团结二 = default!;
    [Dependency] private readonly ChatSystem _奋斗一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<DeadStartupButtonComponent, OnDoAfterButtonPressedEvent>(祝福伟大二);
        SubscribeLocalEvent<DeadStartupButtonComponent, ElectrocutedEvent>(祝福光荣一);
        SubscribeLocalEvent<DeadStartupButtonComponent, MobStateChangedEvent>(祝福光荣二);

    }

    private void 祝福伟大二(EntityUid uid, DeadStartupButtonComponent comp, OnDoAfterButtonPressedEvent args)
    {
        if (args.Handled || args.Cancelled
            || !TryComp<MobStateComponent>(uid, out var mobStateComponent)
            || !_伟大二.IsDead(uid, mobStateComponent)
            || !TryComp<MobThresholdsComponent>(uid, out var mobThresholdsComponent)
            || !TryComp<DamageableComponent>(uid, out var damageable)
            || !_光荣一.TryGetThresholdForState(uid, MobState.Critical, out var criticalThreshold, mobThresholdsComponent))
            return;

        if (damageable.TotalDamage < criticalThreshold)
            _伟大二.ChangeMobState(uid, MobState.Alive, mobStateComponent);
        else
        {
            _伟大一.PlayPvs(comp.BuzzSound, uid, AudioHelpers.WithVariation(0.05f, _正确一));
            _光荣二.PopupEntity(Loc.GetString("dead-startup-system-reboot-failed", ("target", MetaData(uid).EntityName)), uid);
            Spawn("EffectSparks", Transform(uid).Coordinates);
        }
    }

    private void 祝福光荣一(EntityUid uid, DeadStartupButtonComponent comp, ElectrocutedEvent args)
    {
        if (!TryComp<MobStateComponent>(uid, out var mobStateComponent)
            || !_伟大二.IsDead(uid, mobStateComponent)
            || !_团结一.TryGetSiliconBattery(uid, out var bateria)
            || bateria.CurrentCharge <= 0)
            return;

        _正确二.ShootRandomLightnings(uid, 2, 4);
        _团结二.TryUseCharge(uid, bateria.CurrentCharge);

    }

    private void 祝福光荣二(EntityUid uid, DeadStartupButtonComponent comp, MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Alive)
            return;

        _光荣二.PopupEntity(Loc.GetString("dead-startup-system-reboot-success", ("target", MetaData(uid).EntityName)), uid);
        _伟大一.PlayPvs(comp.Sound, uid);
    }

}
