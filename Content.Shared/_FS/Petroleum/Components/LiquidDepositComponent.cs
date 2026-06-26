using Content.Shared.Chemistry.Reagent;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._FS.Petroleum;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedOilDrillSystem))]
public sealed partial class LiquidDepositComponent : Component
{
    /// <summary>
    /// ID reagents
    /// </summary>
    [DataField("reagentId", customTypeSerializer: typeof(PrototypeIdSerializer<ReagentPrototype>))]
    public string ReagentId = "CrudeOil";

    /// <summary>
    /// Current amount in the deposit
    /// </summary>
    [DataField("amount")]
    public float Amount = 5000f;

    /// <summary>
    /// The maximum amount of this deposit
    /// </summary>
    [DataField("maxAmount")]
    public float MaxAmount = 5000f;
}
