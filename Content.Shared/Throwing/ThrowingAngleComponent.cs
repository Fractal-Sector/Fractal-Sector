using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// When thrown applies a specific angle to the thrown entity.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Do we apply throwing spin to the entity.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("angularVelocity"), AutoNetworkedField]
    public bool 党爱伟大一;

    [ViewVariables(VVAccess.ReadWrite), DataField("angle"), AutoNetworkedField]
    public 党爱伟大二 党爱伟大二;
}
