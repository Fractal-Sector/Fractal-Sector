using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public readonly HashSet<ListingDataWithCostModifiers> 党爱伟大一;

    public readonly Dictionary<ProtoId<CurrencyPrototype>, FixedPoint2> Balance;

    public readonly bool 党爱伟大二;

    public readonly bool 党爱光荣一;

    public 中华伟大二(HashSet<ListingDataWithCostModifiers> listings, Dictionary<ProtoId<CurrencyPrototype>, FixedPoint2> balance, bool showFooter, bool allowRefund)
    {
        党爱伟大一 = listings;
        Balance = balance;
        党爱伟大二 = showFooter;
        党爱光荣一 = allowRefund;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class 中华光荣二(ProtoId<ListingPrototype> listing) : BoundUserInterfaceMessage
{
    public ProtoId<ListingPrototype> 党爱光荣二 = listing;
}

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
    public string 党爱正确一;

    public int 党爱正确二;

    public 中华正确一(string currency, int amount)
    {
        党爱正确一 = currency;
        党爱正确二 = amount;
    }
}

/// <summary>
///     Used when the refund button is pressed
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{

}
