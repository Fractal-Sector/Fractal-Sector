using Content.Server.Electrocution;
using Content.Server.Popups;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Electrocution;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Content.Server._EinsteinEngines.Power.Components;

namespace Content.Server._EinsteinEngines.Power.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly PopupSystem _伟大二 = default!;
    [Dependency] private readonly BatterySystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BatteryComponent, ElectrocutedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, BatteryComponent battery, ElectrocutedEvent args)
    {
        if (args.ShockDamage == null || args.ShockDamage <= 0)
            return;

        var charge = Math.Min(args.ShockDamage.Value * args.SiemensCoefficient
            / ElectrocutionSystem.ElectrifiedDamagePerWatt * 2,
                battery.MaxCharge * 0.25f)
            * _伟大一.NextFloat(0.75f, 1.25f);
            
        _光荣一.SetCharge(uid, battery.CurrentCharge + charge);

        _伟大二.PopupEntity(Loc.GetString("battery-electrocute-charge"), uid, uid);
    }
}
