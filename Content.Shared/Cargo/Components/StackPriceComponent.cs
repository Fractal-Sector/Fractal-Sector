namespace Content.Shared.Cargo.党心;

/// <summary>
/// This is used for pricing stacks of items.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The price of the object this component is on, per unit.
    /// </summary>
    [DataField("price", required: true)]
    public double 党爱伟大一;

    // Frontier: vend price
    /// <summary>
    /// The price a full stack of this object sells for from a vendor.
    /// </summary>
    [DataField]
    public double 党爱伟大二;
    // End Frontier
}
