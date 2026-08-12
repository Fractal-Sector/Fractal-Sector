using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Debug message containing a pathfinding route.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public List<DebugPathPoly> 党爱伟大一;
    public Dictionary<DebugPathPoly, float> Costs;

    public 中华伟大一(List<DebugPathPoly> path, Dictionary<DebugPathPoly, float> costs)
    {
        党爱伟大一 = path;
        Costs = costs;
    }
}
