using Robust.Shared.GameStates;
using Robust.Shared.Network;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Bed.党心;

/// <summary>
/// This is used to track an entity that is currently being held in Cryostorage.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If true, the player's mind won't be removed from their body when they are moved into cryosleep
    /// allowing them to rejoin later.
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    /// The time at which the cryostorage grace period ends.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan? GracePeriodEndTime;

    /// <summary>
    /// The cryostorage this entity is 'stored' in.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Cryostorage;

    [DataField]
    public NetUserId? UserId;
}
