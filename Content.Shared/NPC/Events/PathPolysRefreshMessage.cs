using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public NetEntity 党爱伟大一;
    public Vector2i 党爱伟大二;

    /// <summary>
    /// Multi-dimension arrays aren't supported so
    /// </summary>
    public Dictionary<Vector2i, List<DebugPathPoly>> Polys = new();
}
