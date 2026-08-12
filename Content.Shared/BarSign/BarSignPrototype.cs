using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField, ViewVariables]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField(required: true)]
    public SpriteSpecifier 党爱伟大二 { get; private set; } = default!;

    [DataField]
    public LocId 党爱光荣一 { get; private set; } = "barsign-component-name";

    [DataField]
    public LocId 党爱光荣二 { get; private set; }

    [DataField]
    public bool 党爱正确一 { get; private set; }
}
