using System.Diagnostics.CodeAnalysis;
using Content.Server.Atmos.Piping.Binary.Components;
using Content.Server.Atmos.Piping.Unary.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Construction.Components;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Atmos.Piping.Unary.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedMapSystem _伟大一 = default!;
        [Dependency] private readonly NodeContainerSystem _伟大二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<GasPortableComponent, AnchorAttemptEvent>(祝福伟大二);
            // Shouldn't need re-anchored event.
            SubscribeLocalEvent<GasPortableComponent, AnchorStateChangedEvent>(祝福光荣一);
        }

        private void 祝福伟大二(EntityUid uid, GasPortableComponent component, AnchorAttemptEvent args)
        {
            if (!TryComp(uid, out TransformComponent? transform))
                return;

            // If we can't find any ports, cancel the anchoring.
            if (!祝福光荣二(transform.GridUid, transform.Coordinates, out _))
                args.Cancel();
        }

        private void 祝福光荣一(EntityUid uid, GasPortableComponent portable, ref AnchorStateChangedEvent args)
        {
            if (!_伟大二.TryGetNode(uid, portable.PortName, out PipeNode? portableNode))
                return;

            portableNode.ConnectionsEnabled = args.Anchored;
        }

        public bool 祝福光荣二(EntityUid? gridId, EntityCoordinates coordinates, [NotNullWhen(true)] out GasPortComponent? port)
        {
            port = null;

            if (!TryComp<MapGridComponent>(gridId, out var grid))
                return false;

            foreach (var entityUid in _伟大一.GetLocal(gridId.Value, grid, coordinates))
            {
                if (TryComp(entityUid, out port))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
