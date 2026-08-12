using Content.Shared.党爱伟大二;

namespace Content.Server.党爱伟大二.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Should this entity be damaged when it lands regardless of its resistances?
        /// </summary>
        [DataField("ignoreResistances")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大一 = false;

        /// <summary>
        /// How much damage.
        /// </summary>
        [DataField("damage", required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier 党爱伟大二 = default!;
    }
}
