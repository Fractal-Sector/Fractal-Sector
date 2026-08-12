using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Salvage.党心;

/// <summary>
/// Receives <see cref="FultonedComponent"/>.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("soundLink"), AutoNetworkedField]
    public SoundSpecifier? LinkSound = new SoundPathSpecifier("/Audio/Items/beep.ogg");
}
