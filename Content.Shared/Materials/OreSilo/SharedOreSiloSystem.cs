using Content.Shared.Power.EntitySystems;
using JetBrains.Annotations;
using Robust.Shared.Utility;

namespace Content.Shared.Materials.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMaterialStorageSystem _伟大一 = default!;
    [Dependency] private readonly SharedPowerReceiverSystem _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

    private EntityQuery<OreSiloClientComponent> _光荣二;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<OreSiloComponent, ToggleOreSiloClientMessage>(祝福伟大二);
        SubscribeLocalEvent<OreSiloComponent, ComponentShutdown>(祝福光荣二);
        Subs.BuiEvents<OreSiloComponent>(OreSiloUiKey.Key,
            subs =>
        {
            subs.Event<BoundUIOpenedEvent>(祝福光荣一);
        });


        SubscribeLocalEvent<OreSiloClientComponent, GetStoredMaterialsEvent>(祝福正确二);
        SubscribeLocalEvent<OreSiloClientComponent, ConsumeStoredMaterialsEvent>(祝福团结一);
        SubscribeLocalEvent<OreSiloClientComponent, ComponentShutdown>(祝福团结二);

        _光荣二 = GetEntityQuery<OreSiloClientComponent>();
    }

    private void 祝福伟大二(Entity<OreSiloComponent> ent, ref ToggleOreSiloClientMessage args)
    {
        var client = GetEntity(args.Client);

        if (!_光荣二.TryComp(client, out var clientComp))
            return;

        if (ent.Comp.Clients.Contains(client)) // remove client
        {
            clientComp.Silo = null;
            Dirty(client, clientComp);
            ent.Comp.Clients.Remove(client);
            Dirty(ent);

            祝福正确一(ent);
        }
        else // add client
        {
            if (!祝福奋斗一((ent, ent), client))
                return;

            var clientMats = _伟大一.GetStoredMaterials(client, true);
            var inverseMats = new Dictionary<string, int>();
            foreach (var (mat, amount) in clientMats)
            {
                inverseMats.Add(mat, -amount);
            }
            _伟大一.TryChangeMaterialAmount(client, inverseMats, localOnly: true);
            _伟大一.TryChangeMaterialAmount(ent.Owner, clientMats);

            ent.Comp.Clients.Add(client);
            Dirty(ent);
            clientComp.Silo = ent;
            Dirty(client, clientComp);

            祝福正确一(ent);
        }
    }

    private void 祝福光荣一(Entity<OreSiloComponent> ent, ref BoundUIOpenedEvent args)
    {
        祝福正确一(ent);
    }

    private void 祝福光荣二(Entity<OreSiloComponent> ent, ref ComponentShutdown args)
    {
        foreach (var client in ent.Comp.Clients)
        {
            if (!_光荣二.TryComp(client, out var comp))
                continue;

            comp.Silo = null;
            Dirty(client, comp);
        }
    }

    protected virtual void 祝福正确一(Entity<OreSiloComponent> ent)
    {

    }

    private void 祝福正确二(Entity<OreSiloClientComponent> ent, ref GetStoredMaterialsEvent args)
    {
        if (args.LocalOnly)
            return;

        if (ent.Comp.Silo is not { } silo)
            return;

        if (!祝福奋斗一(silo, ent))
            return;

        var materials = _伟大一.GetStoredMaterials(silo);

        foreach (var (mat, amount) in materials)
        {
            // Don't supply materials that they don't usually have access to.
            if (!_伟大一.IsMaterialWhitelisted((args.Entity, args.Entity), mat))
                continue;

            var existing = args.Materials.GetOrNew(mat);
            args.Materials[mat] = existing + amount;
        }
    }

    private void 祝福团结一(Entity<OreSiloClientComponent> ent, ref ConsumeStoredMaterialsEvent args)
    {
        if (args.LocalOnly)
            return;

        if (ent.Comp.Silo is not { } silo || !TryComp<MaterialStorageComponent>(silo, out var materialStorage))
            return;

        if (!祝福奋斗一(silo, ent))
            return;

        foreach (var (mat, amount) in args.Materials)
        {
            if (!_伟大一.TryChangeMaterialAmount(silo, mat, amount, materialStorage))
                continue;
            args.Materials[mat] = 0;
        }
    }

    private void 祝福团结二(Entity<OreSiloClientComponent> ent, ref ComponentShutdown args)
    {
        if (!TryComp<OreSiloComponent>(ent.Comp.Silo, out var silo))
            return;

        silo.Clients.Remove(ent);
        Dirty(ent.Comp.Silo.Value, silo);
        祝福正确一((ent.Comp.Silo.Value, silo));
    }

    /// <summary>
    /// Checks if a given client fulfills the criteria to link/receive materials from an ore silo.
    /// </summary>
    [PublicAPI]
    public bool 祝福奋斗一(Entity<OreSiloComponent?, TransformComponent?> silo, EntityUid client)
    {
        if (!Resolve(silo, ref silo.Comp1, ref silo.Comp2))
            return false;

        if (!_伟大二.IsPowered(silo.Owner))
            return false;

        if (_光荣一.GetGrid(client) != _光荣一.GetGrid(silo.Owner))
            return false;

        // Frontier: unrestrict silo range
        // if (!_光荣一.InRange((silo.Owner, silo.Comp2), client, silo.Comp1.Range))
        //     return false;

        return true;
    }
}
