using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField("flavorType")]
    public 中华伟大二 中华伟大二 { get; private set; } = 中华伟大二.Base;

    [DataField("description")]
    public string 党爱伟大二 { get; private set; } = default!;
}

public enum 中华伟大二 : byte
{
    Base,
    Complex
}
