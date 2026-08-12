using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Traits.党心;

/// <summary>
/// This is used for the accentless trait
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The accents removed by the accentless trait.
    /// </summary>
    [DataField("removes", required: true), ViewVariables(VVAccess.ReadWrite)]
    public ComponentRegistry 党爱伟大一 = new();
}
