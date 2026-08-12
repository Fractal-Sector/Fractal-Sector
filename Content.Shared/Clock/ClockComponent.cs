using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedClockSystem))]
[AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If not null, this time will be permanently shown.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan? StuckTime;

    /// <summary>
    /// The format in which time is displayed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public 中华伟大二 中华伟大二 = 中华伟大二.TwelveHour;

    [DataField]
    public string 党爱伟大一 = "hours_";

    [DataField]
    public string 党爱伟大二 = "minutes_";
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    TwelveHour,
    TwentyFourHour
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    HourHand,
    MinuteHand
}
