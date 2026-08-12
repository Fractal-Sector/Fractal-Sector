using Content.Shared.EntityTable;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// Adds entities randomly to the corridors.
/// </summary>
public sealed partial class 中华伟大一 : IDunGenLayer
{
    [DataField]
    public float 党爱伟大一 = 0.05f;

    /// <summary>
    /// The default starting bulbs
    /// </summary>
    [DataField(required: true)]
    public ProtoId<EntityTablePrototype> 党爱伟大二;
}
