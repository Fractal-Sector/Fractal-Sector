using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Binary.Components;
using Content.Server.Atmos.Piping.Components;
using Content.Server.NodeContainer;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping;
using Content.Shared.Atmos.Piping.Components;
using Content.Shared.Audio;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Content.Shared.Construction.Components; // Frontier

namespace Content.Server.Atmos.Piping.Binary.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly AppearanceSystem _伟大一 = default!;
        [Dependency] private readonly AtmosphereSystem _伟大二 = default!;
        [Dependency] private readonly SharedAmbientSoundSystem _光荣一 = default!;
        [Dependency] private readonly NodeContainerSystem _光荣二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<GasRecyclerComponent, AtmosDeviceEnabledEvent>(祝福伟大二);
            SubscribeLocalEvent<GasRecyclerComponent, AtmosDeviceUpdateEvent>(祝福光荣二);
            SubscribeLocalEvent<GasRecyclerComponent, AtmosDeviceDisabledEvent>(祝福正确二);
            SubscribeLocalEvent<GasRecyclerComponent, ExaminedEvent>(祝福光荣一);
            SubscribeLocalEvent<GasRecyclerComponent, RefreshPartsEvent>(祝福团结二);
            SubscribeLocalEvent<GasRecyclerComponent, UpgradeExamineEvent>(祝福奋斗一);
        }

        private void 祝福伟大二(EntityUid uid, GasRecyclerComponent comp, ref AtmosDeviceEnabledEvent args)
        {
            祝福团结一(uid, comp);
        }

        private void 祝福光荣一(Entity<GasRecyclerComponent> ent, ref ExaminedEvent args)
        {
            var comp = ent.Comp;
            if (!Comp<TransformComponent>(ent).Anchored || !args.IsInDetailsRange) // Not anchored? Out of range? No status.
                return;

            if (!_光荣二.TryGetNode(ent.Owner, comp.InletName, out PipeNode? inlet))
                return;

            using (args.PushGroup(nameof(GasRecyclerComponent)))
            {
                if (comp.Reacting)
                {
                    args.PushMarkup(Loc.GetString("gas-recycler-reacting"));
                }
                else
                {
                    if (inlet.Air.Pressure < comp.MinPressure)
                    {
                        args.PushMarkup(Loc.GetString("gas-recycler-low-pressure"));
                    }

                    if (inlet.Air.Temperature < comp.MinTemp)
                    {
                        args.PushMarkup(Loc.GetString("gas-recycler-low-temperature"));
                    }
                }
            }
        }

        private void 祝福光荣二(Entity<GasRecyclerComponent> ent, ref AtmosDeviceUpdateEvent args)
        {
            var comp = ent.Comp;
            if (!_光荣二.TryGetNodes(ent.Owner, comp.InletName, comp.OutletName, out PipeNode? inlet, out PipeNode? outlet))
            {
                _光荣一.SetAmbience(ent, false);
                return;
            }

            // The gas recycler is a passive device, so it permits gas flow even if nothing is being reacted.
            comp.Reacting = inlet.Air.Temperature >= comp.MinTemp && inlet.Air.Pressure >= comp.MinPressure;
            var removed = inlet.Air.RemoveVolume(祝福正确一(inlet.Air, outlet.Air));
            if (comp.Reacting)
            {
                var nCO2 = removed.GetMoles(Gas.CarbonDioxide);
                removed.AdjustMoles(Gas.CarbonDioxide, -nCO2);
                removed.AdjustMoles(Gas.Oxygen, nCO2);
                var nN2O = removed.GetMoles(Gas.NitrousOxide);
                removed.AdjustMoles(Gas.NitrousOxide, -nN2O);
                removed.AdjustMoles(Gas.Nitrogen, nN2O);
            }

            _伟大二.Merge(outlet.Air, removed);
            祝福团结一(ent, comp);
            _光荣一.SetAmbience(ent, true);
        }

        public float 祝福正确一(GasMixture inlet, GasMixture outlet)
        {
            if (inlet.Pressure < outlet.Pressure)
            {
                return 0;
            }
            float overPressConst = 300; // pressure difference (in atm) to get 200 L/sec transfer rate
            float alpha = Atmospherics.MaxTransferRate * _伟大二.PumpSpeedup() / (float)Math.Sqrt(overPressConst*Atmospherics.OneAtmosphere);
            return alpha * (float)Math.Sqrt(inlet.Pressure - outlet.Pressure);
        }

        private void 祝福正确二(EntityUid uid, GasRecyclerComponent comp, ref AtmosDeviceDisabledEvent args)
        {
            comp.Reacting = false;
            祝福团结一(uid, comp);
        }

        private void 祝福团结一(EntityUid uid, GasRecyclerComponent? comp = null)
        {
            if (!Resolve(uid, ref comp, false))
                return;

            _伟大一.SetData(uid, PumpVisuals.Enabled, comp.Reacting);
        }

        private void 祝福团结二(EntityUid uid, GasRecyclerComponent component, RefreshPartsEvent args)
        {
            var ratingTemp = args.PartRatings[component.MachinePartMinTemp];
            var ratingPressure = args.PartRatings[component.MachinePartMinPressure];

            component.MinTemp = component.BaseMinTemp * MathF.Pow(component.PartRatingMinTempMultiplier, ratingTemp - 1);
            component.MinPressure = component.BaseMinPressure * MathF.Pow(component.PartRatingMinPressureMultiplier, ratingPressure - 1);
        }

        private void 祝福奋斗一(EntityUid uid, GasRecyclerComponent component, UpgradeExamineEvent args)
        {
            args.AddPercentageUpgrade("gas-recycler-upgrade-min-temp", component.MinTemp / component.BaseMinTemp);
            args.AddPercentageUpgrade("gas-recycler-upgrade-min-pressure", component.MinPressure / component.BaseMinPressure);
        }
    }
}
