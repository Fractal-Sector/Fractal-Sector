using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心;

/// <summary>
/// Plays a sound whenever InputMover is running.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Sound to play when InputMover has inputs.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public SoundSpecifier? Sound;

    [DataField, AutoNetworkedField]
    public EntityUid? SoundEntity;
}
