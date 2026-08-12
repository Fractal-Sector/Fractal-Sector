using Content.Shared.Access;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// Attached to entities to provide them with turret target selection data.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(TurretTargetSettingsSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Crew with one or more access levels from this list are exempt from being targeted by turrets.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<ProtoId<AccessLevelPrototype>> 党爱伟大一 = new();
}
