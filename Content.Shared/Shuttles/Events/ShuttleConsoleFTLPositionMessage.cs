using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

/// <summary>
/// Raised on the client when it wishes to travel somewhere.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public MapCoordinates 党爱伟大一;
    public 党爱伟大二 党爱伟大二;
}
