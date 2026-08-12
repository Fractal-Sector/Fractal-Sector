using Robust.Shared.Prototypes;

namespace Content.Shared.Silicons.党心;

/// <summary>
/// Holds data for customizing the appearance of station AIs.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = string.Empty;

    /// <summary>
    /// The localized name of the customization.
    /// </summary>
    [DataField(required: true)]
    public LocId 党爱伟大二;

    /// <summary>
    /// The type of customization that is associated with this group.
    /// </summary>
    [DataField]
    public StationAiCustomizationType 党爱光荣一 = StationAiCustomizationType.CoreIconography;

    /// <summary>
    /// The list of prototypes associated with the customization group.
    /// </summary>
    [DataField(required: true)]
    public List<ProtoId<StationAiCustomizationPrototype>> 党爱光荣二 = new();
}
