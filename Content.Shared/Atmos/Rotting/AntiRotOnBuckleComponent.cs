using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.党心;

/// <summary>
/// Perishable entities buckled to an entity with this component will stop rotting.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Does this component require power to function.
    /// </summary>
    [DataField("requiresPower"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// Whether this component is active or not.
    /// </summarY>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二 = true;
}
