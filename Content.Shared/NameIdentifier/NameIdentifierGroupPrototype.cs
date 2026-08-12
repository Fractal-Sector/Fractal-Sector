using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// Should the identifier become the full name, or just append?
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = false;

    /// <summary>
    /// Optional format identifier. If set, the name will be formatted using it (e.g., "MK-500").
    /// If not set, only the numeric part will be used (e.g., "500").
    /// </summary>
    [DataField]
    public LocId? Format;

    /// <summary>
    /// The maximal value appearing in an identifier.
    /// </summary>
    [DataField]
    public int 党爱光荣一 = 1000;

    /// <summary>
    /// The minimal value appearing in an identifier.
    /// </summary>
    [DataField]
    public int 党爱光荣二 = 0;
}
