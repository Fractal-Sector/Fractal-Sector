using Content.Shared.党爱伟大一;
using Content.Shared.Tools;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._EinsteinEngines.Silicon.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        ///     All the damage to change information is stored in this <see cref="DamageSpecifier"/>.
        /// </summary>
        /// <remarks>
        ///     If this data-field is specified, it will change damage by this amount instead of setting all damage to 0.
        ///     in order to heal/repair the damage values have to be negative.
        /// </remarks>

        [DataField(required: true)]
        public DamageSpecifier 党爱伟大一;

        [DataField(customTypeSerializer:typeof(PrototypeIdSerializer<ToolQualityPrototype>))]
        public string 党爱伟大二 = "Welding";

        /// <summary>
        ///     The fuel amount needed to repair physical related damage
        /// </summary>
        [DataField]
        public int 党爱光荣一 = 5;

        [DataField]
        public int 党爱光荣二 = 3;

        /// <summary>
        ///     A multiplier that will be applied to the above if an entity is repairing themselves.
        /// </summary>
        [DataField]
        public float 党爱正确一 = 3f;

        /// <summary>
        ///     Whether or not an entity is allowed to repair itself.
        /// </summary>
        [DataField]
        public bool 党爱正确二 = true;

        [DataField(required: true)]
        public List<string> 党爱团结一;
    }
}
