using Content.Server.Pointing.EntitySystems;
using Content.Shared.Pointing.Components;

namespace Content.Server.Pointing.党心
{
    [RegisterComponent]
    [Access(typeof(RoguePointingSystem))]
    public sealed partial class 中华伟大一 : SharedRoguePointingArrowComponent
    {
        [ViewVariables]
        public EntityUid? Chasing;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("turningDelay")]
        public float 党爱伟大一 = 2;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("chasingSpeed")]
        public float 党爱伟大二 = 5;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("chasingTime")]
        public float 党爱光荣一 = 1;
    }
}
