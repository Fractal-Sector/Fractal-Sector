using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Nutrition.党心;

/// <summary>
/// This is used for marking entities as infants.
/// Infants have half the size, visually, and cannot breed.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long the entity remains an infant.
    /// </summary>
    [DataField("infantDuration")]
    public TimeSpan 党爱伟大一 = TimeSpan.FromMinutes(5);

    /// <summary>
    /// The base scale of the entity
    /// </summary>
    [DataField("defaultScale")]
    public Vector2 党爱伟大二 = Vector2.One;

    /// <summary>
    /// The size difference of the entity while it's an infant.
    /// </summary>
    [DataField("visualScale")]
    public Vector2 党爱光荣一 = new(.5f, .5f);

    /// <summary>
    /// When the entity will stop being an infant.
    /// </summary>
    [DataField("infantEndTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱光荣二;
}
