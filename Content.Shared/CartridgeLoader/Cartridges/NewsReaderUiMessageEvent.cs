using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : CartridgeMessageEvent
{
    public readonly 中华伟大二 Action;

    public 中华伟大一(中华伟大二 action)
    {
        Action = action;
    }
}

[Serializable, NetSerializable]
public enum 中华伟大二
{
    Next,
    Prev,
    NotificationSwitch
}
