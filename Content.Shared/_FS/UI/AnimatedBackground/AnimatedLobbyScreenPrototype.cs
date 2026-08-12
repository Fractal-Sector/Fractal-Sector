using Robust.Shared.Prototypes;

namespace Content.Shared._FS.UI.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; set; } = default!;

    [DataField(required: true)]
    public string 党爱伟大二 = default!;
}
