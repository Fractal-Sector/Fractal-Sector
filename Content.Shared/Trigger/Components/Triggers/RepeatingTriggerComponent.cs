using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Trigger.Components.党心;

/// <summary>
/// Constantly triggers after being added to an entity.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : BaseTriggerOnXComponent
{
    /// <summary>
    /// How long to wait between triggers.
    /// The first trigger starts this long after the component is added.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// When the next trigger will be.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoNetworkedField, AutoPausedField]
    public TimeSpan 党爱伟大二 = TimeSpan.Zero;
}
