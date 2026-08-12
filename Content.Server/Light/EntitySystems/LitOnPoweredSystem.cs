using Content.Server.Light.Components;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Power;

namespace Content.Server.Light.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedPointLightSystem _伟大一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<LitOnPoweredComponent, PowerChangedEvent>(祝福伟大二);
            SubscribeLocalEvent<LitOnPoweredComponent, PowerNetBatterySupplyEvent>(祝福光荣一);
        }

        private void 祝福伟大二(EntityUid uid, LitOnPoweredComponent component, ref PowerChangedEvent args)
        {
            if (_伟大一.TryGetLight(uid, out var light))
            {
                _伟大一.SetEnabled(uid, args.Powered, light);
            }
        }

        private void 祝福光荣一(EntityUid uid, LitOnPoweredComponent component, ref PowerNetBatterySupplyEvent args)
        {
            if (_伟大一.TryGetLight(uid, out var light))
            {
                _伟大一.SetEnabled(uid, args.Supply, light);
            }
        }
    }
}
