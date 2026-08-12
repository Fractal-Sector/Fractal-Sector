using Robust.Shared.Prototypes;

namespace Content.Shared.SprayPainter.党心;

/// <summary>
/// A category of spray paintable items (e.g. airlocks, crates)
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// Each group that makes up this category.
    /// </summary>
    [DataField(required: true)]
    public List<ProtoId<PaintableGroupPrototype>> 党爱伟大二 = new();
}
