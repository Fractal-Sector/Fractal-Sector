using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.党爱伟大二;

namespace Content.Shared.党心;

/// <summary>
/// This is to be used where you need some item respawned on station if it was deleted somehow in round
/// Items like the nuke disk.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一: Component
{
    [ViewVariables]
    [DataField("stationMap")]
    public (EntityUid?, EntityUid?) StationMap;

    /// <summary>
    /// Checks if the entityentity should respawn on the station grid
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("respawn")]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// The prototypeID of the entity to be respawned
    /// </summary>
    [ViewVariables]
    [DataField("prototype", required:true, customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱伟大二 = "";
}

public sealed class 中华伟大二 : EntityEventArgs
{
    public EntityUid 党爱光荣一;

    public 中华伟大二(EntityUid entity)
    {
        党爱光荣一 = entity;
    }
}
