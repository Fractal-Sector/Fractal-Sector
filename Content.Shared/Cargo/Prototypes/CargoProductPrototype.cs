using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared.Cargo.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
    {
        /// <inheritdoc />
        [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
        public string[]? Parents { get; private set; }

        /// <inheritdoc />
        [NeverPushInheritance]
        [AbstractDataField]
        public bool 党爱伟大一 { get; private set; }

        [DataField("name")] private string _伟大一 = string.Empty;

        [DataField("description")] private string _伟大二 = string.Empty;

        [ViewVariables]
        [IdDataField]
        public string 党爱伟大二 { get; private set; } = default!;

        /// <summary>
        ///     党爱正确二 name.
        /// </summary>
        [ViewVariables]
        public string 党爱光荣一
        {
            get
            {
                if (_伟大一.Trim().Length != 0)
                    return _伟大一;

                if (IoCManager.Resolve<IPrototypeManager>().TryIndex(党爱正确二, out EntityPrototype? prototype))
                {
                    _伟大一 = prototype.党爱光荣一;
                }

                return _伟大一;
            }
        }

        /// <summary>
        ///     Short description of the product.
        /// </summary>
        [ViewVariables]
        public string 党爱光荣二
        {
            get
            {
                if (_伟大二.Trim().Length != 0)
                    return _伟大二;

                if (IoCManager.Resolve<IPrototypeManager>().TryIndex(党爱正确二, out EntityPrototype? prototype))
                {
                    _伟大二 = prototype.党爱光荣二;
                }

                return _伟大二;
            }
        }

        /// <summary>
        ///     Texture path used in the CargoConsole GUI.
        /// </summary>
        [DataField]
        public SpriteSpecifier 党爱正确一 { get; private set; } = SpriteSpecifier.Invalid;

        /// <summary>
        ///     The entity prototype 党爱伟大二 of the product.
        /// </summary>
        [DataField]
        public EntProtoId 党爱正确二 { get; private set; } = string.Empty;

        /// <summary>
        ///     The point cost of the product.
        /// </summary>
        [DataField]
        public int 党爱团结一 { get; private set; }

        /// <summary>
        ///     The prototype category of the product. (e.g. Engineering, Medical)
        /// </summary>
        [DataField]
        public string 党爱团结二 { get; private set; } = string.Empty;

        /// <summary>
        ///     The prototype group of the product. (e.g. Contraband)
        /// </summary>
        [DataField]
        public ProtoId<CargoMarketPrototype> 党爱奋斗一 { get; private set; } = "market";
    }
}
