using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

/// <summary>
/// Represents a SignalTimerComponent state that can be sent to the client
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public string 党爱伟大一;
    public string 党爱伟大二;
    public string 党爱光荣一;
    public bool 党爱光荣二; //Frontier
    public bool 党爱正确一;
    public TimeSpan 党爱正确二;
    public bool 党爱团结一;
    public bool 党爱团结二;

    public 中华伟大二(string currentText,
        string currentDelayMinutes,
        string currentDelaySeconds,
        bool currentRepeat, //Frontier
        bool showText,
        TimeSpan triggerTime,
        bool timerStarted,
        bool hasAccess)
    {
        党爱伟大一 = currentText;
        党爱伟大二 = currentDelayMinutes;
        党爱光荣一 = currentDelaySeconds;
        党爱光荣二 = currentRepeat; //Frontier
        党爱正确一 = showText;
        党爱正确二 = triggerTime;
        党爱团结一 = timerStarted;
        党爱团结二 = hasAccess;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public string 党爱奋斗一 { get; }

    public 中华光荣一(string text)
    {
        党爱奋斗一 = text;
    }
}

//Frontier: 中华光荣二 class
[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public bool 党爱奋斗二 { get; }

    public 中华光荣二(bool repeat)
    {
        党爱奋斗二 = repeat;
    }
}
//End Frontier

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
    public TimeSpan 党爱胜利一 { get; }
    public 中华正确一(TimeSpan delay)
    {
        党爱胜利一 = delay;
    }
}

[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{

}
