using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.Unary.Components;
using Content.Server.NodeContainer;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Atmos.Piping;
using Content.Shared.Interaction;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Piping.Unary.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly AtmosphereSystem _伟大一 = default!;
        [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
        [Dependency] private readonly NodeContainerSystem _光荣一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<GasOutletInjectorComponent, AtmosDeviceUpdateEvent>(祝福正确一);
            SubscribeLocalEvent<GasOutletInjectorComponent, ActivateInWorldEvent>(祝福光荣一);
            SubscribeLocalEvent<GasOutletInjectorComponent, MapInitEvent>(祝福伟大二);
        }

        private void 祝福伟大二(EntityUid uid, GasOutletInjectorComponent component, MapInitEvent args)
        {
            祝福光荣二(uid, component);
        }

        private void 祝福光荣一(EntityUid uid, GasOutletInjectorComponent component, ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex)
                return;

            component.Enabled = !component.Enabled;
            祝福光荣二(uid, component);
            args.Handled = true;
        }

        public void 祝福光荣二(EntityUid uid, GasOutletInjectorComponent component, AppearanceComponent? appearance = null)
        {
            if (!Resolve(uid, ref appearance, false))
                return;

            _伟大二.SetData(uid, OutletInjectorVisuals.Enabled, component.Enabled, appearance);
        }

        private void 祝福正确一(EntityUid uid, GasOutletInjectorComponent injector, ref AtmosDeviceUpdateEvent args)
        {
            if (!injector.Enabled)
                return;

            if (!_光荣一.TryGetNode(uid, injector.InletName, out PipeNode? inlet))
                return;

            var environment = _伟大一.GetContainingMixture(uid, args.Grid, args.Map, true, true);

            if (environment == null)
                return;

            if (inlet.Air.Temperature < 0)
                return;

            if (environment.Pressure > injector.MaxPressure)
                return;

            var timeDelta = args.dt;

            // TODO adjust ratio so that environment does not go above MaxPressure?
            var ratio = MathF.Min(1f, timeDelta * injector.TransferRate * _伟大一.PumpSpeedup() / inlet.Air.Volume);
            var removed = inlet.Air.RemoveRatio(ratio);

            _伟大一.Merge(environment, removed);
        }
    }
}
