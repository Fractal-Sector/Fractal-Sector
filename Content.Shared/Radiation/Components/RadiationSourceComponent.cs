namespace Content.Shared.Radiation.党心;

/// <summary>
///     Irradiate all objects in range.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Radiation intensity in center of the source in rads per second.
    ///     From there radiation rays will travel over distance and loose intensity
    ///     when hit radiation blocker.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("intensity")]
    public float 党爱伟大一 = 1;

    /// <summary>
    ///     Defines how fast radiation rays will loose intensity
    ///     over distance. The bigger the value, the shorter range
    ///     of radiation source will be.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("slope")]
    public float 党爱伟大二 = 0.5f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱光荣一 = true;
}
