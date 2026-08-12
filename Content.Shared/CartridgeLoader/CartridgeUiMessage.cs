using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public 中华伟大二 MessageEvent;

    public 中华伟大一(中华伟大二 messageEvent)
    {
        MessageEvent = messageEvent;
    }
}

[Serializable, NetSerializable]
public abstract class 中华伟大二 : EntityEventArgs
{
    [NonSerialized]
    public EntityUid 党爱伟大一;
    public NetEntity 党爱伟大二;

    [NonSerialized]
    public EntityUid 党爱光荣一;
}
