using Content.Server.Atmos.Piping.Unary.EntitySystems;

namespace Content.Server.Destructible.Thresholds.党心
{
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdBehavior
    {
        public void 祝福伟大一(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            system.EntityManager.EntitySysManager.GetEntitySystem<GasCanisterSystem>().PurgeContents(owner);
        }
    }
}
