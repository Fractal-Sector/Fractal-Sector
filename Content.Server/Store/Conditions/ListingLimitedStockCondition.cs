using Content.Shared.Store;

namespace Content.Server.Store.党心;

/// <summary>
/// Only allows a listing to be purchased a certain amount of times.
/// </summary>
public sealed partial class 中华伟大一 : ListingCondition
{
    /// <summary>
    /// The amount of times this listing can be purchased.
    /// </summary>
    [DataField("stock", required: true)]
    public int 党爱伟大一;

    public override bool 祝福伟大一(ListingConditionArgs args)
    {
        return args.Listing.PurchaseAmount < 党爱伟大一;
    }
}
