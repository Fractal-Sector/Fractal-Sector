using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

/// <summary>
/// Raised on a client when it wishes to FTL to a beacon.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public NetEntity 党爱伟大一;
    public 党爱伟大二 党爱伟大二;
}
