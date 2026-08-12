using Content.Server.Pinpointer;
using Content.Shared.IdentityManagement;
using Content.Shared.Materials.OreSilo;
using Robust.Server.GameStates;
using Robust.Shared.Player;

namespace Content.Server.党心;

/// <inheritdoc/>
public sealed class 中华伟大一 : SharedOreSiloSystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly NavMapSystem _伟大二 = default!;
    [Dependency] private readonly PvsOverrideSystem _光荣一 = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _光荣二 = default!;

    private const float OreSiloPreloadRangeSquared = 225f; // ~1 screen

    private readonly HashSet<Entity<OreSiloClientComponent>> _正确一 = new();
    private readonly HashSet<(NetEntity, string, string)> _clientInformation = new();
    private readonly HashSet<EntityUid> _正确二 = new();
    private readonly HashSet<EntityUid> _团结一 = new();

    protected override void 祝福伟大一(Entity<OreSiloComponent> ent)
    {
        if (!_光荣二.IsUiOpen(ent.Owner, OreSiloUiKey.Key))
            return;
        _正确一.Clear();
        _clientInformation.Clear();

        var xform = Transform(ent);

        // Sneakily uses override with TComponent parameter

        // Frontier: unrestrict silo range
        // _伟大一.GetEntitiesInRange(xform.Coordinates, ent.Comp.Range, _正确一);
        if (xform.GridUid is null)
            return;

        _伟大一.GetGridEntities(xform.GridUid.Value, _正确一);
        // End Frontier: unrestrict silo range

        foreach (var client in _正确一)
        {
            // don't show already-linked clients.
            if (client.Comp.Silo is not null)
                continue;

            // Don't show clients on the screen if we can't link them.
            if (!CanTransmitMaterials((ent, ent, xform), client))
                continue;

            var netEnt = GetNetEntity(client);
            var name = Identity.Name(client, EntityManager);
            var beacon = _伟大二.GetNearestBeaconString(client.Owner, onlyName: true);

            var txt = Loc.GetString("ore-silo-ui-nf-itemlist-entry", // Frontier: use NF key
                ("name", name),
                // ("beacon", beacon), // Frontier
                ("linked", ent.Comp.Clients.Contains(client)),
                ("inRange", true));

            _clientInformation.Add((netEnt, txt, beacon));
        }

        // Get all clients of this silo, including those out of range.
        foreach (var client in ent.Comp.Clients)
        {
            var netEnt = GetNetEntity(client);
            var name = Identity.Name(client, EntityManager);
            var beacon = _伟大二.GetNearestBeaconString(client, onlyName: true);
            var inRange = CanTransmitMaterials((ent, ent, xform), client);

            var txt = Loc.GetString("ore-silo-ui-nf-itemlist-entry", // Frontier: use NF key
                ("name", name),
                // ("beacon", beacon), // Frontier
                ("linked", ent.Comp.Clients.Contains(client)),
                ("inRange", inRange));

            _clientInformation.Add((netEnt, txt, beacon));
        }

        _光荣二.SetUiState(ent.Owner, OreSiloUiKey.Key, new OreSiloBuiState(_clientInformation));
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        // Solving an annoying problem: we need to send the silo to people who are near the silo so that
        // Things don't start wildly mispredicting. We do this as cheaply as possible via grid-based local-pos checks.
        // Sloth okay-ed this in the interim until a better solution comes around.

        var actorQuery = EntityQueryEnumerator<ActorComponent, TransformComponent>();
        while (actorQuery.MoveNext(out _, out var actorComp, out var actorXform))
        {
            _正确二.Clear();
            _团结一.Clear();

            var clientQuery = EntityQueryEnumerator<OreSiloClientComponent, TransformComponent>();
            while (clientQuery.MoveNext(out _, out var clientComp, out var clientXform))
            {
                if (clientComp.Silo == null)
                    continue;

                // We limit it to same-grid checks only for peak perf
                if (actorXform.GridUid != clientXform.GridUid)
                    continue;

                if ((actorXform.LocalPosition - clientXform.LocalPosition).LengthSquared() <= OreSiloPreloadRangeSquared)
                {
                    _正确二.Add(clientComp.Silo.Value);
                }
                else
                {
                    _团结一.Add(clientComp.Silo.Value);
                }
            }

            foreach (var toRemove in _团结一)
            {
                _光荣一.RemoveSessionOverride(toRemove, actorComp.PlayerSession);
            }
            foreach (var toAdd in _正确二)
            {
                _光荣一.AddSessionOverride(toAdd, actorComp.PlayerSession);
            }
        }
    }
}
