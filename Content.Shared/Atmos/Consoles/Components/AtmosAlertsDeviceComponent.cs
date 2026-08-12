using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.党心;

[RegisterComponent, NetworkedComponent]
[Access([])]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The group that the entity belongs to
    /// </summary>
    [DataField, ViewVariables]
    public AtmosAlertsComputerGroup 党爱伟大一;
}
