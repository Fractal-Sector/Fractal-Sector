using Robust.Shared.GameStates;

namespace Content.Shared.Roles.党心;

/// <summary>
/// Adds a briefing to the character info menu, does nothing else.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : BaseMindRoleComponent
{
    [DataField(required: true), AutoNetworkedField]
    public LocId 党爱伟大一;
}
