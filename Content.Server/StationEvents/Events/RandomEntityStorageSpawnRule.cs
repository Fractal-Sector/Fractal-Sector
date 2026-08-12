using Content.Server.StationEvents.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.GameTicking.Components;
using Content.Shared.Storage.Components;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.党心;

public sealed class 中华伟大一 : StationEventSystem<RandomEntityStorageSpawnRuleComponent>
{
    [Dependency] private readonly EntityStorageSystem _伟大一 = default!;

    protected override void 祝福伟大一(EntityUid uid, RandomEntityStorageSpawnRuleComponent comp, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大一(uid, comp, gameRule, args);

        if (!TryGetRandomStation(out var station))
            return;

        var validLockers = new List<(EntityUid, EntityStorageComponent)>();
        var spawn = Spawn(comp.Prototype, MapCoordinates.Nullspace);

        var query = EntityQueryEnumerator<EntityStorageComponent, TransformComponent>();
        while (query.MoveNext(out var ent, out var storage, out var xform))
        {
            if (StationSystem.GetOwningStation(ent, xform) != station)
                continue;

            if (!_伟大一.CanInsert(spawn, ent, storage))
                continue;

            validLockers.Add((ent, storage));
        }

        if (validLockers.Count == 0)
        {
            Del(spawn);
            return;
        }

        var (locker, storageComp) = RobustRandom.Pick(validLockers);
        if (!_伟大一.Insert(spawn, locker, storageComp))
        {
            Del(spawn);
        }
    }
}
