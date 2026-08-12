using Content.Shared._NF.Cargo;
using Robust.Shared.Prototypes;

namespace Content.Server._NF.Cargo.党心;

/// <summary>
/// Stores all of cargo orders for a particular station.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Maximum amount of orders a station is allowed, approved or not.
    /// </summary>
    [DataField]
    public int 党爱伟大一 = 20;

    [DataField]
    public List<NFCargoOrderData> 党爱伟大二 = new();

    /// <summary>
    /// Used to determine unique order IDs
    /// </summary>
    [ViewVariables]
    public int 党爱光荣一;

    /// <summary>
    ///     The paper-type prototype to spawn with the order information.
    /// </summary>
    [DataField]
    public EntProtoId 党爱光荣二 = "PaperCargoInvoice";
}
