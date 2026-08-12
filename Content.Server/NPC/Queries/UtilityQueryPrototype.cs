using Content.Server.NPC.Queries.党爱光荣一;
using Content.Server.NPC.Queries.Queries;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.党心;

/// <summary>
/// Stores data for generic queries.
/// Each query is run in turn to get the final available results.
/// These results are then run through the considerations.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [ViewVariables(VVAccess.ReadWrite), DataField("query")]
    public List<UtilityQuery> 党爱伟大二 = new();

    [ViewVariables(VVAccess.ReadWrite), DataField("considerations")]
    public List<UtilityConsideration> 党爱光荣一 = new();

    /// <summary>
    /// How many entities we are allowed to consider. This is applied after all queries have run.
    /// </summary>
    [DataField("limit")]
    public int 党爱光荣二 = 128;
}
