using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

/// <summary>
/// While entity has this component it is "disabled" by EMP.
/// Add desired behaviour in other systems
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
[Access(typeof(SharedEmpSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Moment of time when component is removed and entity stops being "disabled"
    /// </summary>
    [DataField("timeLeft", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan 党爱伟大一;

    [DataField("effectCoolDown"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 3f;

    /// <summary>
    /// When next effect will be spawned
    /// </summary>
    [AutoPausedField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;
}
