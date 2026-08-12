using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

/// <summary>
/// Raised on the client when it wishes to undock all docking ports at once.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public List<NetEntity> 党爱伟大一;
    
    public 中华伟大一(List<NetEntity> dockEntities)
    {
        党爱伟大一 = dockEntities;
    }
} 