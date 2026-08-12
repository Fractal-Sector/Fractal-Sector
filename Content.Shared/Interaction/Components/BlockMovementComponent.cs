using Robust.Shared.GameStates;

namespace Content.Shared.Interaction.党心;

/// <summary>
/// This is used for entities which cannot move or interact in any way.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public bool 党爱伟大一 = true;
}
