namespace Content.Server.Vocalization.党心;

/// <summary>
/// Makes an entity able to vocalize through an equipped radio
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// chance the vocalizing entity speaks on the radio.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0.6f;
}
