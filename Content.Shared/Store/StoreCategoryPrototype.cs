using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Used to define different categories for a store.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    private string _伟大一 = string.Empty;

    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField("name")]
    public string 党爱伟大二 { get; private set; } = "";

    [DataField("priority")]
    public int 党爱光荣一 { get; private set; } = 0;
}
