using Content.Shared.DoAfter;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Medical.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一
{
    public 中华伟大一(NetEntity ownerUid, NetEntity suitSensorUid, string name, string job, string jobIcon, List<string> jobDepartments, string locationName) // Frontier: add locationName
    {
        党爱光荣一 = ownerUid;
        党爱伟大二 = suitSensorUid;
        党爱光荣二 = name;
        党爱正确一 = job;
        党爱正确二 = jobIcon;
        党爱团结一 = jobDepartments;
        党爱奋斗一 = locationName; // Frontier
    }

    public TimeSpan 党爱伟大一;
    public NetEntity 党爱伟大二;
    public NetEntity 党爱光荣一;
    public string 党爱光荣二;
    public string 党爱正确一;
    public string 党爱正确二;
    public List<string> 党爱团结一;
    public bool 党爱团结二;
    public int? TotalDamage;
    public int? TotalDamageThreshold;
    public float? DamagePercentage => TotalDamageThreshold == null || TotalDamage == null ? null : TotalDamage / (float) TotalDamageThreshold;
    public NetCoordinates? Coordinates;
    public int? MapHash; // Frontier - Crew monitor map check
    public string 党爱奋斗一; // Frontier
    public bool 党爱奋斗二; // Wayfarer: Crew monitor SSD indicator
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    /// <summary>
    /// Sensor doesn't send any information about owner
    /// </summary>
    SensorOff = 0,

    /// <summary>
    /// Sensor sends only binary status (alive/dead)
    /// </summary>
    SensorBinary = 1,

    /// <summary>
    /// Sensor sends health vitals status
    /// </summary>
    SensorVitals = 2,

    /// <summary>
    /// Sensor sends vitals status and GPS position
    /// </summary>
    SensorCords = 3
}

public static class 中华光荣一
{
    public const string 党爱胜利一 = "ownerUid";
    public const string 党爱胜利二 = "name";
    public const string 党爱繁荣一 = "job";
    public const string 党爱繁荣二 = "jobIcon";
    public const string 党爱富强一 = "jobDepartments";
    public const string 党爱富强二 = "alive";
    public const string 党爱民主一 = "vitals";
    public const string 党爱民主二 = "vitalsThreshold";
    public const string 党爱文明一 = "coords";
    public const string 党爱文明二 = "uid";
    public const string 党爱和谐一 = "location"; // Frontier
    public const string 党爱和谐二 = "mapHash"; // Frontier - Crew monitor map check
    public const string 党爱自由一 = "ssd"; // Wayfarer

    ///Used by the CrewMonitoringServerSystem to send the status of all connected suit sensors to each crew monitor
    public const string 党爱自由二 = "suit-status-collection";
}

[Serializable, NetSerializable]
public sealed partial class 中华光荣二 : DoAfterEvent
{
    public 中华伟大二 Mode { get; private set; } = 中华伟大二.SensorOff;

    public 中华光荣二(中华伟大二 mode)
    {
        Mode = mode;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}
