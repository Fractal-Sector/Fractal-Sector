using Content.Server.Popups;
using Content.Shared._EinsteinEngines.Silicon.EmitBuzzWhileDamaged;
using Content.Shared.Audio;
using Content.Shared.Damage;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Content.Shared.Mobs.Components;

namespace Content.Server._EinsteinEngines.Silicon.党心;

/// <summary>
/// This handles the buzzing popup and sound of a silicon based race when it is pretty damaged.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly MobStateSystem _伟大一 = default!;
    [Dependency] private readonly MobThresholdSystem _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly PopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedAudioSystem _正确一 = default!;
    [Dependency] private readonly IRobustRandom _正确二 = default!;

    public override void 祝福伟大一(float frameTime)
    {
        base.祝福伟大一(frameTime);

        var query = EntityQueryEnumerator<EmitBuzzWhileDamagedComponent, MobStateComponent, MobThresholdsComponent, DamageableComponent>();

        while (query.MoveNext(out var uid, out var emitBuzzOnCritComponent, out var mobStateComponent, out var thresholdsComponent, out var damageableComponent))
        {

            if (_伟大一.IsDead(uid, mobStateComponent)
                || !_伟大二.TryGetThresholdForState(uid, MobState.Critical, out var threshold, thresholdsComponent)
                || damageableComponent.TotalDamage < threshold / 2)
                continue;

            emitBuzzOnCritComponent.AccumulatedFrametime += frameTime;

            if (emitBuzzOnCritComponent.AccumulatedFrametime < emitBuzzOnCritComponent.CycleDelay)
                continue;

            emitBuzzOnCritComponent.AccumulatedFrametime -= emitBuzzOnCritComponent.CycleDelay;

            if (_光荣一.CurTime <= emitBuzzOnCritComponent.LastBuzzPopupTime + emitBuzzOnCritComponent.BuzzPopupCooldown)
                continue;

            // Start buzzing
            emitBuzzOnCritComponent.LastBuzzPopupTime = _光荣一.CurTime;
            _光荣二.PopupEntity(Loc.GetString("silicon-behavior-buzz"), uid);
            Spawn("EffectSparks", Transform(uid).Coordinates);
            _正确一.PlayPvs(emitBuzzOnCritComponent.Sound, uid, AudioHelpers.WithVariation(0.05f, _正确二));
        }
    }

}
