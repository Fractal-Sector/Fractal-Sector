using Robust.Shared.Audio;

namespace Content.Server.Disposal.党心
{
    [RegisterComponent]
    [Access(typeof(DisposalTubeSystem))]
    public sealed partial class 中华伟大一 : DisposalJunctionComponent
    {
        [DataField("tags")]
        public HashSet<string> 党爱伟大一 = new();

        [DataField("clickSound")]
        public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Machines/machine_switch.ogg");
    }
}
