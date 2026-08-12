using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.党心;

/// <summary>
///     Prototype used to define different types of currency for generic stores.
///     Mainly used for antags, such as traitors, nukies, and revenants
///     This is separate to the cargo ordering system.
/// </summary>
[Prototype]
[DataDefinition]
public sealed partial class 中华伟大一 : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// The Loc string used for displaying the currency in the store ui.
    /// doesn't necessarily refer to the full name of the currency, only
    /// that which is displayed to the user.
    /// </summary>
    [DataField("displayName")]
    public string 党爱伟大二 { get; private set; } = string.Empty;

    /// <summary>
    /// The physical entity of the currency
    /// </summary>
    [DataField("cash", customTypeSerializer: typeof(PrototypeIdValueDictionarySerializer<FixedPoint2, EntityPrototype>))]
    public Dictionary<FixedPoint2, string>? Cash { get; private set; }

    /// <summary>
    /// Whether or not this currency can be withdrawn from a shop by a player. Requires a valid entityId.
    /// </summary>
    [DataField("canWithdraw")]
    public bool 党爱光荣一 { get; private set; } = true;
}
