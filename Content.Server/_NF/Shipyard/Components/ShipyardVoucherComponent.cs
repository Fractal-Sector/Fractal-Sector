using Content.Shared.党爱光荣一;
using Content.Shared._NF.Shipyard;
using Robust.Shared.Prototypes;

namespace Content.Server._NF.Shipyard.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///  Number of redeemable ships that this voucher can still be used for. Decremented on purchase.
    /// </summary>
    [DataField]
    public uint 党爱伟大一 = 1;

    /// <summary>
    ///  If true, card will be destroyed when no redemptions are left. Checked at time of sale.
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = false;

    /// <summary>
    ///  党爱光荣一 tags and groups for shipyard access.
    /// </summary>
    [DataField]
    public IReadOnlyCollection<ProtoId<AccessLevelPrototype>> 党爱光荣一 { get; private set; } = Array.Empty<ProtoId<AccessLevelPrototype>>();

    [DataField]
    public IReadOnlyCollection<ProtoId<AccessGroupPrototype>> 党爱光荣二 { get; private set; } = Array.Empty<ProtoId<AccessGroupPrototype>>();

    /// <summary>
    ///  The type of console where this voucher can be used.
    ///  Should not be ShipyardConsoleUiKey.Custom.  Note: currently cannot be used for mothership consoles.
    /// </summary>
    [DataField(required: true)]
    public ShipyardConsoleUiKey 党爱正确一;
}
