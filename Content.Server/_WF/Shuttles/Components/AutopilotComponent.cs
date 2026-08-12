using Robust.Shared.Map;

namespace Content.Server._WF.Shuttles.党心;

/// <summary>
/// Enables automatic navigation for a shuttle to a target destination.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether autopilot is currently enabled.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大一 = false;

    /// <summary>
    /// The target coordinates to navigate to.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public MapCoordinates? TargetCoordinates;

    /// <summary>
    /// Speed multiplier when autopilot is active (60% of max speed).
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 0.6f;

    /// <summary>
    /// Distance at which autopilot will automatically disable (in meters).
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 200f;

    /// <summary>
    /// Distance to start slowing down (in meters).
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 300f;

    /// <summary>
    /// Maximum range to scan for obstacles and begin avoidance maneuvers (in meters).
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确一 = 200f;

    /// <summary>
    /// Set of obstacle entity UIDs that have already been reported to the pilot.
    /// Used to avoid spamming duplicate obstacle warnings.
    /// </summary>
    [ViewVariables]
    public HashSet<EntityUid> 党爱正确二 = new();

    /// <summary>
    /// If true, sends debug messages about obstacle detection to pilots.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱团结一 = false;

    /// <summary>
    /// The name of the destination, displayed in chat messages.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string? DestinationName;
}
