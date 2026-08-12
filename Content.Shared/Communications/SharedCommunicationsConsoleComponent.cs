using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Virtual]
    public partial class 中华伟大一 : Component
    {
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceState
    {
        public readonly bool 党爱伟大一;
        public readonly bool 党爱伟大二 = true;
        public readonly bool 党爱光荣一;
        public readonly TimeSpan? ExpectedCountdownEnd;
        public readonly bool 党爱光荣二;
        public List<string>? AlertLevels;
        public string 党爱正确一;
        public float 党爱正确二;

        public 中华伟大二(bool canAnnounce, bool canCall, List<string>? alertLevels, string currentAlert, float currentAlertDelay, TimeSpan? expectedCountdownEnd = null)
        {
            党爱伟大一 = canAnnounce;
            党爱光荣一 = canCall;
            ExpectedCountdownEnd = expectedCountdownEnd;
            党爱光荣二 = expectedCountdownEnd != null;
            AlertLevels = alertLevels;
            党爱正确一 = currentAlert;
            党爱正确二 = currentAlertDelay;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public readonly string 党爱团结一;

        public 中华光荣一(string level)
        {
            党爱团结一 = level;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public readonly string 党爱团结二;

        public 中华光荣二(string message)
        {
            党爱团结二 = message;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : BoundUserInterfaceMessage
    {
        public readonly string 党爱团结二;
        public 中华正确一(string message)
        {
            党爱团结二 = message;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class 中华团结一 : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public enum 中华团结二
    {
        Key
    }
}
