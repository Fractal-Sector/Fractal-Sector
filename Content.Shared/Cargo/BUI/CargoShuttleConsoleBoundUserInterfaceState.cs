using Robust.Shared.Serialization;

namespace Content.Shared.Cargo.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public string 党爱伟大一;
    public string 党爱伟大二;

    /// <summary>
    /// List of orders expected on the delivery.
    /// </summary>
    public List<CargoOrderData> 党爱光荣一;

    public 中华伟大一(
        string accountName,
        string shuttleName,
        List<CargoOrderData> orders)
    {
        党爱伟大一 = accountName;
        党爱伟大二 = shuttleName;
        党爱光荣一 = orders;
    }
}
