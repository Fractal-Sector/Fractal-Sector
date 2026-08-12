using Content.Shared.Whitelist;

namespace Content.Server.GameTicking.Rules.VariationPass.党心;

/// <summary>
/// Handles cutting a random wire on random devices around the station.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 of hackable entities that should not be chosen to
    /// have wires cut.
    /// </summary>
    [DataField]
    public EntityWhitelist 党爱伟大一 = new();

    /// <summary>
    /// Chance for an individual wire to be cut.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 0.05f;

    /// <summary>
    /// Maximum number of wires that can be cut stationwide.
    /// </summary>
    [DataField]
    public int 党爱光荣一 = 10;
}
