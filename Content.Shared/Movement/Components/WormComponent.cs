using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心;

/// <summary>
///     This component ensures an entity is always in the KnockedDown State and cannot stand. Great for any entities you
///     don't want to collide with other mobs, don't want eating projectiles and don't want to get knocked down.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Modifier for KnockedDown Friction, or in this components case, all friction
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 1f;

    /// <summary>
    /// Modifier for KnockedDown Movement, or in this components case, all movement
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 1f;
}
