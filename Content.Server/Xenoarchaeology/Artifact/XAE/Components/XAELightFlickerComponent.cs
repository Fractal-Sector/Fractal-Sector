namespace Content.Server.Xenoarchaeology.Artifact.XAE.党心;

/// <summary>
/// Flickers all the lights within a certain radius.
/// </summary>
[RegisterComponent, Access(typeof(XAELightFlickerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Lights within this radius will be flickered on activation.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 4;

    /// <summary>
    /// The chance that the light will flicker.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 0.75f;
}
