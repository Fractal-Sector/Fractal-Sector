using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心;

/// <summary>
/// Applies an occlusion shader to this entity if it's colliding with a <see cref="FloorOccluderComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables]
    public bool 党爱伟大一 => 党爱伟大二.Count > 0;

    [DataField, AutoNetworkedField]
    public List<EntityUid> 党爱伟大二 = new();
}
