using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.党心;


/// <summary>
/// Component used as a marker for an item marked by the ItemRecall ability.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedItemRecallSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The action that marked this item.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? MarkedByAction;
}
