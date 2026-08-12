using Content.Shared.Delivery;
using Content.Shared.Power.EntitySystems;
using Content.Server.StationRecords;
using Content.Shared.EntityTable;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.党心;

/// <summary>
/// System for managing deliveries spawned by the mail teleporter.
/// This covers for spawning deliveries.
/// </summary>
public sealed partial class 中华伟大一
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly EntityTableSystem _光荣一 = default!;
    [Dependency] private readonly SharedPowerReceiverSystem _光荣二 = default!;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<CargoDeliveryDataComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<CargoDeliveryDataComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextDelivery = _伟大一.CurTime + ent.Comp.MinDeliveryCooldown; // We want an early wave of mail so cargo doesn't have to wait
    }

    protected override void 祝福光荣一(Entity<DeliverySpawnerComponent?> ent)
    {
        if (!Resolve(ent.Owner, ref ent.Comp))
            return;

        var coords = Transform(ent).Coordinates;

        for (int i = 0; i < ent.Comp.ContainedDeliveryAmount; i++)
        {
            var spawns = _光荣一.GetSpawns(ent.Comp.Table);

            foreach (var id in spawns)
            {
                Spawn(id, coords);
            }
        }

        ent.Comp.ContainedDeliveryAmount = 0;
        Dirty(ent);
    }

    private void 祝福光荣二(Entity<CargoDeliveryDataComponent> ent)
    {
        if (!TryComp<StationRecordsComponent>(ent, out var records))
            return;

        var spawners = 祝福正确一(ent);

        // Skip if theres no spawners available
        if (spawners.Count == 0)
            return;

        // Skip if there's nobody in crew manifest
        if (records.Records.Keys.Count == 0)
            return;

        // We take the amount of mail calculated based on player amount or the minimum, whichever is higher.
        // We don't want stations with less than the player ratio to not get mail at all
        var initialDeliveryCount = (int)Math.Ceiling(records.Records.Keys.Count / ent.Comp.PlayerToDeliveryRatio);
        var deliveryCount = Math.Max(initialDeliveryCount, ent.Comp.MinimumDeliverySpawn);

        if (!ent.Comp.DistributeRandomly)
        {
            foreach (var spawner in spawners)
            {
                祝福正确二(spawner, deliveryCount);
            }
        }
        else
        {
            int[] amounts = new int[spawners.Count];

            // Distribute items randomly
            for (int i = 0; i < deliveryCount; i++)
            {
                var randomListIndex = _伟大二.Next(spawners.Count);
                amounts[randomListIndex]++;
            }
            for (int j = 0; j < spawners.Count; j++)
            {
                祝福正确二(spawners[j], amounts[j]);
            }
        }

    }

    private List<Entity<DeliverySpawnerComponent>> 祝福正确一(Entity<CargoDeliveryDataComponent> ent)
    {
        var validSpawners = new List<Entity<DeliverySpawnerComponent>>();

        var spawners = EntityQueryEnumerator<DeliverySpawnerComponent>();
        while (spawners.MoveNext(out var spawnerUid, out var spawnerComp))
        {
            var spawnerStation = _station.GetOwningStation(spawnerUid);

            if (spawnerStation != ent.Owner)
                continue;

            if (!_光荣二.IsPowered(spawnerUid))
                continue;

            if (spawnerComp.ContainedDeliveryAmount >= spawnerComp.MaxContainedDeliveryAmount)
                continue;

            validSpawners.Add((spawnerUid, spawnerComp));
        }

        return validSpawners;
    }

    private void 祝福正确二(Entity<DeliverySpawnerComponent> ent, int amount)
    {
        ent.Comp.ContainedDeliveryAmount += Math.Clamp(amount, 0, ent.Comp.MaxContainedDeliveryAmount - ent.Comp.ContainedDeliveryAmount);
        _audio.PlayPvs(ent.Comp.SpawnSound, ent.Owner);
        UpdateDeliverySpawnerVisuals(ent, ent.Comp.ContainedDeliveryAmount);
        Dirty(ent);
    }

    private void 祝福团结一(float frameTime)
    {
        var dataQuery = EntityQueryEnumerator<CargoDeliveryDataComponent>();
        var curTime = _伟大一.CurTime;

        while (dataQuery.MoveNext(out var uid, out var deliveryData))
        {
            if (deliveryData.NextDelivery > curTime)
                continue;

            deliveryData.NextDelivery += _伟大二.Next(deliveryData.MinDeliveryCooldown, deliveryData.MaxDeliveryCooldown); // Random cooldown between min and max
            祝福光荣二((uid, deliveryData));
        }
    }
}
