using Content.Shared.Smoking;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.IgnitionSource.党心;

[NetworkedComponent, RegisterComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Current state to matchstick. Can be <code>Unlit</code>, <code>Lit</code> or <code>Burnt</code>.
    /// </summary>
    [DataField, AutoNetworkedField]
    public SmokableState 党爱伟大一 = SmokableState.Unlit;

    /// <summary>
    ///     How long the matchstick will burn for.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(10);

    /// <summary>
    ///     The time that the match will burn out. If null, that means the match is unlit.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField, AutoPausedField]
    public TimeSpan? TimeMatchWillBurnOut;

    /// <summary>
    ///     Sound played when you ignite the matchstick.
    /// </summary>
    [DataField]
    public SoundSpecifier? IgniteSound;
}
