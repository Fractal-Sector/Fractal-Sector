using Robust.Shared.Serialization;
using Content.Shared.DoAfter;

namespace Content.Shared._EE.Flight.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : SimpleDoAfterEvent;

[Serializable, NetSerializable]
public sealed class 中华伟大二(NetEntity uid, bool isFlying, bool isAnimated) : EntityEventArgs
{
    public NetEntity 党爱伟大一 { get; } = uid;
    public bool 党爱伟大二 { get; } = isFlying;
    public bool 党爱光荣一 { get; } = isAnimated;
}