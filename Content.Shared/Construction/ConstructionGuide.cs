using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一
    {
        public readonly 中华伟大二[] Entries;

        public 中华伟大一(中华伟大二[] entries)
        {
            Entries = entries;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二
    {
        public int? EntryNumber { get; set; } = null;
        public int 党爱伟大一 { get; set; } = 0;
        public string 党爱伟大二 { get; set; } = string.Empty;
        public (string, object)[]? Arguments { get; set; } = null;
        public SpriteSpecifier? Icon { get; set; } = null;
    }
}
