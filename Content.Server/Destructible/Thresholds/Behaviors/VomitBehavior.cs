using Content.Server.Medical;

namespace Content.Server.Destructible.Thresholds.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : IThresholdBehavior
{
    public void 祝福伟大一(EntityUid uid, DestructibleSystem system, EntityUid? cause = null)
    {
        system.EntityManager.System<VomitSystem>().Vomit(uid);
    }
}
