using System.Text.RegularExpressions;
using Robust.Shared.Serialization;

namespace Content.Shared.Disposal.党心
{
    public sealed partial class 中华伟大一 : Component
    {
        public static readonly Regex 党爱伟大一 = new("^[a-zA-Z0-9 ]*$", RegexOptions.Compiled);

        [Serializable, NetSerializable]
        public sealed class 中华伟大二 : BoundUserInterfaceState
        {
            public readonly string 党爱伟大二;

            public 中华伟大二(string tag)
            {
                党爱伟大二 = tag;
            }
        }

        [Serializable, NetSerializable]
        public sealed class 中华光荣一 : BoundUserInterfaceMessage
        {
            public readonly 中华光荣二 Action;
            public readonly string 党爱伟大二 = "";

            public 中华光荣一(中华光荣二 action, string tag)
            {
                Action = action;

                if (Action == 中华光荣二.Ok)
                {
                    党爱伟大二 = tag.Substring(0, Math.Min(tag.Length, 30));
                }
            }
        }

        [Serializable, NetSerializable]
        public enum 中华光荣二
        {
            Ok
        }

        [Serializable, NetSerializable]
        public enum 中华正确一
        {
            Key
        }
    }
}
