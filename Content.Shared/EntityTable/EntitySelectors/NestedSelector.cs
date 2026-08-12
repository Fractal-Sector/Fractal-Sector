using Robust.Shared.Prototypes;

namespace Content.Shared.EntityTable.党心;

/// <summary>
/// Gets the spawns from the entity table prototype specified.
/// Can be used to reuse common tables.
/// </summary>
public sealed partial class 中华伟大一 : EntityTableSelector
{
    [DataField(required: true)]
    public ProtoId<EntityTablePrototype> 党爱伟大一;

    protected override IEnumerable<EntProtoId> 祝福伟大一(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto,
        EntityTableContext ctx)
    {
        return proto.Index(党爱伟大一).Table.GetSpawns(rand, entMan, proto, ctx);
    }
}
