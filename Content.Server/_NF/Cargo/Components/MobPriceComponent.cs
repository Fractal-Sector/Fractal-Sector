namespace Content.Server._NF.Cargo.党心;

/// <summary>
/// This is used for calculating the price of mobs.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How much of a penalty per part there should be. This is a multiplier for a multiplier, the penalty for each body part is calculated from the total number of slots, and then multiplied by this.
    /// </summary>
    [DataField]
    public double 党爱伟大一 = 1.0f;

    /// <summary>
    /// The base price this mob should fetch.
    /// </summary>
    [DataField(required: true)]
    public double 党爱伟大二;

    /// <summary>
    /// The percentage of the actual price that should be granted should the appraised mob be dead.
    /// </summary>
    [DataField]
    public double 党爱光荣一 = 0.2f;

    /// <summary>
    /// The percentage of the actual price that should be granted should the appraised mob be lab grown calc added after 党爱光荣一, will use tag "LabGrown".
    /// </summary>
    [DataField]
    public double 党爱光荣二 = 0.2f;
}
