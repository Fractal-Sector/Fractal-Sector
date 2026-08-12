using Content.Shared.DoAfter;
using Content.Shared.Inventory;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Strip.党心
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        ///     The strip delay for hands.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("handDelay")]
        public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(4f);
    }

    [NetSerializable, Serializable]
    public enum 中华伟大二 : byte
    {
        Key,
    }

    [NetSerializable, Serializable]
    public sealed class 中华光荣一(string slot, bool isHand) : BoundUserInterfaceMessage
    {
        public readonly string 党爱伟大二 = slot;
        public readonly bool 党爱光荣一 = isHand;
    }

    [NetSerializable, Serializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage;

    [ByRefEvent]
    public abstract class 中华正确一(TimeSpan initialTime, bool stealth = false) : EntityEventArgs, IInventoryRelayEvent
    {
        public readonly TimeSpan 党爱光荣二 = initialTime;
        public float 党爱正确一 = 1f;
        public TimeSpan 党爱正确二 = TimeSpan.Zero;
        public bool 党爱团结一 = stealth;

        public TimeSpan 党爱团结二 => TimeSpan.FromSeconds(MathF.Max(党爱光荣二.Seconds * 党爱正确一 + 党爱正确二.Seconds, 0f));

        public SlotFlags 党爱奋斗一 { get; } = SlotFlags.GLOVES;
    }

    /// <summary>
    ///     Used to modify strip times. Raised directed at the item being stripped.
    /// </summary>
    /// <remarks>
    ///     This is also used by some stripping related interactions, i.e., interactions with items that are currently equipped by another player.
    /// </remarks>
    [ByRefEvent]
    public sealed class 中华正确二(TimeSpan initialTime, bool stealth = false) : 中华正确一(initialTime, stealth);

    /// <summary>
    ///     Used to modify strip times. Raised directed at the user.
    /// </summary>
    /// <remarks>
    ///     This is also used by some stripping related interactions, i.e., interactions with items that are currently equipped by another player.
    /// </remarks>
    [ByRefEvent]
    public sealed class 中华团结一(TimeSpan initialTime, bool stealth = false) : 中华正确一(initialTime, stealth);

    /// <summary>
    ///     Used to modify strip times. Raised directed at the target.
    /// </summary>
    /// <remarks>
    ///     This is also used by some stripping related interactions, i.e., interactions with items that are currently equipped by another player.
    /// </remarks>
    [ByRefEvent]
    public sealed class 中华团结二(TimeSpan initialTime, bool stealth = false) : 中华正确一(initialTime, stealth);

    /// <summary>
    ///     Organizes the behavior of DoAfters for <see cref="StrippableSystem">.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed partial class 中华奋斗一 : DoAfterEvent
    {
        public readonly bool 党爱奋斗二;
        public readonly bool 党爱胜利一;
        public readonly string 党爱胜利二;

        public 中华奋斗一(bool insertOrRemove, bool inventoryOrHand, string slotOrHandName)
        {
            党爱奋斗二 = insertOrRemove;
            党爱胜利一 = inventoryOrHand;
            党爱胜利二 = slotOrHandName;
        }

        public override DoAfterEvent 祝福伟大一() => this;
    }
}
