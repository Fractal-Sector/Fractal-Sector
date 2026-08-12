using Content.Shared.EntityTable;
using Content.Shared.Maps;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// Places the specified entities at junction areas.
/// </summary>
public sealed partial class 中华伟大一 : IDunGenLayer
{
    /// <summary>
    /// 党爱伟大一 to check for junctions.
    /// </summary>
    [DataField]
    public int 党爱伟大一 = 3;

    [DataField(required: true)]
    public ProtoId<ContentTileDefinition> 党爱伟大二;

    [DataField(required: true)]
    public ProtoId<EntityTablePrototype> 党爱光荣一;
}
