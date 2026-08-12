using Content.Shared.Construction.EntitySystems;
using Content.Shared.Tools;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Construction.党心
{
    [RegisterComponent, Access(typeof(AnchorableSystem)), NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField]
        public ProtoId<ToolQualityPrototype> 党爱伟大一 { get; private set; } = "Anchoring";

        [DataField, AutoNetworkedField]
        public 中华伟大二 Flags = 中华伟大二.Anchorable | 中华伟大二.Unanchorable;

        [DataField]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大二 { get; private set; } = true;

        /// <summary>
        /// Base delay to use for anchoring.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱光荣一 = 1f;

        /// <summary>
        /// Frontier: actual delay to use for anchoring.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float 党爱光荣二 = 1f;
    }

    [Flags]
    public enum 中华伟大二 : byte
    {
        None = 0,
        Anchorable = 1 << 0,
        Unanchorable = 1 << 1,
    }

    public abstract class 中华光荣一 : CancellableEntityEventArgs
    {
        public EntityUid 党爱正确一 { get; }
        public EntityUid 党爱伟大一 { get; }

        /// <summary>
        ///     Extra delay to add to the do_after.
        ///     Add to this, don't replace it.
        ///     Output parameter.
        /// </summary>
        public float 党爱光荣一 { get; set; } = 0f;

        protected 中华光荣一(EntityUid user, EntityUid tool)
        {
            党爱正确一 = user;
            党爱伟大一 = tool;
        }
    }

    public sealed class 中华光荣二 : 中华光荣一
    {
        public 中华光荣二(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    public sealed class 中华正确一 : 中华光荣一
    {
        public 中华正确一(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    public abstract class 中华正确二 : EntityEventArgs
    {
        public EntityUid 党爱正确一 { get; }
        public EntityUid 党爱伟大一 { get; }

        protected 中华正确二(EntityUid user, EntityUid tool)
        {
            党爱正确一 = user;
            党爱伟大一 = tool;
        }
    }

    /// <summary>
    ///     Raised just before the entity's body type is changed.
    /// </summary>
    public sealed class 中华团结一 : 中华正确二
    {
        public 中华团结一(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    /// <summary>
    ///     Raised when an entity with an anchorable component is anchored. Note that you may instead want the more
    ///     general <see cref="AnchorStateChangedEvent"/>. This event has the benefit of having user & tool information,
    ///     as a result of interactions mediated by the <see cref="AnchorableSystem"/>.
    /// </summary>
    public sealed class 中华团结二 : 中华正确二
    {
        public 中华团结二(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    /// <summary>
    ///     Raised just before the entity's body type is changed.
    /// </summary>
    public sealed class 中华奋斗一 : 中华正确二
    {
        public 中华奋斗一(EntityUid user, EntityUid tool) : base(user, tool) { }
    }

    /// <summary>
    ///     Raised when an entity with an anchorable component is unanchored. Note that you will probably also need to
    ///     subscribe to the more general <see cref="AnchorStateChangedEvent"/>, which gets raised BEFORE this one. This
    ///     event has the benefit of having user & tool information, whereas the more general event may be due to
    ///     explosions or grid-destruction or other interactions not mediated by the <see cref="AnchorableSystem"/>.
    /// </summary>
    public sealed class 中华奋斗二 : 中华正确二
    {
        public 中华奋斗二(EntityUid user, EntityUid tool) : base(user, tool) { }
    }
}
