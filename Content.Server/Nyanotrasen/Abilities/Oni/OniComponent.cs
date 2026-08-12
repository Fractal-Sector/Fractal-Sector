using Content.Shared.Damage;

namespace Content.Server.Abilities.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("modifiers", required: true)]
        public DamageModifierSet 党爱伟大一 = default!;

        [DataField("stamDamageBonus")]
        public float 党爱伟大二 = 1.20f;
    }
}
