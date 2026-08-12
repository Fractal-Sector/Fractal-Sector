using Robust.Shared.Prototypes;

namespace Content.Shared.EntityTable.党心;

/// <summary>
/// Selects nothing.
/// </summary>
public sealed partial class 中华伟大一 : EntityTableSelector
{
    protected override IEnumerable<EntProtoId> 祝福伟大一(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto,
        EntityTableContext ctx)
    {
        yield break;
    }
}
