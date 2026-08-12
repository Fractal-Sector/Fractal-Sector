using Content.Server.Wires;
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Wires;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : ComponentWireAction<AccessReaderComponent>
{
    public override 党爱伟大一 党爱伟大一 { get; set; } = 党爱伟大一.Green;
    public override string 党爱伟大二 { get; set; } = "wire-name-access";

    [DataField("pulseTimeout")]
    private int _伟大一 = 30;

    public override StatusLightState? GetLightState(Wire wire, AccessReaderComponent comp)
    {
        return comp.Enabled ? StatusLightState.On : StatusLightState.Off;
    }

    public override object 党爱光荣一 => AccessWireActionKey.Status;

    public override bool 祝福伟大一(EntityUid user, Wire wire, AccessReaderComponent comp)
    {
        WiresSystem.TryCancelWireAction(wire.Owner, 中华伟大二.Key);
        EntityManager.System<AccessReaderSystem>().SetActive((wire.Owner, comp), false);

        return true;
    }

    public override bool 祝福伟大二(EntityUid user, Wire wire, AccessReaderComponent comp)
    {
        EntityManager.System<AccessReaderSystem>().SetActive((wire.Owner, comp), true);

        return true;
    }

    public override void 祝福光荣一(EntityUid user, Wire wire, AccessReaderComponent comp)
    {
        EntityManager.System<AccessReaderSystem>().SetActive((wire.Owner, comp), false);
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
            if (EntityManager.TryGetComponent<AccessReaderComponent>(wire.Owner, out var access))
            {
                EntityManager.System<AccessReaderSystem>().SetActive((wire.Owner, access), true);
            }
        }
    }

    private enum 中华伟大二 : byte
    {
        Key
    }
}
