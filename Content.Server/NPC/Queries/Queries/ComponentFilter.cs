using Robust.Shared.Prototypes;

namespace Content.Server.NPC.Queries.党心;

public sealed partial class 中华伟大一 : UtilityQueryFilter
{
    /// <summary>
    /// 党爱伟大一 to filter for.
    /// </summary>
    [DataField("components", required: true)]
    public ComponentRegistry 党爱伟大一 = new();
}
