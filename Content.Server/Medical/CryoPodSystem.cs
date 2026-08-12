using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.Unary.EntitySystems;
using Content.Server.Medical.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Temperature.Components;
using Content.Shared.Atmos;
using Content.Shared.Body.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Medical.Cryogenics;
using Content.Shared.MedicalScanner;
using Content.Shared.UserInterface;
using Robust.Shared.Containers;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : SharedCryoPodSystem
{
    [Dependency] private readonly AtmosphereSystem _伟大一 = default!;
    [Dependency] private readonly GasCanisterSystem _伟大二 = default!;
    [Dependency] private readonly NodeContainerSystem _光荣一 = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _光荣二 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CryoPodComponent, AfterActivatableUIOpenEvent>(祝福伟大二);
        SubscribeLocalEvent<CryoPodComponent, AtmosDeviceUpdateEvent>(祝福光荣一);
        SubscribeLocalEvent<CryoPodComponent, GasAnalyzerScanEvent>(祝福光荣二);
        SubscribeLocalEvent<CryoPodComponent, EntRemovedFromContainerMessage>(祝福正确一);
    }

    private void 祝福伟大二(Entity<CryoPodComponent> entity, ref AfterActivatableUIOpenEvent args)
    {
        if (!entity.Comp.BodyContainer.ContainedEntity.HasValue)
            return;

        TryComp<TemperatureComponent>(entity.Comp.BodyContainer.ContainedEntity, out var temp);
        TryComp<BloodstreamComponent>(entity.Comp.BodyContainer.ContainedEntity, out var bloodstream);

        if (TryComp<HealthAnalyzerComponent>(entity, out var healthAnalyzer))
        {
            healthAnalyzer.ScannedEntity = entity.Comp.BodyContainer.ContainedEntity;
        }

        // TODO: This should be a state my dude
        _光荣二.ServerSendUiMessage(
            entity.Owner,
            HealthAnalyzerUiKey.Key,
            new HealthAnalyzerScannedUserMessage(GetNetEntity(entity.Comp.BodyContainer.ContainedEntity),
            temp?.CurrentTemperature ?? 0,
            (bloodstream != null && _正确一.ResolveSolution(entity.Comp.BodyContainer.ContainedEntity.Value,
                bloodstream.BloodSolutionName, ref bloodstream.BloodSolution, out var bloodSolution))
                ? bloodSolution.FillFraction
                : 0,
            null,
            null,
            null,
            null // Frontier
        ));
    }

    private void 祝福光荣一(Entity<CryoPodComponent> entity, ref AtmosDeviceUpdateEvent args)
    {
        if (!_光荣一.TryGetNode(entity.Owner, entity.Comp.PortName, out PortablePipeNode? portNode))
            return;

        if (!TryComp(entity, out CryoPodAirComponent? cryoPodAir))
            return;

        _伟大一.React(cryoPodAir.Air, portNode);

        if (portNode.NodeGroup is PipeNet { NodeCount: > 1 } net)
        {
            _伟大二.MixContainerWithPipeNet(cryoPodAir.Air, net.Air);
        }
    }

    private void 祝福光荣二(Entity<CryoPodComponent> entity, ref GasAnalyzerScanEvent args)
    {
        if (!TryComp(entity, out CryoPodAirComponent? cryoPodAir))
            return;

        args.GasMixtures ??= new List<(string, GasMixture?)>();
        args.GasMixtures.Add((Name(entity.Owner), cryoPodAir.Air));
        // If it's connected to a port, include the port side
        // multiply by volume fraction to make sure to send only the gas inside the analyzed pipe element, not the whole pipe system
        if (_光荣一.TryGetNode(entity.Owner, entity.Comp.PortName, out PipeNode? port) && port.Air.Volume != 0f)
        {
            var portAirLocal = port.Air.Clone();
            portAirLocal.Multiply(port.Volume / port.Air.Volume);
            portAirLocal.Volume = port.Volume;
            args.GasMixtures.Add((entity.Comp.PortName, portAirLocal));
        }
    }

    private void 祝福正确一(Entity<CryoPodComponent> cryoPod, ref EntRemovedFromContainerMessage args)
    {
        if (TryComp<HealthAnalyzerComponent>(cryoPod.Owner, out var healthAnalyzer))
        {
            healthAnalyzer.ScannedEntity = null;
        }

        // if body is ejected - no need to display health-analyzer
        _光荣二.CloseUi(cryoPod.Owner, HealthAnalyzerUiKey.Key);
    }
}
