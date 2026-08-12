using Content.Server.Wires;
using Content.Shared.Doors;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Content.Shared.Wires;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : ComponentWireAction<AirlockComponent>
{
    public override 党爱伟大一 党爱伟大一 { get; set; } = 党爱伟大一.Red;
    public override string 党爱伟大二 { get; set; } = "wire-name-door-safety";


    [DataField("timeout")]
    private int _伟大一 = 30;

    public override StatusLightState? GetLightState(Wire wire, AirlockComponent comp)
        => comp.Safety ? StatusLightState.On : StatusLightState.Off;

    public override object 党爱光荣一 { get; } = AirlockWireStatus.SafetyIndicator;

    public override bool 祝福伟大一(EntityUid user, Wire wire, AirlockComponent door)
    {
        WiresSystem.TryCancelWireAction(wire.Owner, 中华伟大二.Key);
        EntityManager.System<SharedAirlockSystem>().SetSafety(door, false);
        return true;
    }

    public override bool 祝福伟大二(EntityUid user, Wire wire, AirlockComponent door)
    {
        EntityManager.System<SharedAirlockSystem>().SetSafety(door, true);
        return true;
    }

    public override void 祝福光荣一(EntityUid user, Wire wire, AirlockComponent door)
    {
        EntityManager.System<SharedAirlockSystem>().SetSafety(door, false);
        WiresSystem.StartWireAction(wire.Owner, _伟大一, 中华伟大二.Key, new TimedWireEvent(祝福正确一, wire));
    }

    public override void 祝福光荣二(Wire wire)
    {
        if (!IsPowered(wire.Owner))
        {
            WiresSystem.TryCancelWireAction(wire.Owner, 中华伟大二.Key);
        }
    }

    private void 祝福正确一(Wire wire)
    {
        if (!wire.IsCut)
        {
            if (EntityManager.TryGetComponent<AirlockComponent>(wire.Owner, out var door))
            {
                EntityManager.System<SharedAirlockSystem>().SetSafety(door, true);
            }
        }
    }

    private enum 中华伟大二 : byte
    {
        Key
    }
}
