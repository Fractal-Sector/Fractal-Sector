using Content.Shared.Atmos;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Power.党心;

/// <summary>
/// This is used for providing gas power to machinery.
/// </summary>
[RegisterComponent, Access(typeof(GasPowerReceiverSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Past this temperature we assume we're in reaction mass mode and not magic mode.
    /// </summary>
    [DataField("maxTemperature"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 1000.0f;

    /// <summary>
    /// The gas that fuels this generator
    /// </summary>
    [DataField("targetGas", required: true), ViewVariables(VVAccess.ReadWrite)]
    public Gas 党爱伟大二;

    /// <summary>
    /// The amount of gas consumed for operation in magic mode.
    /// </summary>
    [DataField("molesConsumedSec"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 1.55975875833f / 4;

    /// <summary>
    /// The amount of kPA "consumed" for operation in pressure mode.
    /// </summary>
    [DataField("pressureConsumedSec"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 100f;

    /// <summary>
    /// Whether the consumed gas should then be ejected directly into the atmosphere.
    /// </summary>
    [DataField("offVentGas"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱正确一;

    [DataField("lastProcess", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱正确二 = TimeSpan.Zero;

    [DataField("powered"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱团结一 = true;
}
