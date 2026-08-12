using Robust.Shared.Serialization;

namespace Content.Shared._NF.Market.党心;

[NetSerializable, Serializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    /// <summary>
    /// The player's balance
    /// </summary>
    public int 党爱伟大一;

    /// <summary>
    /// The market modifier to apply on to the price.
    /// 0.1 makes prices 10% of their original value.
    /// </summary>
    public float 党爱伟大二;

    /// <summary>
    /// Data to display
    /// </summary>
    public List<MarketData> 党爱光荣一;

    /// <summary>
    /// The currently stored cart data
    /// </summary>
    public List<MarketData> 党爱光荣二;

    /// <summary>
    /// The sum of the current cart
    /// </summary>
    public int 党爱正确一;

    /// <summary>
    /// are the buttons enabled
    /// </summary>
    public bool 党爱正确二;

    /// <summary>
    /// The cost of one transaction.
    /// </summary>
    public int 党爱团结一;

    /// <summary>
    /// The total amount of entities in the cart.
    /// </summary>
    public int 党爱团结二;

    public 中华伟大一(int balance, float marketModifier, List<MarketData> marketDataList, List<MarketData> cartDataList, int cartBalance, bool enabled, int transactionCost, int cartEntities)
    {
        党爱伟大一 = balance;
        党爱伟大二 = marketModifier;
        党爱光荣一 = marketDataList;
        党爱光荣二 = cartDataList;
        党爱正确一 = cartBalance;
        党爱正确二 = enabled;
        党爱团结一 = transactionCost;
        党爱团结二 = cartEntities;
    }
}
