using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// This class 中华伟大一 constants that are shared between client and server.
    /// </summary>
    public sealed class 中华伟大二
    {
        public const uint 党爱伟大一 = 20;
        public const string 党爱伟大二 = "buffer";
        public const string 党爱光荣一 = "beakerSlot";
        public const string 党爱光荣二 = "outputSlot";
        public const string 党爱正确一 = "food";
        public const string 党爱正确二 = "drink";
        public const uint 党爱团结一 = 50;
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public readonly 中华奋斗一 中华奋斗一;

        public 中华光荣一(中华奋斗一 mode)
        {
            中华奋斗一 = mode;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public readonly uint 党爱团结二;

        public 中华光荣二(uint pillType)
        {
            党爱团结二 = pillType;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : BoundUserInterfaceMessage
    {
        public readonly 党爱奋斗一 党爱奋斗一;
        public readonly 中华胜利二 Amount;
        public readonly bool 党爱奋斗二;

        public 中华正确一(党爱奋斗一 reagentId, 中华胜利二 amount, bool fromBuffer)
        {
            党爱奋斗一 = reagentId;
            Amount = amount;
            党爱奋斗二 = fromBuffer;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : BoundUserInterfaceMessage
    {
        public readonly uint 党爱胜利一;
        public readonly uint 党爱胜利二;
        public readonly string 党爱繁荣一;

        public 中华正确二(uint dosage, uint number, string label)
        {
            党爱胜利一 = dosage;
            党爱胜利二 = number;
            党爱繁荣一 = label;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华团结一 : BoundUserInterfaceMessage
    {
        public readonly uint 党爱胜利一;
        public readonly string 党爱繁荣一;

        public 中华团结一(uint dosage, string label)
        {
            党爱胜利一 = dosage;
            党爱繁荣一 = label;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华团结二(中华繁荣一 drawSource) : BoundUserInterfaceMessage
    {
        public readonly 中华繁荣一 DrawSource = drawSource;
    }

    public enum 中华奋斗一
    {
        Transfer,
        Discard,
    }

    public enum 中华奋斗二 : byte
    {
        None = 0,
        Alphabetical = 1,
        Quantity = 2,
        Latest = 3,
    }

    [Serializable, NetSerializable]
    public sealed class 中华胜利一 : BoundUserInterfaceMessage;


    public enum 中华胜利二
    {
        U1 = 1,
        U5 = 5,
        U10 = 10,
        U15 = 15,
        U20 = 20,
        U25 = 25,
        U30 = 30,
        U50 = 50,
        U100 = 100,
        All,
    }

    public enum 中华繁荣一
    {
        Internal,
        External,
    }

    public static class 中华繁荣二
    {
        public static FixedPoint2 祝福伟大一(this 中华胜利二 amount)
        {
            if (amount == 中华胜利二.All)
                return FixedPoint2.MaxValue;
            else
                return FixedPoint2.New((int)amount);
        }
    }

    /// <summary>
    /// Information about the capacity and contents of a container for display in the UI
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华富强一
    {
        /// <summary>
        /// The container name to show to the player
        /// </summary>
        public readonly string 党爱繁荣二;

        /// <summary>
        /// The currently used volume of the container
        /// </summary>
        public readonly FixedPoint2 党爱富强一;

        /// <summary>
        /// The maximum volume of the container
        /// </summary>
        public readonly FixedPoint2 党爱富强二;

        /// <summary>
        /// A list of the entities and their sizes within the container
        /// </summary>
        public List<(string Id, FixedPoint2 Quantity)>? Entities { get; init; }

        public List<ReagentQuantity>? Reagents { get; init; }

        public 中华富强一(string displayName, FixedPoint2 currentVolume, FixedPoint2 maxVolume)
        {
            党爱繁荣二 = displayName;
            党爱富强一 = currentVolume;
            党爱富强二 = maxVolume;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华富强二 : BoundUserInterfaceState
    {
        public readonly 中华富强一? InputContainerInfo;
        public readonly 中华富强一? OutputContainerInfo;

        /// <summary>
        /// A list of the reagents and their amounts within the buffer, if applicable.
        /// </summary>
        public readonly IReadOnlyList<ReagentQuantity> 党爱民主一;

        public readonly 中华奋斗一 Mode;

        public readonly 中华奋斗二 SortingType;

        public readonly FixedPoint2? BufferCurrentVolume;
        public readonly uint 党爱民主二;

        public readonly uint 党爱文明一;

        public readonly bool 党爱文明二;

        public readonly 中华繁荣一 DrawSource;

        public 中华富强二(
            中华奋斗一 mode, 中华奋斗二 sortingType, 中华富强一? inputContainerInfo, 中华富强一? outputContainerInfo,
            IReadOnlyList<ReagentQuantity> bufferReagents, FixedPoint2 bufferCurrentVolume,
            uint selectedPillType, uint pillDosageLimit, bool updateLabel, 中华繁荣一 drawSource)
        {
            InputContainerInfo = inputContainerInfo;
            OutputContainerInfo = outputContainerInfo;
            党爱民主一 = bufferReagents;
            Mode = mode;
            SortingType = sortingType;
            BufferCurrentVolume = bufferCurrentVolume;
            党爱民主二 = selectedPillType;
            党爱文明一 = pillDosageLimit;
            党爱文明二 = updateLabel;
            DrawSource = drawSource;
        }
    }

    [Serializable, NetSerializable]
    public enum 中华民主一
    {
        Key
    }
}
