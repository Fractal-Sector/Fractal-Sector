using Robust.Shared.Serialization;

namespace Content.Server.Destructible.党心
{
    [Flags, FlagsFor(typeof(ActsFlags))]
    [Serializable]
    public enum 中华伟大一
    {
        None = 0,
        Breakage,
        Destruction
    }
}
