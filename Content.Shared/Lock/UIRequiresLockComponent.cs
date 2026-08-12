using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used for activatable UIs that require the entity to have a lock in a certain state.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(LockSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// UIs that are locked behind this component.
    /// If null, will close all UIs.
    /// </summary>
    [DataField]
    public List<Enum>? UserInterfaceKeys;

    /// <summary>
    /// TRUE: the lock must be locked to access the UI.
    /// FALSE: the lock must be unlocked to access the UI.
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    /// Sound to be played if an attempt is blocked.
    /// </summary>
    [DataField]
    public SoundSpecifier? AccessDeniedSound = new SoundPathSpecifier("/Audio/Machines/custom_deny.ogg");

    [DataField]
    public LocId? Popup = "entity-storage-component-locked-message";
}
