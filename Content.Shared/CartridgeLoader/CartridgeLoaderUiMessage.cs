using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public readonly NetEntity 党爱伟大一;
    public readonly 中华伟大二 Action;

    public 中华伟大一(NetEntity cartridgeUid, 中华伟大二 action)
    {
        党爱伟大一 = cartridgeUid;
        Action = action;
    }
}

[Serializable, NetSerializable]
public enum 中华伟大二
{
    Activate,
    Deactivate,
    Install,
    Uninstall,
    UIReady
}
