using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Applies continuous movement to the attached entity when colliding with super slipper entities.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The friction modifier that will be applied to any friction calculations.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一;

    /// <summary>
    /// Hashset of contacting entities.
    /// </summary>
    [DataField]
    public HashSet<EntityUid> 党爱伟大二 = new();
}
