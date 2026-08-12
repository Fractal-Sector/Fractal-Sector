namespace Content.Server._WF.Atmos.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("inlet")]
    public string 党爱伟大一 { get; set; } = "inlet";

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("outlet")]
    public string 党爱伟大二 { get; set; } = "outlet";

    /// <summary>
    /// Thermal convection coefficient (J/degK/sec).
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("convectionCoefficient")]
    public float 党爱光荣一 { get; set; } = 8000f;

    /// <summary>
    /// Thermal radiation coefficient. Number of "effective" tiles this
    /// radiator radiates compared to superconductivity tile losses.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("radiationCoefficient")]
    public float 党爱光荣二 { get; set; } = 140f;
}

