using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Shared.Chemistry.党心;

/// <summary>
///     Reagents that can be added easily. For example like
///     pouring something into another beaker, glass, or into the gas
///     tank of a car.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 name that can added to easily.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大一 = "default";

    /// <summary>
    /// The maximum amount that can be transferred to the solution at once
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2? MaxRefill = null;

    /// <summary>
    /// Frontier: prevent transferring solution out into others
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二 = false;
}
