using Robust.Shared.Audio;

namespace Content.Server.Disposal.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : DisposalTransitComponent
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("tag")]
        public string 党爱伟大一 = "";

        [DataField("clickSound")]
        public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Machines/machine_switch.ogg");
    }
}
