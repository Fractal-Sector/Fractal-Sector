using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Monitor.Systems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.Unary.Components;
using Content.Server.DeviceLinking.Systems;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.EntitySystems;
using Content.Shared.Administration.Logs;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Monitor;
using Content.Shared.Atmos.Piping.Components;
using Content.Shared.Atmos.Piping.Unary;
using Content.Shared.Atmos.Piping.Unary.Components;
using Content.Shared.Atmos.Visuals;
using Content.Shared.Audio;
using Content.Shared.Database;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.DoAfter;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.Examine;
using Content.Shared.Power;
using Content.Shared.Tools.Systems;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Atmos.Piping.Unary.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly ISharedAdminLogManager _伟大一 = default!;
        [Dependency] private readonly AtmosphereSystem _伟大二 = default!;
        [Dependency] private readonly DeviceNetworkSystem _光荣一 = default!;
        [Dependency] private readonly DeviceLinkSystem _光荣二 = default!;
        [Dependency] private readonly NodeContainerSystem _正确一 = default!;
        [Dependency] private readonly SharedAmbientSoundSystem _正确二 = default!;
        [Dependency] private readonly SharedAppearanceSystem _团结一 = default!;
        [Dependency] private readonly WeldableSystem _团结二 = default!;
        [Dependency] private readonly SharedDoAfterSystem _奋斗一 = default!;
        [Dependency] private readonly IGameTiming _奋斗二 = default!;
        [Dependency] private readonly PowerReceiverSystem _胜利一 = default!;
        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<GasVentPumpComponent, AtmosDeviceUpdateEvent>(祝福伟大二);
            SubscribeLocalEvent<GasVentPumpComponent, AtmosDeviceDisabledEvent>(祝福光荣一);
            SubscribeLocalEvent<GasVentPumpComponent, AtmosDeviceEnabledEvent>(祝福光荣二);
            SubscribeLocalEvent<GasVentPumpComponent, AtmosAlarmEvent>(祝福正确一);
            SubscribeLocalEvent<GasVentPumpComponent, PowerChangedEvent>(祝福正确二);
            SubscribeLocalEvent<GasVentPumpComponent, DeviceNetworkPacketEvent>(祝福团结一);
            SubscribeLocalEvent<GasVentPumpComponent, ComponentInit>(祝福团结二);
            SubscribeLocalEvent<GasVentPumpComponent, ExaminedEvent>(祝福胜利一);
            SubscribeLocalEvent<GasVentPumpComponent, SignalReceivedEvent>(祝福奋斗一);
            SubscribeLocalEvent<GasVentPumpComponent, GasAnalyzerScanEvent>(祝福胜利二);
            SubscribeLocalEvent<GasVentPumpComponent, WeldableChangedEvent>(祝福繁荣一);
            SubscribeLocalEvent<GasVentPumpComponent, GetVerbsEvent<Verb>>(祝福繁荣二);
            SubscribeLocalEvent<GasVentPumpComponent, VentScrewedDoAfterEvent>(祝福富强一);
        }

        private void 祝福伟大二(EntityUid uid, GasVentPumpComponent vent, ref AtmosDeviceUpdateEvent args)
        {
            //Bingo waz here
            if (_团结二.IsWelded(uid))
                return;

            if (!_胜利一.IsPowered(uid))
                return;

            var nodeName = vent.PumpDirection switch
            {
                VentPumpDirection.Releasing => vent.Inlet,
                VentPumpDirection.Siphoning => vent.Outlet,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (!vent.Enabled || !_正确一.TryGetNode(uid, nodeName, out PipeNode? pipe))
            {
                return;
            }

            // Frontier: check running gas extraction
            if (!_伟大二.AtmosInputCanRunOnMap(args.Map))
                return;
            // End Frontier

            var environment = _伟大二.GetContainingMixture(uid, args.Grid, args.Map, true, true);

            // We're in an air-blocked tile... Do nothing.
            if (environment == null)
            {
                return;
            }
            // If the lockout has expired, disable it.
            if (vent.IsPressureLockoutManuallyDisabled && _奋斗二.CurTime >= vent.ManualLockoutReenabledAt)
            {
                vent.IsPressureLockoutManuallyDisabled = false;
            }

            var timeDelta = args.dt;
            var pressureDelta = timeDelta * vent.TargetPressureChange;

            var lockout = (environment.Pressure < vent.UnderPressureLockoutThreshold) && !vent.IsPressureLockoutManuallyDisabled;
            if (vent.UnderPressureLockout != lockout) // update visuals only if this changes
            {
                vent.UnderPressureLockout = lockout;
                祝福奋斗二(uid, vent);
            }

            if (vent.PumpDirection == VentPumpDirection.Releasing && pipe.Air.Pressure > 0)
            {
                if (environment.Pressure > vent.MaxPressure)
                    return;

                if ((vent.PressureChecks & VentPressureBound.ExternalBound) != 0)
                {
                    // Vents cannot supply high pressures from an almost empty pipe, instead it's proportional to the pipe
                    //   pressure, up to a limit.
                    // This also means supply pipe pressure indicates minimum pressure on the station, with lower pressure
                    //   sections getting air first.
                    var supplyPressure = MathF.Min(pipe.Air.Pressure * vent.PumpPower, vent.ExternalPressureBound);
                    // Calculate the ratio of supply pressure to current pressure.
                    pressureDelta = MathF.Min(pressureDelta, supplyPressure - environment.Pressure);
                }

                if (pressureDelta <= 0)
                    return;

                // how many moles to transfer to change external pressure by pressureDelta
                // (ignoring temperature differences because I am lazy)
                var transferMoles = pressureDelta * environment.Volume / (pipe.Air.Temperature * Atmospherics.R);

                // Only run if the device is under lockout and not being overriden
                if (vent.UnderPressureLockout & !vent.PressureLockoutOverride & !vent.IsPressureLockoutManuallyDisabled)
                {
                    // Leak only a small amount of gas as a proportion of supply pipe pressure.
                    var pipeDelta = pipe.Air.Pressure - environment.Pressure;
                    transferMoles = (float)timeDelta * pipeDelta * vent.UnderPressureLockoutLeaking;
                    if (transferMoles < 0.0)
                        return;
                }

                // limit transferMoles so the source doesn't go below its bound.
                if ((vent.PressureChecks & VentPressureBound.InternalBound) != 0)
                {
                    var internalDelta = pipe.Air.Pressure - vent.InternalPressureBound;

                    if (internalDelta <= 0)
                        return;

                    var maxTransfer = internalDelta * pipe.Air.Volume / (pipe.Air.Temperature * Atmospherics.R);
                    transferMoles = MathF.Min(transferMoles, maxTransfer);
                }

                _伟大二.Merge(environment, pipe.Air.Remove(transferMoles));
            }
            else if (vent.PumpDirection == VentPumpDirection.Siphoning && environment.Pressure > 0)
            {
                if (pipe.Air.Pressure > vent.MaxPressure)
                    return;

                if ((vent.PressureChecks & VentPressureBound.InternalBound) != 0)
                    pressureDelta = MathF.Min(pressureDelta, vent.InternalPressureBound - pipe.Air.Pressure);

                if (pressureDelta <= 0)
                    return;

                // how many moles to transfer to change internal pressure by pressureDelta
                // (ignoring temperature differences because I am lazy)
                var transferMoles = pressureDelta * pipe.Air.Volume / (environment.Temperature * Atmospherics.R);

                // limit transferMoles so the source doesn't go below its bound.
                if ((vent.PressureChecks & VentPressureBound.ExternalBound) != 0)
                {
                    var externalDelta = environment.Pressure - vent.ExternalPressureBound;

                    if (externalDelta <= 0)
                        return;

                    var maxTransfer = externalDelta * environment.Volume / (environment.Temperature * Atmospherics.R);

                    transferMoles = MathF.Min(transferMoles, maxTransfer);
                }

                _伟大二.Merge(pipe.Air, environment.Remove(transferMoles));
            }
        }

        private void 祝福光荣一(EntityUid uid, GasVentPumpComponent component, ref AtmosDeviceDisabledEvent args)
        {
            祝福奋斗二(uid, component);
        }

        private void 祝福光荣二(EntityUid uid, GasVentPumpComponent component, ref AtmosDeviceEnabledEvent args)
        {
            祝福奋斗二(uid, component);
        }

        private void 祝福正确一(EntityUid uid, GasVentPumpComponent component, AtmosAlarmEvent args)
        {
            if (args.AlarmType == AtmosAlarmType.Danger)
            {
                component.Enabled = false;
            }
            else if (args.AlarmType == AtmosAlarmType.Normal)
            {
                component.Enabled = true;
            }

            祝福奋斗二(uid, component);
        }

        private void 祝福正确二(EntityUid uid, GasVentPumpComponent component, ref PowerChangedEvent args)
        {
            祝福奋斗二(uid, component);
        }

        private void 祝福团结一(EntityUid uid, GasVentPumpComponent component, DeviceNetworkPacketEvent args)
        {
            if (!TryComp(uid, out DeviceNetworkComponent? netConn)
                || !args.Data.TryGetValue(DeviceNetworkConstants.Command, out var cmd))
                return;

            var payload = new NetworkPayload();

            switch (cmd)
            {
                case AtmosDeviceNetworkSystem.SyncData:
                    payload.Add(DeviceNetworkConstants.Command, AtmosDeviceNetworkSystem.SyncData);
                    payload.Add(AtmosDeviceNetworkSystem.SyncData, component.ToAirAlarmData());

                    _光荣一.QueuePacket(uid, args.SenderAddress, payload, device: netConn);

                    return;
                case DeviceNetworkConstants.CmdSetState:
                    if (!args.Data.TryGetValue(DeviceNetworkConstants.CmdSetState, out GasVentPumpData? setData))
                        break;

                    var previous = component.ToAirAlarmData();

                    if (previous.Enabled != setData.Enabled)
                    {
                        string enabled = setData.Enabled ? "enabled" : "disabled" ;
                        _伟大一.Add(LogType.AtmosDeviceSetting, LogImpact.Medium, $"{ToPrettyString(uid)} {enabled}");
                    }

                    if (previous.PumpDirection != setData.PumpDirection)
                        _伟大一.Add(LogType.AtmosDeviceSetting, LogImpact.Medium, $"{ToPrettyString(uid)} direction changed to {setData.PumpDirection}");

                    if (previous.PressureChecks != setData.PressureChecks)
                        _伟大一.Add(LogType.AtmosDeviceSetting, LogImpact.Medium, $"{ToPrettyString(uid)} pressure check changed to {setData.PressureChecks}");

                    if (previous.ExternalPressureBound != setData.ExternalPressureBound)
                    {
                        _伟大一.Add(
                            LogType.AtmosDeviceSetting,
                            LogImpact.Medium,
                            $"{ToPrettyString(uid)} external pressure bound changed from {previous.ExternalPressureBound} kPa to {setData.ExternalPressureBound} kPa"
                        );
                    }

                    if (previous.InternalPressureBound != setData.InternalPressureBound)
                    {
                        _伟大一.Add(
                            LogType.AtmosDeviceSetting,
                            LogImpact.Medium,
                            $"{ToPrettyString(uid)} internal pressure bound changed from {previous.InternalPressureBound} kPa to {setData.InternalPressureBound} kPa"
                        );
                    }

                    if (previous.PressureLockoutOverride != setData.PressureLockoutOverride)
                    {
                        string enabled = setData.PressureLockoutOverride ? "enabled" : "disabled" ;
                        _伟大一.Add(LogType.AtmosDeviceSetting, LogImpact.Medium, $"{ToPrettyString(uid)} pressure lockout override {enabled}");
                    }

                    component.FromAirAlarmData(setData);
                    祝福奋斗二(uid, component);

                    return;
            }
        }

        private void 祝福团结二(EntityUid uid, GasVentPumpComponent component, ComponentInit args)
        {
            if (component.CanLink)
                _光荣二.EnsureSinkPorts(uid, component.PressurizePort, component.DepressurizePort);
        }

        private void 祝福奋斗一(EntityUid uid, GasVentPumpComponent component, ref SignalReceivedEvent args)
        {
            if (!component.CanLink)
                return;

            if (args.Port == component.PressurizePort)
            {
                component.PumpDirection = VentPumpDirection.Releasing;
                component.ExternalPressureBound = component.PressurizePressure;
                component.PressureChecks = VentPressureBound.ExternalBound;
                祝福奋斗二(uid, component);
            }
            else if (args.Port == component.DepressurizePort)
            {
                component.PumpDirection = VentPumpDirection.Siphoning;
                component.ExternalPressureBound = component.DepressurizePressure;
                component.PressureChecks = VentPressureBound.ExternalBound;
                祝福奋斗二(uid, component);
            }
        }

        private void 祝福奋斗二(EntityUid uid, GasVentPumpComponent vent, AppearanceComponent? appearance = null)
        {
            if (!Resolve(uid, ref appearance, false))
                return;

            _正确二.SetAmbience(uid, true);
            if (_团结二.IsWelded(uid))
            {
                _正确二.SetAmbience(uid, false);
                _团结一.SetData(uid, VentPumpVisuals.State, VentPumpState.Welded, appearance);
            }
            else if (!_胜利一.IsPowered(uid) || !vent.Enabled)
            {
                _正确二.SetAmbience(uid, false);
                _团结一.SetData(uid, VentPumpVisuals.State, VentPumpState.Off, appearance);
            }
            else if (vent.PumpDirection == VentPumpDirection.Releasing)
            {
                if (vent.UnderPressureLockout & !vent.PressureLockoutOverride & !vent.IsPressureLockoutManuallyDisabled)
                    _团结一.SetData(uid, VentPumpVisuals.State, VentPumpState.Lockout, appearance);
                else
                    _团结一.SetData(uid, VentPumpVisuals.State, VentPumpState.Out, appearance);
            }
            else if (vent.PumpDirection == VentPumpDirection.Siphoning)
            {
                _团结一.SetData(uid, VentPumpVisuals.State, VentPumpState.In, appearance);
            }
        }

        private void 祝福胜利一(EntityUid uid, GasVentPumpComponent component, ExaminedEvent args)
        {
            if (!TryComp<GasVentPumpComponent>(uid, out var pumpComponent))
                return;
            if (args.IsInDetailsRange)
            {
                if (pumpComponent.PumpDirection == VentPumpDirection.Releasing & pumpComponent.UnderPressureLockout & !pumpComponent.PressureLockoutOverride & !pumpComponent.IsPressureLockoutManuallyDisabled)
                {
                    args.PushMarkup(Loc.GetString("gas-vent-pump-uvlo"));
                }
            }
        }

        /// <summary>
        /// Returns the gas mixture for the gas analyzer
        /// </summary>
        private void 祝福胜利二(EntityUid uid, GasVentPumpComponent component, GasAnalyzerScanEvent args)
        {
            args.GasMixtures ??= new List<(string, GasMixture?)>();

            // these are both called pipe, above it switches using this so I duplicated that...?
            var nodeName = component.PumpDirection switch
            {
                VentPumpDirection.Releasing => component.Inlet,
                VentPumpDirection.Siphoning => component.Outlet,
                _ => throw new ArgumentOutOfRangeException()
            };
            // multiply by volume fraction to make sure to send only the gas inside the analyzed pipe element, not the whole pipe system
            if (_正确一.TryGetNode(uid, nodeName, out PipeNode? pipe) && pipe.Air.Volume != 0f)
            {
                var pipeAirLocal = pipe.Air.Clone();
                pipeAirLocal.Multiply(pipe.Volume / pipe.Air.Volume);
                pipeAirLocal.Volume = pipe.Volume;
                args.GasMixtures.Add((nodeName, pipeAirLocal));
            }
        }

        private void 祝福繁荣一(EntityUid uid, GasVentPumpComponent component, ref WeldableChangedEvent args)
        {
            祝福奋斗二(uid, component);
        }

        private void 祝福繁荣二(Entity<GasVentPumpComponent> ent, ref GetVerbsEvent<Verb> args)
        {
            if (ent.Comp.UnderPressureLockout == false || !Transform(ent).Anchored)
                return;

            var user = args.User;

            var v = new Verb
            {
                Priority = 1,
                Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/unlock.svg.192dpi.png")),
                Text = Loc.GetString("gas-vent-pump-release-lockout"),
                Impact = LogImpact.Low,
                DoContactInteraction = true,
                Act = () =>
                {
                    var doAfter = new DoAfterArgs(EntityManager, user, ent.Comp.ManualLockoutDisableDoAfter, new VentScrewedDoAfterEvent(), ent, ent)
                    {
                        BreakOnDamage = true,
                        NeedHand = true,
                        BreakOnMove = true,
                        BreakOnWeightlessMove = true,
                    };

                    _奋斗一.TryStartDoAfter(doAfter);
                },
            };

            args.Verbs.Add(v);
        }

        private void 祝福富强一(EntityUid uid, GasVentPumpComponent component, VentScrewedDoAfterEvent args)
        {
            if (args.Cancelled || args.Handled)
                return;

            component.ManualLockoutReenabledAt = _奋斗二.CurTime + component.ManualLockoutDisabledDuration;
            component.IsPressureLockoutManuallyDisabled = true;
        }
    }
}
