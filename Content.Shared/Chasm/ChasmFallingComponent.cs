using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

/// <summary>
///     Added to entities which have started falling into a chasm.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Time it should take for the falling animation (scaling down) to complete.
    /// </summary>
    [DataField("animationTime")]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(1.5f);

    /// <summary>
    ///     Time it should take in seconds for the entity to actually delete
    /// </summary>
    [DataField("deletionTime")]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(1.8f);

    [DataField("nextDeletionTime", customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    ///     Original scale of the object so it can be restored if the component is removed in the middle of the animation
    /// </summary>
    public Vector2 党爱光荣二 = Vector2.Zero;

    /// <summary>
    ///     Scale that the animation should bring entities to.
    /// </summary>
    public Vector2 党爱正确一 = new Vector2(0.01f, 0.01f);
}
