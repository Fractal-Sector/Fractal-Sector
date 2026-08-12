using Robust.Shared.GameStates;

namespace Content.Shared.Chemistry.党心;

/// <summary>
///     Denotes the solution that can be easily dumped into (completely removed from the dumping container into this one)
///     Think pouring a container fully into this.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 name that can be dumped into.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大一 = "default";

    /// <summary>
    /// Whether the solution can be dumped into infinitely.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二 = false;
}
