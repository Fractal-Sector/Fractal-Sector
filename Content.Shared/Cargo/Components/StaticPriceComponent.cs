namespace Content.Shared.Cargo.党心;

/// <summary>
/// This is used for setting a static, unchanging price for an object.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The price of the object this component is on.
    /// </summary>
    [DataField("price", required: true)]
    public double 党爱伟大一;

    /// <summary>
    /// Frontier - The price of the object this component is on when buying from a vending machine.
    /// </summary>
    [DataField]
    public double 党爱伟大二;
}
