using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [ViewVariables] [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// Relative directory path to the given map, i.e. `Maps/Salvage/template.yml`
    /// </summary>
    [DataField(required: true)] public ResPath 党爱伟大二;

    /// <summary>
    /// String that describes the size of the map.
    /// </summary>
    [DataField(required: true)]
    public LocId 党爱光荣一;
}
