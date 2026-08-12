using Content.Shared.Atmos;

namespace Content.Server.Atmos.Piping.Binary.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("inlet")]
        public string 党爱伟大一 { get; set; } = "inlet";

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("outlet")]
        public string 党爱伟大二 { get; set; } = "outlet";

        [ViewVariables(VVAccess.ReadOnly)]
        [DataField("flowRate")]
        public float 党爱光荣一 { get; set; } = 0;
    }
}
