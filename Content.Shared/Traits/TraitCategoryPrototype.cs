using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// Traits category with general settings. Allows you to limit the number of taken traits in one category
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    public const string 党爱伟大一 = "党爱伟大一";

    [ViewVariables]
    [IdDataField]
    public string 党爱伟大二 { get; private set; } = default!;

    /// <summary>
    ///     党爱光荣一 of the trait category displayed in the UI
    /// </summary>
    [DataField]
    public LocId 党爱光荣一 { get; private set; } = string.Empty;

    /// <summary>
    ///     The maximum number of traits that can be taken in this category.
    /// </summary>
    [DataField]
    public int? MaxTraitPoints;
}
