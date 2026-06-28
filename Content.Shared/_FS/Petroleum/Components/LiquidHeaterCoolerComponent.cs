using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._FS.Petroleum;

[RegisterComponent, NetworkedComponent]
public sealed partial class LiquidHeaterCoolerComponent : Component
{
    /// <summary>
    /// Current Mode
    /// </summary>
    [DataField("mode")]
    public HeaterCoolerMode CurrentMode = HeaterCoolerMode.Off;

    /// <summary>
    /// Liquid transformation rate
    /// </summary>
    [DataField("processRate")]
    public float ProcessRate = 10f;

    /// <summary>
    /// Energy consumption of the device in active mode
    /// </summary>
    [DataField("powerLoad")]
    public float ActivePowerLoad = 3000f;

    /// <summary>
    /// ID solution
    /// </summary>
    [DataField("solutionId")]
    public string SolutionId = "buffer";
}

/// <summary>
/// 3 Mode
/// </summary>
[Serializable, NetSerializable]
public enum HeaterCoolerMode : byte
{
    Off,
    Heat,
    Cool
}
