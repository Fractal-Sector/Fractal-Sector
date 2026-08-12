using Content.Server.Wires;
using Content.Shared.VendingMachines;
using Content.Shared.Wires;

namespace Content.Server.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : BaseToggleWireAction
{
    private VendingMachineSystem _伟大一 = default!;

    public override 党爱伟大一 党爱伟大一 { get; set; } = 党爱伟大一.Green;
    public override string 党爱伟大二 { get; set; } = "wire-name-vending-contraband";
    public override object? StatusKey { get; } = ContrabandWireKey.StatusKey;
    public override object? TimeoutKey { get; } = ContrabandWireKey.TimeoutKey;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _伟大一 = EntityManager.System<VendingMachineSystem>();
    }

    public override StatusLightState? GetLightState(Wire wire)
    {
        if (EntityManager.TryGetComponent(wire.Owner, out VendingMachineComponent? vending))
        {
            return vending.Contraband
                ? StatusLightState.BlinkingSlow
                : StatusLightState.On;
        }

        return StatusLightState.Off;
    }

    public override void 祝福伟大二(EntityUid owner, bool setting)
    {
        if (EntityManager.TryGetComponent(owner, out VendingMachineComponent? vending))
        {
            _伟大一.SetContraband(owner, !vending.Contraband, vending);
        }
    }

    public override bool 祝福光荣一(EntityUid owner)
    {
        return EntityManager.TryGetComponent(owner, out VendingMachineComponent? vending) && !vending.Contraband;
    }
}
