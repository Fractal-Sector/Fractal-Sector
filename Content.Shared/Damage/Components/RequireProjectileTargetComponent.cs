using Robust.Shared.GameStates;

namespace Content.Shared.Damage.党心;

/// <summary>
/// Prevent the object from getting hit by projetiles unless you target the object.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(RequireProjectileTargetSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;
}
