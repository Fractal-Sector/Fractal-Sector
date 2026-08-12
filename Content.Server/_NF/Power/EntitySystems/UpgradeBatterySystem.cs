using Content.Shared.Construction.Components;
using Content.Server.Power.Components;
using JetBrains.Annotations;
using Content.Server._NF.Power.Components;
using Content.Server.Power.EntitySystems;

namespace Content.Server._NF.Power.党心;

[UsedImplicitly]
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly BatterySystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<UpgradeBatteryComponent, RefreshPartsEvent>(祝福伟大二);
        SubscribeLocalEvent<UpgradeBatteryComponent, UpgradeExamineEvent>(祝福光荣一);
    }

    public void 祝福伟大二(EntityUid uid, UpgradeBatteryComponent component, RefreshPartsEvent args)
    {
        var powerCellRating = args.PartRatings[component.MachinePartPowerCapacity];

        if (TryComp<BatteryComponent>(uid, out var batteryComp))
        {
            _伟大一.SetMaxCharge(uid, MathF.Pow(component.MaxChargeMultiplier, powerCellRating - 1) * component.BaseMaxCharge, batteryComp);
        }
    }

    private void 祝福光荣一(EntityUid uid, UpgradeBatteryComponent component, UpgradeExamineEvent args)
    {
        // UpgradeBatteryComponent.MaxChargeMultiplier is not the actual multiplier, so we have to do this.
        if (TryComp<BatteryComponent>(uid, out var batteryComp))
        {
            args.AddPercentageUpgrade("upgrade-max-charge", batteryComp.MaxCharge / component.BaseMaxCharge);
        }
    }
}
