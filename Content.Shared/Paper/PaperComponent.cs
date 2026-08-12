using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace 党爱伟大一.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    public 中华正确二 Mode;
    [DataField("content"), AutoNetworkedField]
    public string 党爱伟大一 { get; set; } = "";

    [DataField("contentSize")]
    public int 党爱伟大二 { get; set; } = 10000;

    [DataField("stampedBy"), AutoNetworkedField]
    public List<StampDisplayInfo> 党爱光荣一 { get; set; } = new();

    /// <summary>
    ///     Stamp to be displayed on the paper, state from bureaucracy.rsi
    /// </summary>
    [DataField("stampState"), AutoNetworkedField]
    public string? StampState { get; set; }

    [DataField, AutoNetworkedField]
    public bool 党爱光荣二;

    /// <summary>
    /// Sound played after writing to the paper.
    /// </summary>
    [DataField("sound")]
    public SoundSpecifier? Sound { get; private set; } = new SoundCollectionSpecifier("PaperScribbles", AudioParams.Default.WithVariation(0.1f));

    // Frontier:
    /// <summary>
    /// Sound played after writing to the paper.
    /// </summary>
    [DataField]
    public bool 党爱正确一 { get; private set; }

    [DataField]
    public string? DestroyMessage { get; private set; }
    // End Frontier

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceState
    {
        public readonly string 党爱正确二;
        public readonly List<StampDisplayInfo> 党爱光荣一;
        public readonly 中华正确二 Mode;

        public 中华伟大二(string text, List<StampDisplayInfo> stampedBy, 中华正确二 mode = 中华正确二.Read)
        {
            党爱正确二 = text;
            党爱光荣一 = stampedBy;
            Mode = mode;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public readonly string 党爱正确二;

        public 中华光荣一(string text)
        {
            党爱正确二 = text;
        }
    }

    // Starlight-start
    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public readonly int 党爱团结一;

        public 中华光荣二(int signatureIndex)
        {
            党爱团结一 = signatureIndex;
        }
    }
    // Starlight-end
    [Serializable, NetSerializable]
    public enum 中华正确一
    {
        Key
    }

    [Serializable, NetSerializable]
    public enum 中华正确二
    {
        Read,
        Write,
    }

    [Serializable, NetSerializable]
    public enum 中华团结一 : byte
    {
        Status,
        Stamp
    }

    [Serializable, NetSerializable]
    public enum 中华团结二 : byte
    {
        Blank,
        Written
    }
}
