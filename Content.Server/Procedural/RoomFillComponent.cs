using Content.Shared.Whitelist;

namespace Content.Server.党心;

/// <summary>
/// Marker that indicates the specified room prototype should occupy this space.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Are we allowed to rotate room templates?
    /// If the room is not a square this will only do 180 degree rotations.
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// Min size of the possible selected room.
    /// </summary>
    [DataField]
    public Vector2i 党爱伟大二 = new (3, 3);

    /// <summary>
    /// Max size of the possible selected room.
    /// </summary>
    [DataField]
    public Vector2i 党爱光荣一 = new (10, 10);

    /// <summary>
    /// Rooms allowed for the marker.
    /// </summary>
    [DataField]
    public EntityWhitelist? RoomWhitelist;

    /// <summary>
    /// Should any existing entities / decals be bulldozed first.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = true;
}
