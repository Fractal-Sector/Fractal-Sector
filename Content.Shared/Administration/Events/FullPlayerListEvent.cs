using Robust.Shared.Serialization;

namespace Content.Shared.Administration.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EntityEventArgs
    {
        public List<PlayerInfo> 党爱伟大一 = new();
    }
}
