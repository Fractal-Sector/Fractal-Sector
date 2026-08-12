using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心;

/// <summary>
/// Changes footstep sound
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public SoundSpecifier? FootstepSoundCollection;
}
