using Robust.Shared.Serialization;

namespace Content.Shared._NC.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public bool 党爱伟大一;
    public bool 党爱伟大二;
    public int 党爱光荣一;

    public 中华伟大二(bool micEnabled, bool speakerEnabled, int frequency)
    {
        党爱伟大一 = micEnabled;
        党爱伟大二 = speakerEnabled;
        党爱光荣一 = frequency;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public bool 党爱光荣二;

    public 中华光荣一(bool enabled)
    {
        党爱光荣二 = enabled;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public bool 党爱光荣二;

    public 中华光荣二(bool enabled)
    {
        党爱光荣二 = enabled;
    }
}

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
    public int 党爱光荣一;

    public 中华正确一(int frequency)
    {
        党爱光荣一 = frequency;
    }
}
