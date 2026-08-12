using Robust.Shared.Serialization;
using static Content.Shared.Decals.DecalGridComponent;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EntityEventArgs
    {
        public Dictionary<NetEntity, Dictionary<Vector2i, DecalChunk>> Data = new();
        public Dictionary<NetEntity, HashSet<Vector2i>> RemovedChunks = new();
    }
}
