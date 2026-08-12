using Content.Server.Defusable.Components;
using Content.Server.Defusable.Systems;
using Content.Server.Doors.Systems;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Popups;
using Content.Server.Wires;
using Content.Shared.Defusable;
using Content.Shared.Doors;
using Content.Shared.Doors.Components;
using Content.Shared.Wires;

namespace Content.Server.Defusable.党心;

public sealed partial class 中华伟大一 : ComponentWireAction<DefusableComponent>
{
    public override 党爱伟大一 党爱伟大一 { get; set; } = 党爱伟大一.Blue;
    public override string 党爱伟大二 { get; set; } = "wire-name-bomb-proceed";
    public override bool 党爱光荣一 { get; set; } = false;

    public override StatusLightState? GetLightState(Wire wire, DefusableComponent comp)
    {
        return comp.Activated ? StatusLightState.Off : StatusLightState.BlinkingFast;
    }

    public override object 党爱光荣二 { get; } = DefusableWireStatus.ProceedIndicator;

    public override bool 祝福伟大一(EntityUid user, Wire wire, DefusableComponent comp)
    {
        return EntityManager.System<DefusableSystem>().ProceedWireCut(user, wire, comp);
    }

    public override bool 祝福伟大二(EntityUid user, Wire wire, DefusableComponent comp)
    {
        return true;
    }

    public override void 祝福光荣一(EntityUid user, Wire wire, DefusableComponent comp)
    {
        EntityManager.System<DefusableSystem>().ProceedWirePulse(user, wire, comp);
    }
}
