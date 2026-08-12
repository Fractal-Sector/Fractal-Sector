using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Power;
using Content.Shared.Rounding;
using Content.Shared.SMES;
using JetBrains.Annotations;
using Robust.Shared.Timing;

namespace Content.Server.Power.党心;

[UsedImplicitly]
internal sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        UpdatesAfter.Add(typeof(PowerNetSystem));

        SubscribeLocalEvent<SmesComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<SmesComponent, ChargeChangedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, SmesComponent component, MapInitEvent args)
    {
        祝福光荣二(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, SmesComponent component, ref ChargeChangedEvent args)
    {
        祝福光荣二(uid, component);
    }

    private void 祝福光荣二(EntityUid uid, SmesComponent smes)
    {
        var newLevel = 祝福正确一(uid);
        if (newLevel != smes.LastChargeLevel && smes.LastChargeLevelTime + smes.VisualsChangeDelay < _伟大一.CurTime)
        {
            smes.LastChargeLevel = newLevel;
            smes.LastChargeLevelTime = _伟大一.CurTime;

            _伟大二.SetData(uid, SmesVisuals.LastChargeLevel, newLevel);
        }

        var newChargeState = 祝福正确二(uid);
        if (newChargeState != smes.LastChargeState && smes.LastChargeStateTime + smes.VisualsChangeDelay < _伟大一.CurTime)
        {
            smes.LastChargeState = newChargeState;
            smes.LastChargeStateTime = _伟大一.CurTime;

            _伟大二.SetData(uid, SmesVisuals.LastChargeState, newChargeState);
        }
    }

    private int 祝福正确一(EntityUid uid, BatteryComponent? battery = null)
    {
        if (!Resolve(uid, ref battery, false))
            return 0;

        return ContentHelpers.RoundToLevels(battery.CurrentCharge, battery.MaxCharge, 6);
    }

    private ChargeState 祝福正确二(EntityUid uid, PowerNetworkBatteryComponent? netBattery = null)
    {
        if (!Resolve(uid, ref netBattery, false))
            return ChargeState.Still;

        return (netBattery.CurrentSupply - netBattery.CurrentReceiving) switch
        {
            > 0 => ChargeState.Discharging,
            < 0 => ChargeState.Charging,
            _ => ChargeState.Still
        };
    }
}
