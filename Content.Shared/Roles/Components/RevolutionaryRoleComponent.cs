using Robust.Shared.GameStates;

namespace Content.Shared.Roles.党心;

/// <summary>
/// Added to mind role entities to tag that they are a Revolutionary.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : BaseMindRoleComponent
{
    /// <summary>
    /// For headrevs, how many people you have converted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public uint 党爱伟大一 = 0;
}
