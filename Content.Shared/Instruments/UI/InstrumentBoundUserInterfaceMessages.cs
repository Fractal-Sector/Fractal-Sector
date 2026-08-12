using Robust.Shared.Serialization;

namespace Content.Shared.Instruments.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public (NetEntity, string)[] Nearby { get; set; }

    public 中华伟大二((NetEntity, string)[] nearby)
    {
        Nearby = nearby;
    }
}
