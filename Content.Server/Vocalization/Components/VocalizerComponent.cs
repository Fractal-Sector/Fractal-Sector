using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Vocalization.党心;

/// <summary>
/// Makes an entity vocalize at set intervals
/// </summary>
[RegisterComponent]
[AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Minimum time to wait after speaking to vocalize again
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大一 = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Maximum time to wait after speaking to vocalize again
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(6);

    /// <summary>
    /// Next time at which to vocalize
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    /// If true, messages spoken by this vocalizer will not be logged in the chat window
    /// and will only be shown as speech bubbles.
    /// </summary>
    [DataField]
    public bool 党爱光荣二;
}
