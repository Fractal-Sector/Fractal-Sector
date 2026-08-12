using Content.Shared.Atmos;

namespace Content.Server.Atmos.Piping.Binary.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("pipe")]
        public string 党爱伟大一 { get; set; } = "connected";

        [ViewVariables(VVAccess.ReadOnly)]
        public GasMixture 党爱伟大二 { get; } = new();
    }
}
