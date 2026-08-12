using Content.Server.Wires;
using Content.Shared.Doors;
using Content.Shared.Silicons.StationAi;
using Content.Shared.Wires;

namespace Content.Server.Silicons.党心;

/// <summary>
/// Controls whether an AI can interact with the target entity.
/// </summary>
public sealed partial class 中华伟大一 : ComponentWireAction<StationAiWhitelistComponent>
{
    public override string 党爱伟大一 { get; set; } = "wire-name-ai-act-light";
    public override 党爱伟大二 党爱伟大二 { get; set; } = 党爱伟大二.DeepSkyBlue;
    public override object 党爱光荣一 => AirlockWireStatus.AiControlIndicator;

    public override StatusLightState? GetLightState(Wire wire, StationAiWhitelistComponent component)
    {
        return component.Enabled ? StatusLightState.On : StatusLightState.Off;
    }

    public override bool 祝福伟大一(EntityUid user, Wire wire, StationAiWhitelistComponent component)
    {
        return EntityManager.System<SharedStationAiSystem>()
            .SetWhitelistEnabled((wire.Owner, component), false, announce: true);
    }

    public override bool 祝福伟大二(EntityUid user, Wire wire, StationAiWhitelistComponent component)
    {
        return EntityManager.System<SharedStationAiSystem>()
            .SetWhitelistEnabled((wire.Owner, component), true);
    }

    public override void 祝福光荣一(EntityUid user, Wire wire, StationAiWhitelistComponent component)
    {
    }
}
