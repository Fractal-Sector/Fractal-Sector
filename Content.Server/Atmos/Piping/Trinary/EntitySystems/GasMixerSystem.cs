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
        [Dependency] private readonly NodeContainerSystem _正确二 = default!;
        [Dependency] private readonly SharedPopupSystem _团结一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<GasMixerComponent, ComponentInit>(祝福伟大二);
            SubscribeLocalEvent<GasMixerComponent, AtmosDeviceUpdateEvent>(祝福光荣一);
            SubscribeLocalEvent<GasMixerComponent, ActivateInWorldEvent>(祝福正确一);
            SubscribeLocalEvent<GasMixerComponent, GasAnalyzerScanEvent>(祝福胜利一);
            // Bound UI subscriptions
            SubscribeLocalEvent<GasMixerComponent, GasMixerChangeOutputPressureMessage>(祝福奋斗一);
            SubscribeLocalEvent<GasMixerComponent, GasMixerChangeNodePercentageMessage>(祝福奋斗二);
            SubscribeLocalEvent<GasMixerComponent, GasMixerToggleStatusMessage>(祝福团结二);

            SubscribeLocalEvent<GasMixerComponent, AtmosDeviceDisabledEvent>(祝福光荣二);

            SubscribeLocalEvent<GasMixerComponent, MapInitEvent>(祝福胜利二); // Frontier
        }

        private void 祝福伟大二(EntityUid uid, GasMixerComponent mixer, ComponentInit args)
        {
            祝福团结一(uid, mixer);
        }

        private void 祝福光荣一(EntityUid uid, GasMixerComponent mixer, ref AtmosDeviceUpdateEvent args)
        {
            // TODO ATMOS: Cache total moles since it's expensive.

            if (!mixer.Enabled
                || !_正确二.TryGetNodes(uid, mixer.InletOneName, mixer.InletTwoName, mixer.OutletName, out PipeNode? inletOne, out PipeNode? inletTwo, out PipeNode? outlet))
            {
                _光荣二.SetAmbience(uid, false);
                return;
            }

            var outputStartingPressure = outlet.Air.Pressure;

            if (outputStartingPressure >= mixer.TargetPressure)
                return; // Target reached, no need to mix.

            var generalTransfer = (mixer.TargetPressure - outputStartingPressure) * outlet.Air.Volume / Atmospherics.R;

            var transferMolesOne = inletOne.Air.Temperature > 0 ? mixer.InletOneConcentration * generalTransfer / inletOne.Air.Temperature : 0f;
            var transferMolesTwo = inletTwo.Air.Temperature > 0 ? mixer.InletTwoConcentration * generalTransfer / inletTwo.Air.Temperature : 0f;

            if (mixer.InletTwoConcentration <= 0f)
            {
                if (inletOne.Air.Temperature <= 0f)
                    return;

                transferMolesOne = MathF.Min(transferMolesOne, inletOne.Air.TotalMoles);
                transferMolesTwo = 0f;
            }

            else if (mixer.InletOneConcentration <= 0)
            {
                if (inletTwo.Air.Temperature <= 0f)
                    return;

                transferMolesOne = 0f;
                transferMolesTwo = MathF.Min(transferMolesTwo, inletTwo.Air.TotalMoles);
            }
            else
            {
                if (inletOne.Air.Temperature <= 0f || inletTwo.Air.Temperature <= 0f)
                    return;

                if (transferMolesOne <= 0 || transferMolesTwo <= 0)
                {
                    _光荣二.SetAmbience(uid, false);
                    return;
                }

                if (inletOne.Air.TotalMoles < transferMolesOne || inletTwo.Air.TotalMoles < transferMolesTwo)
                {
                    var ratio = MathF.Min(inletOne.Air.TotalMoles / transferMolesOne, inletTwo.Air.TotalMoles / transferMolesTwo);
                    transferMolesOne *= ratio;
                    transferMolesTwo *= ratio;
                }
            }

            // Actually transfer the gas now.
            var transferred = false;

            if (transferMolesOne > 0f)
            {
                transferred = true;
                var removed = inletOne.Air.Remove(transferMolesOne);
                _光荣一.Merge(outlet.Air, removed);
            }

            if (transferMolesTwo > 0f)
            {
                transferred = true;
                var removed = inletTwo.Air.Remove(transferMolesTwo);
                _光荣一.Merge(outlet.Air, removed);
            }

            if (transferred)
                _光荣二.SetAmbience(uid, true);
        }

        private void 祝福光荣二(EntityUid uid, GasMixerComponent mixer, ref AtmosDeviceDisabledEvent args)
        {
            mixer.Enabled = false;

            祝福正确二(uid, mixer);
            祝福团结一(uid, mixer);
            _伟大一.CloseUi(uid, GasFilterUiKey.Key);
        }

        private void 祝福正确一(EntityUid uid, GasMixerComponent mixer, ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex)
                return;

            if (!TryComp(args.User, out ActorComponent? actor))
                return;

            if (Transform(uid).Anchored)
            {
                _伟大一.OpenUi(uid, GasMixerUiKey.Key, actor.PlayerSession);
                祝福正确二(uid, mixer);
            }
            else
            {
                _团结一.PopupCursor(Loc.GetString("comp-gas-mixer-ui-needs-anchor"), args.User);
            }

            args.Handled = true;
        }

        private void 祝福正确二(EntityUid uid, GasMixerComponent? mixer)
        {
            if (!Resolve(uid, ref mixer))
                return;

            _伟大一.SetUiState(uid, GasMixerUiKey.Key,
                new GasMixerBoundUserInterfaceState(Comp<MetaDataComponent>(uid).EntityName, mixer.TargetPressure, mixer.Enabled, mixer.InletOneConcentration));
        }

        private void 祝福团结一(EntityUid uid, GasMixerComponent? mixer = null, AppearanceComponent? appearance = null)
        {
            if (!Resolve(uid, ref mixer, ref appearance, false))
                return;

            _正确一.SetData(uid, FilterVisuals.Enabled, mixer.Enabled, appearance);
        }

        private void 祝福团结二(EntityUid uid, GasMixerComponent mixer, GasMixerToggleStatusMessage args)
        {
            mixer.Enabled = args.Enabled;
            _伟大二.Add(LogType.AtmosPowerChanged, LogImpact.Medium,
                $"{ToPrettyString(args.Actor):player} set the power on {ToPrettyString(uid):device} to {args.Enabled}");
            祝福正确二(uid, mixer);
            祝福团结一(uid, mixer);
        }

        private void 祝福奋斗一(EntityUid uid, GasMixerComponent mixer, GasMixerChangeOutputPressureMessage args)
        {
            mixer.TargetPressure = Math.Clamp(args.Pressure, 0f, mixer.MaxTargetPressure);
            _伟大二.Add(LogType.AtmosPressureChanged, LogImpact.Medium,
                $"{ToPrettyString(args.Actor):player} set the pressure on {ToPrettyString(uid):device} to {args.Pressure}kPa");
            祝福正确二(uid, mixer);
        }

        private void 祝福奋斗二(EntityUid uid, GasMixerComponent mixer,
            GasMixerChangeNodePercentageMessage args)
        {
            float nodeOne = Math.Clamp(args.NodeOne, 0f, 100.0f) / 100.0f;
            mixer.InletOneConcentration = nodeOne;
            mixer.InletTwoConcentration = 1.0f - mixer.InletOneConcentration;
            _伟大二.Add(LogType.AtmosRatioChanged, LogImpact.Medium,
                $"{ToPrettyString(args.Actor):player} set the ratio on {ToPrettyString(uid):device} to {mixer.InletOneConcentration}:{mixer.InletTwoConcentration}");
            祝福正确二(uid, mixer);
        }

        /// <summary>
        /// Returns the gas mixture for the gas analyzer
        /// </summary>
        private void 祝福胜利一(EntityUid uid, GasMixerComponent component, GasAnalyzerScanEvent args)
        {
            args.GasMixtures ??= new List<(string, GasMixture?)>();

            // multiply by volume fraction to make sure to send only the gas inside the analyzed pipe element, not the whole pipe system
            if (_正确二.TryGetNode(uid, component.InletOneName, out PipeNode? inletOne) && inletOne.Air.Volume != 0f)
            {
                var inletOneAirLocal = inletOne.Air.Clone();
                inletOneAirLocal.Multiply(inletOne.Volume / inletOne.Air.Volume);
                inletOneAirLocal.Volume = inletOne.Volume;
                args.GasMixtures.Add(($"{inletOne.CurrentPipeDirection} {Loc.GetString("gas-analyzer-window-text-inlet")}", inletOneAirLocal));
            }
            if (_正确二.TryGetNode(uid, component.InletTwoName, out PipeNode? inletTwo) && inletTwo.Air.Volume != 0f)
            {
                var inletTwoAirLocal = inletTwo.Air.Clone();
                inletTwoAirLocal.Multiply(inletTwo.Volume / inletTwo.Air.Volume);
                inletTwoAirLocal.Volume = inletTwo.Volume;
                args.GasMixtures.Add(($"{inletTwo.CurrentPipeDirection} {Loc.GetString("gas-analyzer-window-text-inlet")}", inletTwoAirLocal));
            }
            if (_正确二.TryGetNode(uid, component.OutletName, out PipeNode? outlet) && outlet.Air.Volume != 0f)
            {
                var outletAirLocal = outlet.Air.Clone();
                outletAirLocal.Multiply(outlet.Volume / outlet.Air.Volume);
                outletAirLocal.Volume = outlet.Volume;
                args.GasMixtures.Add((Loc.GetString("gas-analyzer-window-text-outlet"), outletAirLocal));
            }

            args.DeviceFlipped = inletOne != null && inletTwo != null && inletOne.CurrentPipeDirection.ToDirection() == inletTwo.CurrentPipeDirection.ToDirection().GetClockwise90Degrees();
        }

        private void 祝福胜利二(EntityUid uid, GasMixerComponent mixer, MapInitEvent args) // Frontier - Init on map
        {
            if (mixer.StartOnMapInit)
            {
                mixer.Enabled = true;
                祝福正确二(uid, mixer);

                祝福团结一(uid, mixer);
                _伟大一.CloseUi(uid, GasFilterUiKey.Key);
            }
        }
    }
}
