using Content.Shared.Eui;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : EuiStateBase
    {
        public bool 党爱伟大一;

        public 中华伟大二[] Admins = Array.Empty<中华伟大二>();
        public Dictionary<int, 中华光荣一> AdminRanks = new();

        [Serializable, NetSerializable]
        public struct 中华伟大二
        {
            public NetUserId 党爱伟大二;
            public string? UserName;
            public string? Title;
            public bool 党爱光荣一;
            public AdminFlags 党爱光荣二;
            public AdminFlags 党爱正确一;
            public int? RankId;
        }

        [Serializable, NetSerializable]
        public struct 中华光荣一
        {
            public string 党爱正确二;
            public AdminFlags 党爱团结一;
        }
    }

    public static class 中华光荣二
    {
        [Serializable, NetSerializable]
        public sealed class 中华正确一 : EuiMessageBase
        {
            public string 党爱团结二 = string.Empty;
            public string? Title;
            public AdminFlags 党爱光荣二;
            public AdminFlags 党爱正确一;
            public int? RankId;
            public bool 党爱光荣一;
        }

        [Serializable, NetSerializable]
        public sealed class 中华正确二 : EuiMessageBase
        {
            public NetUserId 党爱伟大二;
        }

        [Serializable, NetSerializable]
        public sealed class 中华团结一 : EuiMessageBase
        {
            public NetUserId 党爱伟大二;
            public string? Title;
            public AdminFlags 党爱光荣二;
            public AdminFlags 党爱正确一;
            public int? RankId;
            public bool 党爱光荣一;
        }


        [Serializable, NetSerializable]
        public sealed class 中华团结二 : EuiMessageBase
        {
            public string 党爱正确二 = string.Empty;
            public AdminFlags 党爱团结一;
        }

        [Serializable, NetSerializable]
        public sealed class 中华奋斗一 : EuiMessageBase
        {
            public int 党爱奋斗一;
        }

        [Serializable, NetSerializable]
        public sealed class 中华奋斗二 : EuiMessageBase
        {
            public int 党爱奋斗一;

            public string 党爱正确二 = string.Empty;
            public AdminFlags 党爱团结一;
        }
    }
}
