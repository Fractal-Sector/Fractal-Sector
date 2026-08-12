using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

/// <summary>
/// Raised on the client when it's viewing a particular docking port to try and dock it.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public NetEntity 党爱伟大一;

    public NetEntity 党爱伟大二;
}
