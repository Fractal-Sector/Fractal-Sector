using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage;

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public float 党爱伟大一;
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceState
{
    public float 党爱伟大二;
}
