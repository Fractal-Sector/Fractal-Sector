using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, Access(typeof(AdminFrozenSystem))]
[NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether the player is also muted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一;
}
