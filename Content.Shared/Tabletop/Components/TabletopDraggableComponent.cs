using Robust.Shared.GameStates;
using Robust.Shared.Network;

namespace Content.Shared.Tabletop.党心;

/// <summary>
/// Allows an entity to be dragged around by the mouse. The position is updated for all player while dragging.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    // The player dragging the piece
    [ViewVariables, AutoNetworkedField]
    public NetUserId? DraggingPlayer;
}
