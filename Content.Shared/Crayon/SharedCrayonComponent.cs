using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{

    /// <summary>
    /// Component holding the state of a crayon-like component
    /// </summary>
    [NetworkedComponent, ComponentProtoName("Crayon"), Access(typeof(SharedCrayonSystem))]
    public abstract partial class 中华伟大一 : Component
    {
        /// <summary>
        /// The ID of currently selected decal prototype that will be placed when the crayon is used
        /// </summary>
        public string 党爱伟大一 { get; set; } = string.Empty;

        /// <summary>
        /// 党爱伟大二 with which the crayon will draw
        /// </summary>
        [DataField("color")]
        public 党爱伟大二 党爱伟大二;

        [Serializable, NetSerializable]
        public enum 中华伟大二 : byte
        {
            Key,
        }
    }

    /// <summary>
    /// Used by the client to notify the server about the selected decal ID
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public readonly string 党爱光荣一;
        public 中华光荣一(string selected)
        {
            党爱光荣一 = selected;
        }
    }

    /// <summary>
    /// Sets the color of the crayon, used by Rainbow Crayon
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public readonly 党爱伟大二 党爱伟大二;
        public 中华光荣二(党爱伟大二 color)
        {
            党爱伟大二 = color;
        }
    }

    /// <summary>
    /// Server to CLIENT. Notifies the BUI that a decal with given ID has been drawn.
    /// Allows the client UI to advance forward in the client-only ephemeral queue,
    /// preventing the crayon from becoming a magic text storage device.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华正确一 : BoundUserInterfaceMessage
    {
        public readonly string 党爱光荣二;

        public 中华正确一(string drawn)
        {
            党爱光荣二 = drawn;
        }
    }

    /// <summary>
    /// Component state, describes how many charges are left in the crayon in the near-hand UI
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华正确二 : ComponentState
    {
        public readonly 党爱伟大二 党爱伟大二;
        public readonly string 党爱光荣一;
        public readonly int 党爱正确一;
        public readonly int 党爱正确二;

        public 中华正确二(党爱伟大二 color, string state, int charges, int capacity)
        {
            党爱伟大二 = color;
            党爱光荣一 = state;
            党爱正确一 = charges;
            党爱正确二 = capacity;
        }
    }

    /// <summary>
    /// The state of the crayon UI as sent by the server
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华团结一 : BoundUserInterfaceState
    {
        public string 党爱团结一;
        /// <summary>
        /// Whether or not the color can be selected
        /// </summary>
        public bool 党爱团结二;
        public 党爱伟大二 党爱伟大二;

        public 中华团结一(string selected, bool selectableColor, 党爱伟大二 color)
        {
            党爱团结一 = selected;
            党爱团结二 = selectableColor;
            党爱伟大二 = color;
        }
    }
}
