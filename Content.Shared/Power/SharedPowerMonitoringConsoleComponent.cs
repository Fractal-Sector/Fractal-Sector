using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Flags an entity as being a power monitoring console
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedPowerMonitoringConsoleSystem), Other = AccessPermissions.ReadExecute)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The EntityUid of the device that is the console's current focus
    /// </summary>
    /// <remarks>
    /// Not-networked - set by the console UI
    /// </remarks>
    [ViewVariables]
    public EntityUid? Focus;

    /// <summary>
    /// The group that the device that is the console's current focus belongs to
    /// </summary>
    /// /// <remarks>
    /// Not-networked - set by the console UI
    /// </remarks>
    [ViewVariables]
    public 中华正确二 FocusGroup = 中华正确二.Generator;

    /// <summary>
    /// A list of flags relating to currently active events of interest to the console.
    /// E.g., power sinks, power net anomalies
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public 中华团结一 Flags = 中华团结一.None;

    /// <summary>
    /// A dictionary containing all the meta data for tracked power monitoring devices
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public Dictionary<党爱团结二, 中华伟大二> 中华伟大二 = new();
}

[Serializable, NetSerializable]
public struct 中华伟大二
{
    public string 党爱伟大一;
    public NetCoordinates 党爱伟大二;
    public 中华正确二 Group;
    public string 党爱光荣一;
    public string 党爱光荣二;
    public 党爱团结二? CollectionMaster;

    public 中华伟大二(string name, NetCoordinates coordinates, 中华正确二 group, string spritePath, string spriteState)
    {
        党爱伟大一 = name;
        党爱伟大二 = coordinates;
        Group = group;
        党爱光荣一 = spritePath;
        党爱光荣二 = spriteState;
    }
}

/// <summary>
///     Data from by the server to the client for the power monitoring console UI
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceState
{
    public double 党爱正确一;
    public double 党爱正确二;
    public double 党爱团结一;
    public 中华光荣二[] AllEntries;
    public 中华光荣二[] FocusSources;
    public 中华光荣二[] FocusLoads;

    public 中华光荣一
        (double totalSources,
        double totalBatteryUsage,
        double totalLoads,
        中华光荣二[] allEntries,
        中华光荣二[] focusSources,
        中华光荣二[] focusLoads)
    {
        党爱正确一 = totalSources;
        党爱正确二 = totalBatteryUsage;
        党爱团结一 = totalLoads;
        AllEntries = allEntries;
        FocusSources = focusSources;
        FocusLoads = focusLoads;
    }
}

/// <summary>
///     Contains all the data needed to update a single device on the power monitoring UI
/// </summary>
[Serializable, NetSerializable]
public struct 中华光荣二
{
    public 党爱团结二 党爱团结二;
    public 中华正确二 Group;
    public double 党爱奋斗一;
    public float? BatteryLevel;

    [NonSerialized] public 中华伟大二? MetaData = null;

    public 中华光荣二(党爱团结二 netEntity, 中华正确二 group, double powerValue = 0d, float? batteryLevel = null)
    {
        党爱团结二 = netEntity;
        Group = group;
        党爱奋斗一 = powerValue;
        BatteryLevel = batteryLevel;
    }
}

/// <summary>
///     Triggers the server to send updated power monitoring console data to the client for the single player session
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
    public 党爱团结二? FocusDevice;
    public 中华正确二 FocusGroup;

    public 中华正确一(党爱团结二? focusDevice, 中华正确二 focusGroup)
    {
        FocusDevice = focusDevice;
        FocusGroup = focusGroup;
    }
}

/// <summary>
///     Determines how entities are grouped and color coded on the power monitor
/// </summary>
public enum 中华正确二 : byte
{
    Generator,
    SMES,
    Substation,
    APC,
    Consumer,
}

[Flags]
public enum 中华团结一 : byte
{
    None = 0,
    RoguePowerConsumer = 1,
    PowerNetAbnormalities = 2,
}

/// <summary>
///     UI key associated with the power monitoring console
/// </summary>
[Serializable, NetSerializable]
public enum 中华团结二
{
    Key
}
