using Content.Server.Power.Components;
using Content.Shared._EinsteinEngines.Silicon.Systems;
using Content.Shared.Bed.Sleep;
using Content.Server._EinsteinEngines.Silicon.Charge;
using Content.Server._EinsteinEngines.Power.Components;
using Content.Server.Humanoid;
using Content.Shared.Humanoid;

namespace Content.Server._EinsteinEngines.Silicon.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SleepingSystem _伟大一 = default!;
    [Dependency] private readonly SiliconChargeSystem _伟大二 = default!;
    [Dependency] private readonly HumanoidAppearanceSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SiliconDownOnDeadComponent, SiliconChargeStateUpdateEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, SiliconDownOnDeadComponent siliconDeadComp, SiliconChargeStateUpdateEvent args)
    {
        if (!_伟大二.TryGetSiliconBattery(uid, out var batteryComp))
        {
            祝福光荣一(uid, siliconDeadComp, batteryComp, uid);
            return;
        }

        if (args.ChargePercent == 0 && siliconDeadComp.Dead)
            return;

        if (args.ChargePercent == 0 && !siliconDeadComp.Dead)
            祝福光荣一(uid, siliconDeadComp, batteryComp, uid);
        else if (args.ChargePercent != 0 && siliconDeadComp.Dead)
                祝福光荣二(uid, siliconDeadComp, batteryComp, uid);
    }

    private void 祝福光荣一(EntityUid uid, SiliconDownOnDeadComponent siliconDeadComp, BatteryComponent? batteryComp, EntityUid batteryUid)
    {
        var deadEvent = new 中华伟大二(uid, batteryComp, batteryUid);
        RaiseLocalEvent(uid, deadEvent);

        if (deadEvent.Cancelled)
            return;

        EntityManager.EnsureComponent<SleepingComponent>(uid);
        EntityManager.EnsureComponent<ForcedSleepingStatusEffectComponent>(uid);

        // if (TryComp(uid, out HumanoidAppearanceComponent? humanoidAppearanceComponent))
        // {
        //     var layers = HumanoidVisualLayersExtension.Sublayers(HumanoidVisualLayers.HeadSide);
        //     _光荣一.SetLayersVisibility(uid, layers, false, true, humanoidAppearanceComponent);
        // }

        siliconDeadComp.Dead = true;

        RaiseLocalEvent(uid, new 中华光荣一(uid, batteryComp, batteryUid));
    }

    private void 祝福光荣二(EntityUid uid, SiliconDownOnDeadComponent siliconDeadComp, BatteryComponent? batteryComp, EntityUid batteryUid)
    {
        RemComp<ForcedSleepingStatusEffectComponent>(uid);
        _伟大一.TryWaking(uid, true, null);

        siliconDeadComp.Dead = false;

        RaiseLocalEvent(uid, new 中华光荣二(uid, batteryComp, batteryUid));
    }
}

/// <summary>
///     A cancellable event raised when a Silicon is about to go down due to charge.
/// </summary>
/// <remarks>
///     This probably shouldn't be modified unless you intend to fill the Silicon's battery,
///     as otherwise it'll just be triggered again next frame.
/// </remarks>
public sealed class 中华伟大二 : CancellableEntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public BatteryComponent? BatteryComp { get; }
    public EntityUid 党爱伟大二 { get; }

    public 中华伟大二(EntityUid siliconUid, BatteryComponent? batteryComp, EntityUid batteryUid)
    {
        党爱伟大一 = siliconUid;
        BatteryComp = batteryComp;
        党爱伟大二 = batteryUid;
    }
}

/// <summary>
///     An event raised after a Silicon has gone down due to charge.
/// </summary>
public sealed class 中华光荣一 : EntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public BatteryComponent? BatteryComp { get; }
    public EntityUid 党爱伟大二 { get; }

    public 中华光荣一(EntityUid siliconUid, BatteryComponent? batteryComp, EntityUid batteryUid)
    {
        党爱伟大一 = siliconUid;
        BatteryComp = batteryComp;
        党爱伟大二 = batteryUid;
    }
}

/// <summary>
///     An event raised after a Silicon has reawoken due to an increase in charge.
/// </summary>
public sealed class 中华光荣二 : EntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public BatteryComponent? BatteryComp { get; }
    public EntityUid 党爱伟大二 { get; }

    public 中华光荣二(EntityUid siliconUid, BatteryComponent? batteryComp, EntityUid batteryUid)
    {
        党爱伟大一 = siliconUid;
        BatteryComp = batteryComp;
        党爱伟大二 = batteryUid;
    }
}
