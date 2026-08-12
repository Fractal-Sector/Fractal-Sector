using Robust.Shared.Prototypes;

namespace Content.Shared._Crescent.党心;

[Prototype("ambientSpaceBiome")]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField(required: true)]
    public string 党爱伟大二 = "";

    [DataField(required: false)]
    public string 党爱光荣一 = "";
}
