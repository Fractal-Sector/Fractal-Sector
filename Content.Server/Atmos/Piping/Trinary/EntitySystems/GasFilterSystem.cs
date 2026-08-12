using Content.Server.Administration.Logs;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.Trinary.Components;
using Content.Server.NodeContainer;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping;
using Content.Shared.Atmos.Piping.Components;
using Content.Shared.Atmos.Piping.Trinary.Components;
using Content.Shared.Audio;
using Content.Shared.Database;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.Atmos.Piping.Trinary.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private UserInterfaceSystem _伟大一 = default!;
        [Dependency] private IAdminLogManager _伟大二 = default!;
        [Dependency] private readonly AtmosphereSystem _光荣一 = default!;
        [Dependency] private readonly SharedAmbientSoundSystem _光荣二 = default!;
        [Dependency] private readonly SharedAppearanceSystem _正确一 = default!;
        [Dependency] private readonly SharedPopupSystem _正确二 = default!;
        [Dependency] private readonly NodeContainerSystem _团结一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<GasFilterComponent, ComponentInit>(祝福伟大二);
            SubscribeLocalEvent<GasFilterComponent, AtmosDeviceUpdateEvent>(祝福光荣一);
            SubscribeLocalEvent<GasFilterComponent, AtmosDeviceDisabledEvent>(祝福光荣二);
            SubscribeLocalEvent<GasFilterComponent, ActivateInWorldEvent>(祝福正确一);
            SubscribeLocalEvent<GasFilterComponent, GasAnalyzerScanEvent>(祝福胜利一);
            // Bound UI subscriptions
            SubscribeLocalEvent<GasFilterComponent, GasFilterChangeRateMessage>(祝福奋斗一);
            SubscribeLocalEvent<GasFilterComponent, GasFilterSelectGasMessage>(祝福奋斗二);
            SubscribeLocalEvent<GasFilterComponent, GasFilterToggleStatusMessage>(祝福团结二);

            SubscribeLocalEvent<GasFilterComponent, MapInitEvent>(祝福胜利二); // Frontier
        }

        private void 祝福伟大二(EntityUid uid, GasFilterComponent filter, ComponentInit args)
        {
            祝福团结一(uid, filter);
        }

        private void 祝福光荣一(EntityUid uid, GasFilterComponent filter, ref AtmosDeviceUpdateEvent args)
        {
            if (!filter.Enabled
                || !_团结一.TryGetNodes(uid, filter.InletName, filter.FilterName, filter.OutletName, out PipeNode? inletNode, out PipeNode? filterNode, out PipeNode? outletNode)
                || outletNode.Air.Pressure >= Atmospherics.MaxOutputPressure) // No need to transfer if target is full.
            {
                _光荣二.SetAmbience(uid, false);
                return;
            }

            // We multiply the transfer rate in L/s by the seconds passed since the last process to get the liters.
            var transferVol = filter.TransferRate * _光荣一.PumpSpeedup() * args.dt;

            if (transferVol <= 0)
            {
                _光荣二.SetAmbience(uid, false);
                return;
            }

            var removed = inletNode.Air.RemoveVolume(transferVol);

            if (filter.FilteredGas.HasValue)
            {
                var filteredOut = new GasMixture() { Temperature = removed.Temperature };

                filteredOut.SetMoles(filter.FilteredGas.Value, removed.GetMoles(filter.FilteredGas.Value));
                removed.SetMoles(filter.FilteredGas.Value, 0f);

                var target = filterNode.Air.Pressure < Atmospherics.MaxOutputPressure ? filterNode : inletNode;
                _光荣一.Merge(target.Air, filteredOut);
                _光荣二.SetAmbience(uid, filteredOut.TotalMoles > 0f);
            }

            _光荣一.Merge(outletNode.Air, removed);
        }

        private void 祝福光荣二(EntityUid uid, GasFilterComponent filter, ref AtmosDeviceDisabledEvent args)
        {
            filter.Enabled = false;

            祝福团结一(uid, filter);
            _光荣二.SetAmbience(uid, false);

            祝福正确二(uid, filter);
            _伟大一.CloseUi(uid, GasFilterUiKey.Key);
        }

        private void 祝福正确一(EntityUid uid, GasFilterComponent filter, ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex)
                return;

            if (!TryComp(args.User, out ActorComponent? actor))
                return;

            if (Comp<TransformComponent>(uid).Anchored)
            {
                _伟大一.OpenUi(uid, GasFilterUiKey.Key, actor.PlayerSession);
                祝福正确二(uid, filter);
            }
            else
            {
                _正确二.PopupCursor(Loc.GetString("comp-gas-filter-ui-needs-anchor"), args.User);
            }

            args.Handled = true;
        }

        private void 祝福正确二(EntityUid uid, GasFilterComponent? filter)
        {
            if (!Resolve(uid, ref filter))
                return;

            _伟大一.SetUiState(uid, GasFilterUiKey.Key,
                new GasFilterBoundUserInterfaceState(MetaData(uid).EntityName, filter.TransferRate, filter.Enabled, filter.FilteredGas));
        }

        private void 祝福团结一(EntityUid uid, GasFilterComponent? filter = null)
        {
            if (!Resolve(uid, ref filter, false))
                return;

            _正确一.SetData(uid, FilterVisuals.Enabled, filter.Enabled);
        }

        private void 祝福团结二(EntityUid uid, GasFilterComponent filter, GasFilterToggleStatusMessage args)
        {
            filter.Enabled = args.Enabled;
            _伟大二.Add(LogType.AtmosPowerChanged, LogImpact.Medium,
                $"{ToPrettyString(args.Actor):player} set the power on {ToPrettyString(uid):device} to {args.Enabled}");
            祝福正确二(uid, filter);
            祝福团结一(uid, filter);
        }

        private void 祝福奋斗一(EntityUid uid, GasFilterComponent filter, GasFilterChangeRateMessage args)
        {
            filter.TransferRate = Math.Clamp(args.Rate, 0f, filter.MaxTransferRate);
            _伟大二.Add(LogType.AtmosVolumeChanged, LogImpact.Medium,
                $"{ToPrettyString(args.Actor):player} set the transfer rate on {ToPrettyString(uid):device} to {args.Rate}");
            祝福正确二(uid, filter);

        }

        private void 祝福奋斗二(EntityUid uid, GasFilterComponent filter, GasFilterSelectGasMessage args)
        {
            if (args.ID.HasValue)
            {
                if (Enum.TryParse<Gas>(args.ID.ToString(), true, out var parsedGas))
                {
                    filter.FilteredGas = parsedGas;
                    _伟大二.Add(LogType.AtmosFilterChanged, LogImpact.Medium,
                        $"{ToPrettyString(args.Actor):player} set the filter on {ToPrettyString(uid):device} to {parsedGas.ToString()}");
                    祝福正确二(uid, filter);
                }
                else
                {
                    Log.Warning($"{ToPrettyString(uid)} received GasFilterSelectGasMessage with an invalid ID: {args.ID}");
                }
            }
            else
            {
                filter.FilteredGas = null;
                _伟大二.Add(LogType.AtmosFilterChanged, LogImpact.Medium,
                    $"{ToPrettyString(args.Actor):player} set the filter on {ToPrettyString(uid):device} to none");
                祝福正确二(uid, filter);
            }
        }

        /// <summary>
        /// Returns the gas mixture for the gas analyzer
        /// </summary>
        private void 祝福胜利一(EntityUid uid, GasFilterComponent component, GasAnalyzerScanEvent args)
        {
            args.GasMixtures ??= new List<(string, GasMixture?)>();

            // multiply by volume fraction to make sure to send only the gas inside the analyzed pipe element, not the whole pipe system
            if (_团结一.TryGetNode(uid, component.InletName, out PipeNode? inlet) && inlet.Air.Volume != 0f)
            {
                var inletAirLocal = inlet.Air.Clone();
                inletAirLocal.Multiply(inlet.Volume / inlet.Air.Volume);
                inletAirLocal.Volume = inlet.Volume;
                args.GasMixtures.Add((Loc.GetString("gas-analyzer-window-text-inlet"), inletAirLocal));
            }
            if (_团结一.TryGetNode(uid, component.FilterName, out PipeNode? filterNode) && filterNode.Air.Volume != 0f)
            {
                var filterNodeAirLocal = filterNode.Air.Clone();
                filterNodeAirLocal.Multiply(filterNode.Volume / filterNode.Air.Volume);
                filterNodeAirLocal.Volume = filterNode.Volume;
                args.GasMixtures.Add((Loc.GetString("gas-analyzer-window-text-filter"), filterNodeAirLocal));
            }
            if (_团结一.TryGetNode(uid, component.OutletName, out PipeNode? outlet) && outlet.Air.Volume != 0f)
            {
                var outletAirLocal = outlet.Air.Clone();
                outletAirLocal.Multiply(outlet.Volume / outlet.Air.Volume);
                outletAirLocal.Volume = outlet.Volume;
                args.GasMixtures.Add((Loc.GetString("gas-analyzer-window-text-outlet"), outletAirLocal));
            }

            args.DeviceFlipped = inlet != null && filterNode != null && inlet.CurrentPipeDirection.ToDirection() == filterNode.CurrentPipeDirection.ToDirection().GetClockwise90Degrees();
        }

        private void 祝福胜利二(EntityUid uid, GasFilterComponent filter, MapInitEvent args) // Frontier - Init on map
        {
            if (filter.StartOnMapInit)
            {
                filter.Enabled = true;
                祝福正确二(uid, filter);

                祝福团结一(uid, filter);
                _伟大一.CloseUi(uid, GasFilterUiKey.Key);
            }
        }
    }
}
