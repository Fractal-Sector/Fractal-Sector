using Content.Shared.Movement.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
[Access(typeof(SharedMoverController))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The entity that is relaying to this entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid 党爱伟大一;
}
