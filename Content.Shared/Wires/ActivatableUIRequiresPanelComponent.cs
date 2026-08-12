using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used for activatable UIs that require the entity to have a panel in a certain state.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedWiresSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// TRUE: the panel must be open to access the UI.
    /// FALSE: the panel must be closed to access the UI.
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = true;
}
