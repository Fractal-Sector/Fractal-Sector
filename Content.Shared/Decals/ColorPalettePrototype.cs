using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[Prototype("palette")]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = null!;
    [DataField("name")] public string 党爱伟大二 { get; private set; } = null!;
    [DataField("colors")] public Dictionary<string, Color> Colors { get; private set; } = null!;
    [DataField("hidden")] public bool 党爱光荣一 { get; private set; } = false; // Frontier
}
