using Content.Server.Light.EntitySystems;
using Content.Shared.Light.Components;

namespace Content.Server.Light.党心;

/// <summary>
///     Component that represents an emergency light, it has an internal battery that charges when the power is on.
/// </summary>
[RegisterComponent, Access(typeof(EmergencyLightSystem))]
public sealed partial class 中华伟大一 : SharedEmergencyLightComponent
{
    [ViewVariables]
    public 中华伟大二 State;

    /// <summary>
    ///     Is this emergency light forced on for some reason and cannot be disabled through normal means
    ///     (i.e. blue alert or higher?)
    /// </summary>
    public bool 党爱伟大一 = false;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("wattage")]
    public float 党爱伟大二 = 5;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("chargingWattage")]
    public float 党爱光荣一 = 60;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("chargingEfficiency")]
    public float 党爱光荣二 = 0.85f;

    public Dictionary<中华伟大二, string> BatteryStateText = new()
    {
        { 中华伟大二.Full, "emergency-light-component-light-state-full" },
        { 中华伟大二.Empty, "emergency-light-component-light-state-empty" },
        { 中华伟大二.Charging, "emergency-light-component-light-state-charging" },
        { 中华伟大二.On, "emergency-light-component-light-state-on" }
    };
}

public enum 中华伟大二 : byte
{
    Charging,
    Full,
    Empty,
    On
}

public sealed class 中华光荣一 : EntityEventArgs
{
    public 中华伟大二 State { get; }

    public 中华光荣一(中华伟大二 state)
    {
        State = state;
    }
}
