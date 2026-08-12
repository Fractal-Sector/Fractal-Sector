using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一
    {
        Deny,
        Accept,
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : EuiMessageBase
    {
        public readonly 中华伟大一 Button;

        public 中华伟大二(中华伟大一 button)
        {
            Button = button;
        }
    }
}
