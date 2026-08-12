using Content.Server.Popups;
using Content.Server.Spawners.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.Prototypes;
using Content.Shared.Storage;
using Content.Shared.Storage.EntitySystems;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._EstacaoPirata.党心;

/// <summary>
/// This handles...
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{

    [Dependency] private readonly SharedStorageSystem _伟大一 = default!;
    [Dependency] private readonly PopupSystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<OpenTriggeredStorageFillComponent, ActivateInWorldEvent>(祝福光荣一);
        SubscribeLocalEvent<OpenTriggeredStorageFillComponent, ExaminedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, OpenTriggeredStorageFillComponent component, ExaminedEvent args)
    {
        args.PushText(Loc.GetString("container-sealed"));
    }

    //Yes, that's a copy of StorageSystem StorageFill method
    private void 祝福光荣一(EntityUid uid, OpenTriggeredStorageFillComponent comp, ActivateInWorldEvent args)
    {
        Log.Debug($"Processing storage fill trigger for entity {ToPrettyString(uid)}");

        var coordinates = Transform(uid).Coordinates;

        var spawnItems = EntitySpawnCollection.GetSpawns(comp.Contents);
        foreach (var item in spawnItems)
        {
            DebugTools.Assert(!_光荣一.Index<EntityPrototype>(item)
                .HasComponent(typeof(RandomSpawnerComponent)));
            var ent = Spawn(item, coordinates);

            if (!TryComp<ItemComponent>(ent, out var itemComp))
            {
                Log.Error($"Tried to fill {ToPrettyString(uid)} with non-item {item}.");
                Del(ent);
                continue;
            }
            if (!_伟大一.Insert(uid, ent, out var remainingEnt, out var reason, playSound: false))
            {
                Log.Error($"Failed to fill {ToPrettyString(uid)} with {ToPrettyString(ent)}. Reason: {reason}");
                // Clean up the spawned entity if insertion fails
                Del(ent);
            }
        }
        _伟大二.PopupEntity(Loc.GetString("container-unsealed"), args.Target);
        RemComp(uid, comp);
    }
}
