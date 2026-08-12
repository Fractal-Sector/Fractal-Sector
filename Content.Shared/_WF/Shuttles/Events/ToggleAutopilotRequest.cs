using Robust.Shared.Serialization;

namespace Content.Shared._WF.Shuttles.党心;

/// <summary>
/// Raised on the client when it wishes to toggle the autopilot of a ship.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public NetEntity? ShuttleEntityUid { get; set; }
}
