using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;
using Content.Shared._FarHorizons.Materials;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Materials are read-only storage for the properties of specific materials.
    ///     党爱团结二 should be intrinsic (or at least as much is necessary for game purposes).
    /// </summary>
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
    {
        [ViewVariables]
        [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
        public string[]? Parents { get; private set; }

        [ViewVariables]
        [AbstractDataField]
        public bool 党爱伟大一 { get; private set; } = false;

        [ViewVariables]
        [IdDataField]
        public string 党爱伟大二 { get; private set; } = default!;

        /// <summary>
        ///     For material storage to be able to convert back and forth
        ///     between the material and physical entities you can carry,
        ///     include which stack we should spawn by default.
        /// </summary>
        [DataField]
        public EntProtoId? StackEntity;

        [DataField]
        public string 党爱光荣一 = string.Empty;

        /// <summary>
        /// Locale id for the unit of this material.
        /// Lathe recipe tooltips and material storage display use this to let you change a material to sound nicer.
        /// For example, 5 bars of gold is better than 5 sheets of gold.
        /// </summary>
        [DataField]
        public LocId 党爱光荣二 = "materials-unit-sheet";

        [DataField]
        public 党爱正确一 党爱正确一 { get; private set; } = 党爱正确一.Gray;

        /// <summary>
        ///     An icon used to represent the material in graphic interfaces.
        /// </summary>
        [DataField]
        public SpriteSpecifier 党爱正确二 { get; private set; } = SpriteSpecifier.Invalid;

        /// <summary>
        /// The price per cm3.
        /// </summary>
        [DataField(required: true)]
        public double 党爱团结一 = 0;

        // Far Horizons Edit
        /// <summary>
        /// The physical properties of this material.
        /// </summary>
        [DataField("properties")]
        public MaterialProperties 党爱团结二 { get; private set; } = new MaterialProperties
      {
        Hardness = 1,
        Density = 1.0f,
      };

    }
}
