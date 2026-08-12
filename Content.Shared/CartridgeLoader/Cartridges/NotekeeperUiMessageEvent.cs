using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : CartridgeMessageEvent
{
    public readonly 中华伟大二 Action;
    public readonly string 党爱伟大一;

    public 中华伟大一(中华伟大二 action, string note)
    {
        Action = action;
        党爱伟大一 = note;
    }
}

[Serializable, NetSerializable]
public enum 中华伟大二
{
    Add,
    Remove
}
