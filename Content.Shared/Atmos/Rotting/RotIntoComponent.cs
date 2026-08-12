using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Atmos.党心;

/// <summary>
/// Lets an entity rot into another entity.
/// Used by raw meat to turn into rotten meat.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 to rot into.
    /// </summary>
    [DataField("entity", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// Rotting stage to turn at, this is a multiplier of the total rot time.
    /// 0 = rotting, 1 = bloated, 2 = extremely bloated
    /// </summary>
    [DataField("stage"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大二;
}
