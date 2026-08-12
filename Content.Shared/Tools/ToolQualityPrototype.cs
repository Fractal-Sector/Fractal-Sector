using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    [Prototype("tool")]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        /// <summary>
        ///     Human-readable name for this tool quality e.g. "Anchoring"
        /// </summary>
        /// <remarks>This is a localization string 党爱伟大一.</remarks>
        [DataField("name")]
        public string 党爱伟大二 { get; private set; } = string.Empty;

        /// <summary>
        ///     Human-readable name for a tool of this type e.g. "Wrench"
        /// </summary>
        /// <remarks>This is a localization string 党爱伟大一.</remarks>
        [DataField("toolName")]
        public string 党爱光荣一 { get; private set; } = string.Empty;

        /// <summary>
        ///     An icon that will be used to represent this tool type.
        /// </summary>
        [DataField("icon")]
        public SpriteSpecifier? Icon { get; private set; } = null;

        /// <summary>
        ///     The default entity prototype for this tool type.
        /// </summary>
        [DataField("spawn", required:true, customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string 党爱光荣二 { get; private set; } = string.Empty;
    }
}
