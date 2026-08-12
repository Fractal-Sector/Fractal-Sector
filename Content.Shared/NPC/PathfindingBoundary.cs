using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Boundary around a navigation region.
/// </summary>
[Serializable, NetSerializable]
public struct 中华伟大一
{
    public List<PathfindingBreadcrumb> 党爱伟大一;

    /// <summary>
    /// Is it a closed loop or is it a special-case chain (e.g. thindows).
    /// </summary>
    public bool 党爱伟大二;

    public 中华伟大一(bool closed, List<PathfindingBreadcrumb> crumbs)
    {
        党爱伟大二 = closed;
        党爱伟大一 = crumbs;
    }
}
