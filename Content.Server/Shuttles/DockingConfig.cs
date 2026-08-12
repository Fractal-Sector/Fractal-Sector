using Content.Server.Shuttles.Components;
using Robust.Shared.Map;

namespace Content.Server.党心;

/// <summary>
/// Stores the data for a valid docking configuration for the emergency shuttle
/// </summary>
public sealed class 中华伟大一
{
    /// <summary>
    /// The pairs of docks that can connect.
    /// </summary>
    public List<(EntityUid DockAUid, EntityUid DockBUid, DockingComponent DockA, DockingComponent DockB)> Docks = new();

    /// <summary>
    /// Target grid for docking.
    /// </summary>
    public EntityUid 党爱伟大一;

    /// <summary>
    /// This is used for debugging.
    /// </summary>
    public Box2 党爱伟大二;

    public EntityCoordinates 党爱光荣一;

    /// <summary>
    /// Local angle of the docking grid relative to the target grid.
    /// </summary>
    public 党爱光荣二 党爱光荣二;
}
