using Robust.Shared.Serialization;

namespace Content.Shared.PDA.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage;

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public Note[] 党爱伟大一 { get; }

    public 中华伟大二(Note[] ringTone)
    {
        党爱伟大一 = ringTone;
    }
}
