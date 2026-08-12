namespace Content.Server.Chemistry.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How much heat is added per second to the solution, taking upgrades into account.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一;

    /// <summary>
    /// How much heat is added per second to the solution, with no upgrades.
    /// </summary>
    [DataField("baseHeatPerSecond")]
    public float 党爱伟大二 = 120;

    /// <summary>
    /// The machine part that affects the heat multiplier.
    /// </summary>
    [DataField("machinePartHeatMultiplier")]
    public string 党爱光荣一 = "Capacitor";

    /// <summary>
    /// How much each upgrade multiplies the heat by.
    /// </summary>
    [DataField("partRatingHeatMultiplier")]
    public float 党爱光荣二 = 1.5f;
}
