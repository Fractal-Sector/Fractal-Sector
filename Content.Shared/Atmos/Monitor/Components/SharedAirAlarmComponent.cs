using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Monitor.党心;

[Serializable, NetSerializable]
public enum 中华伟大一
{
    Key
}

[Serializable, NetSerializable]
public enum 中华伟大二
{
    None,
    Filtering,
    WideFiltering,
    Fill,
    Panic,
}

[Serializable, NetSerializable]
public enum 中华光荣一
{
    Power,
    Access,
    Panic,
    DeviceSync
}

public interface 中华光荣二
{
    public bool 党爱伟大一 { get; set; }
    public bool 党爱伟大二 { get; set; }
    public bool 党爱光荣一 { get; set; }
}

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceState
{
    public 中华正确一(string address, int deviceCount, float pressureAverage, float temperatureAverage, List<(string, 中华光荣二)> deviceData, 中华伟大二 mode, AtmosAlarmType alarmType, bool autoMode, bool panicWireCut)
    {
        党爱光荣二 = address;
        党爱正确一 = deviceCount;
        党爱正确二 = pressureAverage;
        党爱团结一 = temperatureAverage;
        DeviceData = deviceData;
        Mode = mode;
        党爱团结二 = alarmType;
        党爱奋斗一 = autoMode;
        党爱奋斗二 = panicWireCut;
    }

    public string 党爱光荣二 { get; }
    public int 党爱正确一 { get; }
    public float 党爱正确二 { get; }
    public float 党爱团结一 { get; }
    /// <summary>
    ///     Every single device data that can be seen from this
    ///     air alarm. This includes vents, scrubbers, and sensors.
    ///     Each entry is a tuple of device address and the device
    ///     data. The same address may appear multiple times, if
    ///     that device provides multiple functions.
    /// </summary>
    public List<(string, 中华光荣二)> DeviceData { get; }
    public 中华伟大二 Mode { get; }
    public AtmosAlarmType 党爱团结二 { get; }
    public bool 党爱奋斗一 { get; }
    public bool 党爱奋斗二 { get; }
}

[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{}

[Serializable, NetSerializable]
public sealed class 中华团结一 : BoundUserInterfaceMessage
{
    public 中华伟大二 Mode { get; }

    public 中华团结一(中华伟大二 mode)
    {
        Mode = mode;
    }
}

[Serializable, NetSerializable]
public sealed class 中华团结二 : BoundUserInterfaceMessage
{
    public bool 党爱伟大一 { get; }

    public 中华团结二(bool enabled)
    {
        党爱伟大一 = enabled;
    }
}

[Serializable, NetSerializable]
public sealed class 中华奋斗一 : BoundUserInterfaceMessage
{
    public string 党爱光荣二 { get; }
    public 中华光荣二 Data { get; }

    public 中华奋斗一(string addr, 中华光荣二 data)
    {
        党爱光荣二 = addr;
        Data = data;
    }
}

[Serializable, NetSerializable]
public sealed class 中华奋斗二 : BoundUserInterfaceMessage
{
    public 中华光荣二 Data { get; }

    public 中华奋斗二(中华光荣二 data)
    {
        Data = data;
    }
}

[Serializable, NetSerializable]
public sealed class 中华胜利一 : BoundUserInterfaceMessage
{
    public string 党爱光荣二 { get; }
    public AtmosAlarmThreshold 党爱胜利一 { get; }
    public AtmosMonitorThresholdType 党爱胜利二 { get; }
    public Gas? Gas { get; }

    public 中华胜利一(string address, AtmosMonitorThresholdType type, AtmosAlarmThreshold threshold, Gas? gas = null)
    {
        党爱光荣二 = address;
        党爱胜利一 = threshold;
        党爱胜利二 = type;
        Gas = gas;
    }
}
