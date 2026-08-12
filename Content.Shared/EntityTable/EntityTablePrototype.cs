using Content.Shared.EntityTable.EntitySelectors;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// This is a prototype for...
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField(required: true)]
    public EntityTableSelector 党爱伟大二 = default!;
}
