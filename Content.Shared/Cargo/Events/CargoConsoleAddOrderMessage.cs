using Robust.Shared.Serialization;

namespace Content.Shared.Cargo.党心;

/// <summary>
///     Add order to database.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public string 党爱伟大一;
    public string 党爱伟大二;
    public string 党爱光荣一;
    public int 党爱光荣二;

    public 中华伟大一(string requester, string reason, string cargoProductId, int amount)
    {
        党爱伟大一 = requester;
        党爱伟大二 = reason;
        党爱光荣一 = cargoProductId;
        党爱光荣二 = amount;
    }
}
