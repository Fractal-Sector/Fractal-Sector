using Content.Server.党爱伟大二.Systems;
using Content.Shared.党爱伟大二;

namespace Content.Server.党爱伟大二.党心
{
    [Access(typeof(DamageOtherOnHitSystem))]
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("ignoreResistances")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大一 = false;

        [DataField("damage", required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier 党爱伟大二 = default!;

    }
}
