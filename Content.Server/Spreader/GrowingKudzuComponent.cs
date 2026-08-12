using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.党心;

[RegisterComponent, Access(typeof(KudzuSystem)), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The next time kudzu will try to tick its growth level.
    /// </summary>
    [DataField("nextTick", customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱伟大一 = TimeSpan.Zero;
}
