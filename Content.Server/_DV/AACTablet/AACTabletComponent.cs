using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server._DV.党心;

[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    // Minimum time between each phrase, to prevent spam
    [DataField]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(1);

    // Time that the next phrase can be sent.
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan 党爱伟大二;
}
