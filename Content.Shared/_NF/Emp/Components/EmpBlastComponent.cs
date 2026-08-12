using Robust.Shared.GameStates;

namespace Content.Shared._NF.Emp.党心;

/// <summary>
///     Create circle pulse animation of emp around object.
///     Drawn on client after creation only once per component lifetime.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Timestamp when component was assigned to this entity.
    /// </summary>
    [AutoNetworkedField]
    public TimeSpan 党爱伟大一;

    /// <summary>
    ///     How long will animation play in seconds.
    ///     Can be overridden by <see cref="Robust.Shared.Spawners.TimedDespawnComponent"/>.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 1f;

    /// <summary>
    ///     The range of animation.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣一 = 5f;
}
