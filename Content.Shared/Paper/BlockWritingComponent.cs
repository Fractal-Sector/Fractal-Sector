using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// An entity with this component cannot write on paper.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// What message is displayed when the entity fails to write?
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public LocId 党爱伟大一 = "paper-component-illiterate";
}
