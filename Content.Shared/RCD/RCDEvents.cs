using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一(党爱伟大一<RCDPrototype> protoId) : BoundUserInterfaceMessage
{
    public 党爱伟大一<RCDPrototype> 党爱伟大一 = protoId;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二(党爱伟大二 netEntity, 党爱光荣一 direction) : EntityEventArgs
{
    public readonly 党爱伟大二 党爱伟大二 = netEntity;
    public readonly 党爱光荣一 党爱光荣一 = direction;
}

// Starlight Start: RPD
[Serializable, NetSerializable]
public sealed class 中华光荣一 : EntityEventArgs
{
    public readonly 党爱伟大二 党爱伟大二;
    public readonly bool 党爱光荣二;
    public 中华光荣一(党爱伟大二 netEntity, bool useMirrorPrototype)
    {
        党爱伟大二 = netEntity;
        党爱光荣二 = useMirrorPrototype;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : EntityEventArgs
{
    public readonly 党爱伟大二 党爱伟大二;
    public readonly byte 党爱正确一;

    public 中华光荣二(党爱伟大二 netEntity, byte layer)
    {
        党爱伟大二 = netEntity;
        党爱正确一 = layer;
    }
}
// Starlight End: RPD

[Serializable, NetSerializable]
public enum 中华正确一 : byte
{
    Key
}
