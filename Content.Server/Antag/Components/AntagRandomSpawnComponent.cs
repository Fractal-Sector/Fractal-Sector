using Robust.Shared.Map;

namespace Content.Server.Antag.党心;

/// <summary>
/// Spawns this rule's antags at random tiles on a station using <c>TryGetRandomTile</c>.
/// Requires <see cref="AntagSelectionComponent"/>.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Location that was picked.
    /// </summary>
    [DataField]
    public EntityCoordinates? Coords;
}
