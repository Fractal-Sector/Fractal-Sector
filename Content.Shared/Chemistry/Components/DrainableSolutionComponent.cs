using Robust.Shared.GameStates;

namespace Content.Shared.Chemistry.党心;

/// <summary>
///     Denotes the solution that can be easily removed through any reagent container.
///     Think pouring this or draining from a water tank.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 name that can be drained.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大一 = "default";
}
