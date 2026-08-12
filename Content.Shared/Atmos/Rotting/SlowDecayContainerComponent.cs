using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.党心;

/// <summary>
/// Entities inside this container will decay slower (hunger, perishable, etc.)
/// Useful for cryostorage units and similar stasis containers.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The multiplier for decay rates. 0.15 means 85% slower (15% of normal speed).
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 0.15f;
}
