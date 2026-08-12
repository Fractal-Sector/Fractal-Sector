using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Stores a list of all stuck entities to release when this entity is deleted.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> 党爱伟大一 = new();
}
