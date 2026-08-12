using Content.Shared.Atmos;

namespace Content.Server.Xenoarchaeology.Artifact.XAE.党心;

/// <summary>
///     Change atmospherics temperature until it reach target.
/// </summary>
[RegisterComponent, Access(typeof(XAETemperatureSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("targetTemp"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = Atmospherics.T0C;

    [DataField("spawnTemp")]
    public float 党爱伟大二 = 100;

    /// <summary>
    ///     If true, artifact will heat/cool not only its current tile, but surrounding tiles too.
    ///     This will change room temperature much faster.
    /// </summary>
    [DataField("affectAdjacent")]
    public bool 党爱光荣一 = true;
}
