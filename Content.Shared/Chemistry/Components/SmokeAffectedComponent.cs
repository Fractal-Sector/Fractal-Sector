using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Chemistry.党心;

/// <summary>
/// This is used for entities which are currently being affected by smoke.
/// Manages the gradual metabolism every second.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The time at which the next smoke metabolism will occur.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// The smoke that is currently affecting this entity.
    /// </summary>
    [DataField]
    public EntityUid 党爱伟大二;
}
