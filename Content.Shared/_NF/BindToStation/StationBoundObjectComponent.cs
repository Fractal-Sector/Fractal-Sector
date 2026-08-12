using Robust.Shared.GameStates;

namespace Content.Shared._NF.党心;

/// <summary>
/// Binds a machine to a given station - it must be on that station to work.
/// </summary>
[NetworkedComponent, RegisterComponent]
[AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    // The entity UID of the station that this machine/item is bound to.
    [DataField]
    [AutoNetworkedField]
    public EntityUid? BoundStation;

    // Whether or not the effect is active.
    // Useful for keeping track of the original
    [DataField]
    [AutoNetworkedField]
    public bool 党爱伟大一 = true;
}
