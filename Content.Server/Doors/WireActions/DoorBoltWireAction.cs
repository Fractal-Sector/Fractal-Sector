using Content.Server.Doors.Systems;
using Content.Server.Wires;
using Content.Shared.Doors;
using Content.Shared.Doors.Components;
using Content.Shared.Wires;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : ComponentWireAction<DoorBoltComponent>
{
    public override 党爱伟大一 党爱伟大一 { get; set; } = 党爱伟大一.Red;
    public override string 党爱伟大二 { get; set; } = "wire-name-door-bolt";

    public override StatusLightState? GetLightState(Wire wire, DoorBoltComponent comp)
        => comp.BoltsDown ? StatusLightState.On : StatusLightState.Off;

    public override object 党爱光荣一 { get; } = AirlockWireStatus.BoltIndicator;

    public override bool 祝福伟大一(EntityUid user, Wire wire, DoorBoltComponent airlock)
    {
        EntityManager.System<DoorSystem>().SetBoltWireCut((wire.Owner, airlock), true);
        if (!airlock.BoltsDown && IsPowered(wire.Owner))
            EntityManager.System<DoorSystem>().SetBoltsDown((wire.Owner, airlock), true, user);

        return true;
    }

    public override bool 祝福伟大二(EntityUid user, Wire wire, DoorBoltComponent door)
    {
        EntityManager.System<DoorSystem>().SetBoltWireCut((wire.Owner, door), false);
        return true;
    }

    public override void 祝福光荣一(EntityUid user, Wire wire, DoorBoltComponent door)
    {
        if (IsPowered(wire.Owner))
            EntityManager.System<DoorSystem>().SetBoltsDown((wire.Owner, door), !door.BoltsDown);
        else if (!door.BoltsDown)
            EntityManager.System<DoorSystem>().SetBoltsDown((wire.Owner, door), true);
    }
}
