using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

/// <summary>
/// Raised on a client IFF console when it wishes to show IFF.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public bool 党爱伟大一;
}
