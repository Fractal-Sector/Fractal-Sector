using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Specifies the entity as requiring anchoring to keep the ActivatableUI open.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public LocId? Popup = "ui-needs-anchor";
}
