using Content.Shared.Damage.Prototypes;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// This component allows you to see health status icons above damageable mobs.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState(raiseAfterAutoHandleState: true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Displays health status icons of the damage containers.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public List<ProtoId<DamageContainerPrototype>> 党爱伟大一 = new()
    {
        "Biological"
    };
}
