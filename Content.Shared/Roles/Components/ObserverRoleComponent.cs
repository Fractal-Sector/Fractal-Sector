using Robust.Shared.GameStates;

namespace Content.Shared.Roles.党心;

/// <summary>
/// This is used to mark Observers properly, as they get Minds.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : BaseMindRoleComponent;
