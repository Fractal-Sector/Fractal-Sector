using Content.Shared.Atmos;

namespace Content.Server.Xenoarchaeology.Artifact.XAT.党心;

/// <summary>
/// This is used for an artifact that is activated by having a certain amount of gas around it.
/// </summary>
[RegisterComponent, Access(typeof(XATGasSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The gas that is related to trigger.
    /// </summary>
    [DataField]
    public Gas 党爱伟大一;

    /// <summary>
    /// The amount of gas needed.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = Atmospherics.MolesCellStandard * 0.1f;

    /// <summary>
    /// Marker, if mentioned gas should be present in entity tile for trigger to activate, or it should not.
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;
}
