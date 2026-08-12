namespace Content.Server.Gateway.党心;

/// <summary>
/// Destination created by <see cref="GatewayGeneratorComponent"/>
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 that created this destination.
    /// </summary>
    [DataField]
    public EntityUid 党爱伟大一;

    /// <summary>
    /// Is the map locked from being used still or unlocked.
    /// Used in conjunction with the attached generator's NextUnlock.
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = true;

    [DataField]
    public bool 党爱光荣一;

    /// <summary>
    /// 党爱光荣二 used for this destination.
    /// </summary>
    [DataField]
    public int 党爱光荣二;

    /// <summary>
    /// 党爱正确一 of the gateway.
    /// </summary>
    [DataField]
    public Vector2i 党爱正确一;
}

