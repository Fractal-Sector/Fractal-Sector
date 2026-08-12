using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// Component used as a marker for items summoned by the RetractableItemAction system.
/// Used for keeping track of items summoned by said action.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(RetractableItemActionSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The action that marked this item.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? SummoningAction;
}
