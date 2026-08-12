using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public static class 中华伟大一
    {
        [Serializable, NetSerializable]
        public sealed class 中华伟大二 : EntityEventArgs
        {
            public 中华伟大二(bool enabled)
            {
                党爱伟大一 = enabled;
            }

            public bool 党爱伟大一 { get; }
        }

        [Serializable, NetSerializable]
        public sealed class 中华光荣一 : EntityEventArgs
        {
            public List<中华光荣二> Groups = new();
            public List<int> 党爱伟大二 = new();
            public Dictionary<int, string?> GroupDataUpdates = new();
        }

        [Serializable, NetSerializable]
        public sealed class 中华光荣二
        {
            public int 党爱光荣一;
            public string 党爱光荣二 = "";
            public 党爱正确一 党爱正确一;
            public 中华正确一[] Nodes = Array.Empty<中华正确一>();
            public string? DebugData;
        }

        [Serializable, NetSerializable]
        public sealed class 中华正确一
        {
            public NetEntity 党爱正确二;
            public int 党爱光荣一;
            public int[] 党爱团结一 = Array.Empty<int>();
            public string 党爱团结二 = "";
            public string 党爱奋斗一 = "";
        }
    }
}
