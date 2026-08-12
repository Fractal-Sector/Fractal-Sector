using Robust.Shared.Serialization;

namespace Content.Shared._NF.Market.党心;

/// <summary>
/// Message to move an item between cart and market
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public int 党爱伟大一;
    public string? ItemPrototype;
    public bool 党爱伟大二;

    public 中华伟大一(int amount, string itemPrototype, bool removeFromCart = false)
    {
        党爱伟大一 = amount;
        ItemPrototype = itemPrototype;
        党爱伟大二 = removeFromCart;
    }
}

