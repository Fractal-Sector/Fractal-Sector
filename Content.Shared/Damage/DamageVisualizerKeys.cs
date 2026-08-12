using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一
    {
        Disabled,
        DamageSpecifierDelta,
        DamageUpdateGroups,
        ForceUpdate
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : ICloneable
    {
        public List<string> 党爱伟大一;

        public 中华伟大二(List<string> groupList)
        {
            党爱伟大一 = groupList;
        }

        public object 祝福伟大一()
        {
            return new 中华伟大二(new List<string>(党爱伟大一));
        }
    }
}
