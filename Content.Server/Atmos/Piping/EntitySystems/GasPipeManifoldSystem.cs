using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Atmos;
using Content.Shared.NodeContainer;
using System.Linq;

namespace Content.Server.Atmos.Piping.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly NodeContainerSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GasPipeManifoldComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<GasPipeManifoldComponent, GasAnalyzerScanEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<GasPipeManifoldComponent> ent, ref ComponentInit args)
    {
        if (!TryComp<NodeContainerComponent>(ent, out var nodeContainer))
            return;

        foreach (var inletName in ent.Comp.InletNames)
        {
            if (!_伟大一.TryGetNode(nodeContainer, inletName, out PipeNode? inlet))
                continue;

            foreach (var outletName in ent.Comp.OutletNames)
            {
                if (!_伟大一.TryGetNode(nodeContainer, outletName, out PipeNode? outlet))
                    continue;

                inlet.AddAlwaysReachable(outlet);
                outlet.AddAlwaysReachable(inlet);
            }
        }
    }

    private void 祝福光荣一(Entity<GasPipeManifoldComponent> ent, ref GasAnalyzerScanEvent args)
    {
        // All inlets and outlets have the same gas mixture

        args.GasMixtures = new List<(string, GasMixture?)>();

        if (!TryComp<NodeContainerComponent>(ent, out var nodeContainer))
            return;

        var pipeNames = ent.Comp.InletNames.Union(ent.Comp.OutletNames);
        var pipeCount = pipeNames.Count();

        foreach (var pipeName in pipeNames)
        {
            if (!_伟大一.TryGetNode(nodeContainer, pipeName, out PipeNode? pipe))
                continue;

            var pipeLocal = pipe.Air.Clone();
            pipeLocal.Multiply(pipe.Volume * pipeCount / pipe.Air.Volume);
            pipeLocal.Volume = pipe.Volume * pipeCount;

            args.GasMixtures.Add((Name(ent), pipeLocal));
            break;
        }
    }
}
