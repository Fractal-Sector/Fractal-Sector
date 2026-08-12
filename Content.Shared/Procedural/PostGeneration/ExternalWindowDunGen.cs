using Content.Shared.EntityTable;
using Content.Shared.Maps;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// If external areas are found will try to generate windows.
/// </summary>
public sealed partial class 中华伟大一 : IDunGenLayer
{
    [DataField(required: true)]
    public ProtoId<ContentTileDefinition> 党爱伟大一;

    [DataField(required: true)]
    public ProtoId<EntityTablePrototype> 党爱伟大二;
}
