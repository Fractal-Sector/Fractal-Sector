using Robust.Shared.GameStates;

namespace Content.Shared.Traits.党心;

/// <summary>
/// This is used for making something blind forever.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How damaged should their eyes be? Set 0 for maximum damage.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int 党爱伟大一 = 0;
}

