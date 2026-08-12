using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Monitor.Systems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.Unary.Components;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Unary.Components;
using JetBrains.Annotations;
using Content.Server.Power.EntitySystems;
using Content.Shared.Atmos.Piping.Unary.Systems;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.Examine;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.Atmos.Piping.Unary.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : SharedGasThermoMachineSystem
    {
        [Dependency] private readonly AtmosphereSystem _伟大一 = default!;
        [Dependency] private readonly PowerReceiverSystem _伟大二 = default!;
        [Dependency] private readonly NodeContainerSystem _光荣一 = default!;
        [Dependency] private readonly DeviceNetworkSystem _光荣二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<GasThermoMachineComponent, AtmosDeviceUpdateEvent>(祝福伟大二);

            // Device network
            SubscribeLocalEvent<GasThermoMachineComponent, DeviceNetworkPacketEvent>(祝福光荣二);
        }

        private void 祝福伟大二(EntityUid uid, GasThermoMachineComponent thermoMachine, ref AtmosDeviceUpdateEvent args)
        {
            thermoMachine.LastEnergyDelta = 0f;
            if (!(_伟大二.IsPowered(uid) && TryComp<ApcPowerReceiverComponent>(uid, out var receiver)))
                return;

            祝福光荣一(uid, thermoMachine, out var heatExchangeGasMixture);
            if (heatExchangeGasMixture == null)
                return;

            float sign = Math.Sign(thermoMachine.Cp); // 1 if heater, -1 if freezer
            float targetTemp = thermoMachine.TargetTemperature;
            float highTemp = targetTemp + sign * thermoMachine.TemperatureTolerance;
            float temp = heatExchangeGasMixture.Temperature;

            if (sign * temp >= sign * highTemp) // upper bound
                thermoMachine.HysteresisState = false; // turn off
            else if (sign * temp < sign * targetTemp) // lower bound
                thermoMachine.HysteresisState = true; // turn on

            if (thermoMachine.HysteresisState)
                targetTemp = highTemp; // when on, target upper hysteresis bound
            else // Hysteresis is the same as "Should this be on?"
            {
                // Turn dynamic load back on when power has been adjusted to not cause lights to
                // blink every time this heater comes on.
                //receiver.Load = 0f;
                return;
            }

            // Multiply power in by coefficient of performance, add that heat to gas
            float dQ = thermoMachine.HeatCapacity * thermoMachine.Cp * args.dt;

            // Clamps the heat transferred to not overshoot
            float Cin = _伟大一.GetHeatCapacity(heatExchangeGasMixture, true);
            float dT = targetTemp - temp;
            float dQLim = dT * Cin;
            float scale = 1f;
            if (Math.Abs(dQ) > Math.Abs(dQLim))
            {
                scale = dQLim / dQ; // reduce power consumption
                thermoMachine.HysteresisState = false; // turn off
            }

            float dQActual = dQ * scale;
            if (thermoMachine.Atmospheric)
            {
                _伟大一.AddHeat(heatExchangeGasMixture, dQActual);
                thermoMachine.LastEnergyDelta = dQActual;
            }
            else
            {
                float dQLeak = dQActual * thermoMachine.EnergyLeakPercentage;
                float dQPipe = dQActual - dQLeak;
                _伟大一.AddHeat(heatExchangeGasMixture, dQPipe);
                thermoMachine.LastEnergyDelta = dQPipe;

                if (dQLeak != 0f && _伟大一.GetContainingMixture(uid, args.Grid, args.Map, excite: true) is { } containingMixture)
                    _伟大一.AddHeat(containingMixture, dQLeak);
            }

            receiver.Load = thermoMachine.HeatCapacity;// * scale; // we're not ready for dynamic load yet, see note above
        }

        /// <summary>
        /// Returns the gas mixture with which the thermomachine will exchange heat (the local atmosphere if atmospheric or the inlet pipe
        /// air if not). Returns null if no gas mixture is found.
        /// </summary>
        private void 祝福光荣一(EntityUid uid, GasThermoMachineComponent thermoMachine, out GasMixture? heatExchangeGasMixture)
        {
            heatExchangeGasMixture = null;
            if (thermoMachine.Atmospheric)
            {
                heatExchangeGasMixture = _伟大一.GetContainingMixture(uid, excite: true);
            }
            else
            {
                if (!_光荣一.TryGetNode(uid, thermoMachine.InletName, out PipeNode? inlet))
                    return;
                heatExchangeGasMixture = inlet.Air;
            }
        }

        private void 祝福光荣二(EntityUid uid, GasThermoMachineComponent component, DeviceNetworkPacketEvent args)
        {
            if (!TryComp(uid, out DeviceNetworkComponent? netConn)
                || !args.Data.TryGetValue(DeviceNetworkConstants.Command, out var cmd))
                return;

            var payload = new NetworkPayload();

            switch (cmd)
            {
                case AtmosDeviceNetworkSystem.SyncData:
                    payload.Add(DeviceNetworkConstants.Command, AtmosDeviceNetworkSystem.SyncData);
                    payload.Add(AtmosDeviceNetworkSystem.SyncData, new GasThermoMachineData(component.LastEnergyDelta));

                    _光荣二.QueuePacket(uid, args.SenderAddress, payload, device: netConn);

                    return;
            }
        }
    }
}
