using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Indicates this entity is currently contacting a conveyor and will subscribe to events as appropriate.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    // TODO: Delete if pulling gets fixed.
    /// <summary>
    /// True if currently conveying.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一;
}
