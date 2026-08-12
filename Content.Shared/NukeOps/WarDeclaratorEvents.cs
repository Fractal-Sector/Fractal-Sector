using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一
{
    Key,
}

public enum 中华伟大二 : byte
{
    WarReady,
    YesWar,
    NoWarUnknown,
    NoWarTimeout,
    NoWarSmallCrew,
    NoWarShuttleDeparted
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceState
{
    public 中华伟大二? Status;
    public TimeSpan 党爱伟大一;
    public TimeSpan 党爱伟大二;

    public 中华光荣一(中华伟大二? status, TimeSpan endTime, TimeSpan shuttleDisabledTime)
    {
        Status = status;
        党爱伟大二 = endTime;
        党爱伟大一 = shuttleDisabledTime;
    }

}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public string 党爱光荣一 { get; }

    public 中华光荣二(string msg)
    {
        党爱光荣一 = msg;
    }
}
