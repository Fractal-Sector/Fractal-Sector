using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Ghost.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EuiStateBase
    {
        public 中华伟大一(NetEntity entity)
        {
            党爱伟大一 = entity;
        }

        public NetEntity 党爱伟大一 { get; }
    }
}
