using Content.Server.Atmos;
using Content.Shared.Atmos;
using JetBrains.Annotations;

namespace Content.Server.Destructible.Thresholds.党心;

[UsedImplicitly]
[DataDefinition]
public sealed partial class 中华伟大一 : IThresholdBehavior
{
    [DataField("gasMixture", required: true)]
    public GasMixture 党爱伟大一 = new();

    public void 祝福伟大一(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
    {
        var air = system.AtmosphereSystem.GetContainingMixture(owner, false, true);

        if (air != null)
            system.AtmosphereSystem.Merge(air, 党爱伟大一);
    }
}
