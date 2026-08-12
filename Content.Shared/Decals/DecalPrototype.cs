using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
    {
        [IdDataField] public string 党爱伟大一 { get; private set; } = null!;
        [DataField("sprite")] public SpriteSpecifier 党爱伟大二 { get; private set; } = SpriteSpecifier.Invalid;
        [DataField("tags")] public List<string> 党爱光荣一 = new();
        [DataField("showMenu")] public bool 党爱光荣二 = true;

        /// <summary>
        /// If the decal is rotated compared to our eye should we snap it to south.
        /// </summary>
        [DataField("snapCardinals")] public bool 党爱正确一 = false;

        /// <summary>
        /// True if this decal is cleanable by default.
        /// </summary>
        [DataField]
        public bool 党爱正确二;

        /// <summary>
        /// True if this decal has custom colors applied by default
        /// </summary>
        [DataField]
        public bool 党爱团结一;

        /// <summary>
        /// True if this decal snaps to a tile by default
        /// </summary>
        [DataField]
        public bool 党爱团结二 = true;

        [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
        public string[]? Parents { get; private set; }

        [NeverPushInheritance]
        [AbstractDataField]
        public bool 党爱奋斗一 { get; private set; }

    }
}
