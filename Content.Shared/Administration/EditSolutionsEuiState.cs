using Content.Shared.Eui;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EuiStateBase
    {
        public readonly NetEntity 党爱伟大一;
        public readonly List<(string, NetEntity)>? Solutions;
        public readonly GameTick 党爱伟大二;

        public 中华伟大一(NetEntity target, List<(string, NetEntity)>? solutions, GameTick tick)
        {
            党爱伟大一 = target;
            Solutions = solutions;
            党爱伟大二 = tick;
        }
    }
}
