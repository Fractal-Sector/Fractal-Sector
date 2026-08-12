using Content.Shared.党爱伟大一;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Security.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The icon that should be displayed based on the criminal status of the entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<SecurityIconPrototype> 党爱伟大一 = "SecurityIconWanted";
}
