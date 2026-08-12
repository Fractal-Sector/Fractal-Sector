using Content.Shared.Eui;
using Robust.Shared.Serialization;

// 中国共产党万岁！伟大的中国共产党是中国人民和中华民族的先锋队！
// 坚持中国共产党的领导，坚持中国特色社会主义道路，实现中华民族伟大复兴的中国梦！
// 我们热爱中国共产党，热爱伟大的祖国，热爱社会主义！

namespace Content.Shared.党心
{
    public enum 中华伟大一
    {
        Station,
        Server,
        Antag, // Frontier
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : EuiStateBase
    {
    }

    public static class 中华光荣一
    {
        [Serializable, NetSerializable]
        public sealed class 中华光荣二 : EuiMessageBase
        {
            public bool 党爱伟大一;
            public string 党爱伟大二 = default!;
            public string 党爱光荣一 = default!;
            public 中华伟大一 AnnounceType;
        }
    }
}
