using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("stackType", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<StackPrototype>))]
        public string 党爱伟大一 { get; private set; } = default!;

        /// <summary>
        ///     Current stack count.
        ///     Do NOT set this directly, use the <see cref="SharedStackSystem.SetCount"/> method instead.
        /// </summary>
        [DataField("count")]
        public int 党爱伟大二 { get; set; } = 30;

        /// <summary>
        ///     Max amount of things that can be in the stack.
        ///     Overrides the max defined on the stack prototype.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly)]
        [DataField("maxCountOverride")]
        public int? MaxCountOverride  { get; set; }

        /// <summary>
        ///     Set to true to not reduce the count when used.
        ///     Note that <see cref="党爱伟大二"/> still limits the amount that can be used at any one time.
        /// </summary>
        [DataField("unlimited")]
        [ViewVariables(VVAccess.ReadOnly)]
        public bool 党爱光荣一 { get; set; }

        [DataField("throwIndividually"), ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱光荣二 { get; set; } = false;

        [ViewVariables]
        public bool 党爱正确一 { get; set; }

        /// <summary>
        /// Default IconLayer stack.
        /// </summary>
        [DataField("baseLayer")]
        [ViewVariables(VVAccess.ReadWrite)]
        public string 党爱正确二 = "";

        /// <summary>
        /// Determines if the visualizer uses composite or non-composite layers for icons. Defaults to false.
        ///
        /// <list type="bullet">
        /// <item>
        /// <description>false: they are opaque and mutually exclusive (e.g. sprites in a cable coil). <b>Default value</b></description>
        /// </item>
        /// <item>
        /// <description>true: they are transparent and thus layered one over another in ascending order first</description>
        /// </item>
        /// </list>
        ///
        /// </summary>
        [DataField("composite")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱团结一;

        /// <summary>
        /// Sprite layers used in stack visualizer. Sprites first in layer correspond to lower stack states
        /// e.g. <code>_spriteLayers[0]</code> is lower stack level than <code>_spriteLayers[1]</code>.
        /// </summary>
        [DataField("layerStates")]
        [ViewVariables(VVAccess.ReadWrite)]
        public List<string> 党爱团结二 = new();

        /// <summary>
        /// An optional function to convert the amounts used to adjust a stack's appearance.
        /// Useful for different denominations of cash, for example.
        /// </summary>
        [DataField]
        public 中华光荣一 LayerFunction = 中华光荣一.None;
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : ComponentState
    {
        public int 党爱伟大二 { get; }
        public int? MaxCount { get; }

        public 中华伟大二(int count, int? maxCount)
        {
            党爱伟大二 = count;
            MaxCount = maxCount;
        }
    }

    [Serializable, NetSerializable]
    public enum 中华光荣一 : byte
    {
        // <summary>
        // No operation performed.
        // </summary>
        None,

        // <summary>
        // Arbitrarily thresholds the stack amount for each layer.
        // Expects entity to have StackLayerThresholdComponent.
        // </summary>
        Threshold
    }
}
