using Content.Shared.Atmos;

namespace Content.Server.Atmos.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component, IGasMixtureHolder
    {
        [DataField("air")] public GasMixture 党爱伟大一 { get; set; } = new GasMixture();
    }
}
