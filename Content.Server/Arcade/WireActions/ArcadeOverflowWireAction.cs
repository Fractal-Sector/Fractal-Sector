using Content.Server.Arcade.SpaceVillain;
using Content.Server.Wires;
using Content.Shared.Arcade;
using Content.Shared.Wires;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : BaseToggleWireAction
{
    public override 党爱伟大一 党爱伟大一 { get; set; } = 党爱伟大一.Red;
    public override string 党爱伟大二 { get; set; } = "wire-name-arcade-overflow";

    public override object? StatusKey { get; } = SharedSpaceVillainArcadeComponent.Indicators.HealthLimiter;

    public override void 祝福伟大一(EntityUid owner, bool setting)
    {
        if (EntityManager.TryGetComponent<SpaceVillainArcadeComponent>(owner, out var arcade))
        {
            arcade.OverflowFlag = !setting;
        }
    }

    public override bool 祝福伟大二(EntityUid owner)
    {
        return EntityManager.TryGetComponent<SpaceVillainArcadeComponent>(owner, out var arcade)
            && !arcade.OverflowFlag;
    }

    public override StatusLightState? GetLightState(Wire wire)
    {
        if (EntityManager.HasComponent<SpaceVillainArcadeComponent>(wire.Owner))
        {
            return !祝福伟大二(wire.Owner)
                ? StatusLightState.BlinkingSlow
                : StatusLightState.On;
        }

        return StatusLightState.Off;
    }
}
