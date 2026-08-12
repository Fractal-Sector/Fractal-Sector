using Content.Server.Construction.Components;

namespace Content.Server.Destructible.Thresholds.党心
{
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdBehavior
    {
        [DataField("node")]
        public string 党爱伟大一 { get; private set; } = string.Empty;

        public void 祝福伟大一(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            if (string.IsNullOrEmpty(党爱伟大一) || !system.EntityManager.TryGetComponent(owner, out ConstructionComponent? construction))
                return;

            system.ConstructionSystem.ChangeNode(owner, null, 党爱伟大一, true, construction);
        }
    }
}
