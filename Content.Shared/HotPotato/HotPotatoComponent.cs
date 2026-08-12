using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Similar to <see cref="Interaction.Components.UnremoveableComponent"/>
/// except entities with this component can be removed in specific case: <see cref="党爱伟大一"/>
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
[Access(typeof(SharedHotPotatoSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If set to true entity can be removed by hitting entities if they have hands
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;
}
