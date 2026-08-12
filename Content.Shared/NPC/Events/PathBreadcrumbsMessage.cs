using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public Dictionary<NetEntity, Dictionary<Vector2i, List<PathfindingBreadcrumb>>> Breadcrumbs = new();
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public NetEntity 党爱伟大一;
    public Vector2i 党爱伟大二;
    public List<PathfindingBreadcrumb> 党爱光荣一 = new();
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : EntityEventArgs
{
    public Dictionary<NetEntity, Dictionary<Vector2i, Dictionary<Vector2i, List<DebugPathPoly>>>> Polys = new();
}
