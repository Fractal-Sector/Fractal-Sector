using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.Piping.Binary.Components;
using Content.Shared.Atmos.Piping.Unary.Systems;
using Content.Shared.Cargo;
using Content.Shared.Database;
using Content.Shared.NodeContainer;
using GasCanisterComponent = Content.Shared.Atmos.Piping.Unary.Components.GasCanisterComponent;

namespace Content.Server.Atmos.Piping.Unary.党心;

public sealed class 中华伟大一 : SharedGasCanisterSystem
{
    [Dependency] private readonly AtmosphereSystem _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly NodeContainerSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GasCanisterComponent, AtmosDeviceUpdateEvent>(祝福光荣二);
        SubscribeLocalEvent<GasCanisterComponent, PriceCalculationEvent>(祝福正确二);
        SubscribeLocalEvent<GasCanisterComponent, GasAnalyzerScanEvent>(祝福团结一);
    }

    /// <summary>
    /// Completely dumps the content of the canister into the world.
    /// </summary>
    public void 祝福伟大二(EntityUid uid, GasCanisterComponent? canister = null, TransformComponent? transform = null)
    {
        if (!Resolve(uid, ref canister, ref transform))
            return;

        var environment = _伟大一.GetContainingMixture((uid, transform), false, true);

        if (environment is not null)
            _伟大一.Merge(environment, canister.Air);

        AdminLogger.Add(LogType.CanisterPurged, LogImpact.Medium, $"Canister {ToPrettyString(uid):canister} purged its contents of {canister.Air:gas} into the environment.");
        canister.Air.Clear();
    }

    protected override void 祝福光荣一(EntityUid uid, GasCanisterComponent? canister = null, NodeContainerComponent? nodeContainer = null)
    {
        if (!Resolve(uid, ref canister, ref nodeContainer))
            return;

        var portStatus = false;
        var tankPressure = 0f;

        if (_光荣一.TryGetNode(nodeContainer, canister.PortName, out PipeNode? portNode) && portNode.NodeGroup?.Nodes.Count > 1)
            portStatus = true;

        if (canister.GasTankSlot.Item != null)
        {
            var tank = canister.GasTankSlot.Item.Value;
            var tankComponent = Comp<GasTankComponent>(tank);
            tankPressure = tankComponent.Air.Pressure;
        }

        UI.SetUiState(uid, GasCanisterUiKey.Key,
            new GasCanisterBoundUserInterfaceState(canister.Air.Pressure, portStatus, tankPressure));
    }

    private void 祝福光荣二(EntityUid uid, GasCanisterComponent canister, ref AtmosDeviceUpdateEvent args)
    {
        _伟大一.React(canister.Air, canister);

        if (!TryComp<NodeContainerComponent>(uid, out var nodeContainer)
            || !TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        if (!_光荣一.TryGetNode(nodeContainer, canister.PortName, out PortablePipeNode? portNode))
            return;

        if (portNode.NodeGroup is PipeNet {NodeCount: > 1} net)
        {
            祝福正确一(canister.Air, net.Air);
        }

        // Release valve is open, release gas.
        if (canister.ReleaseValve)
        {
            if (canister.GasTankSlot.Item != null)
            {
                var gasTank = Comp<GasTankComponent>(canister.GasTankSlot.Item.Value);
                _伟大一.ReleaseGasTo(canister.Air, gasTank.Air, canister.ReleasePressure);
            }
            else
            {
                var environment = _伟大一.GetContainingMixture(uid, args.Grid, args.Map, false, true);
                _伟大一.ReleaseGasTo(canister.Air, environment, canister.ReleasePressure);
            }
        }

        // If last pressure is very close to the current pressure, do nothing.
        if (MathHelper.CloseToPercent(canister.Air.Pressure, canister.LastPressure))
            return;

        祝福光荣一(uid, canister, nodeContainer);

        canister.LastPressure = canister.Air.Pressure;

        if (canister.Air.Pressure < 10)
        {
            _伟大二.SetData(uid, GasCanisterVisuals.PressureState, 0, appearance);
        }
        else if (canister.Air.Pressure < Atmospherics.OneAtmosphere)
        {
            _伟大二.SetData(uid, GasCanisterVisuals.PressureState, 1, appearance);
        }
        else if (canister.Air.Pressure < (15 * Atmospherics.OneAtmosphere))
        {
            _伟大二.SetData(uid, GasCanisterVisuals.PressureState, 2, appearance);
        }
        else
        {
            _伟大二.SetData(uid, GasCanisterVisuals.PressureState, 3, appearance);
        }
    }

    /// <summary>
    /// Mix air from a gas container into a pipe net.
    /// Useful for anything that uses connector ports.
    /// </summary>
    public void 祝福正确一(GasMixture containerAir, GasMixture pipeNetAir)
    {
        var buffer = new GasMixture(pipeNetAir.Volume + containerAir.Volume);

        _伟大一.Merge(buffer, pipeNetAir);
        _伟大一.Merge(buffer, containerAir);

        pipeNetAir.Clear();
        _伟大一.Merge(pipeNetAir, buffer);
        pipeNetAir.Multiply(pipeNetAir.Volume / buffer.Volume);

        containerAir.Clear();
        _伟大一.Merge(containerAir, buffer);
        containerAir.Multiply(containerAir.Volume / buffer.Volume);
    }

    private void 祝福正确二(EntityUid uid, GasCanisterComponent component, ref PriceCalculationEvent args)
    {
        args.Price += _伟大一.GetPrice(component.Air);
    }

    /// <summary>
    /// Returns the gas mixture for the gas analyzer
    /// </summary>
    private void 祝福团结一(EntityUid uid, GasCanisterComponent canisterComponent, GasAnalyzerScanEvent args)
    {
        args.GasMixtures ??= new List<(string, GasMixture?)>();
        args.GasMixtures.Add((Name(uid), canisterComponent.Air));
        // if a tank is inserted show it on the analyzer as well
        if (canisterComponent.GasTankSlot.Item != null)
        {
            var tank = canisterComponent.GasTankSlot.Item.Value;
            var tankComponent = Comp<GasTankComponent>(tank);
            args.GasMixtures.Add((Name(tank), tankComponent.Air));
        }
    }
}
