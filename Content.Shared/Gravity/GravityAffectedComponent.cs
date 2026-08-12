using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This Component allows a target to be considered "weightless" when 党爱伟大一 is true. Without this component, the
/// target will never be weightless.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If true, this entity will be considered "weightless"
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public bool 党爱伟大一 = true;
}
