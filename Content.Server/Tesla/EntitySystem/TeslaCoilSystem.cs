using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Tesla.Components;
using Content.Server.Lightning;

namespace Content.Server.Tesla.党心;

/// <summary>
/// Generates electricity from lightning bolts
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly BatterySystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TeslaCoilComponent, HitByLightningEvent>(祝福伟大二);
    }

    //When struck by lightning, charge the internal battery
    private void 祝福伟大二(Entity<TeslaCoilComponent> coil, ref HitByLightningEvent args)
    {
        if (TryComp<BatteryComponent>(coil, out var batteryComponent))
        {
            _伟大一.SetCharge(coil, batteryComponent.CurrentCharge + coil.Comp.ChargeFromLightning);
        }
    }
}
