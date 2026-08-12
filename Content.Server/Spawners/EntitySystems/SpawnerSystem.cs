using Content.Server.Spawners.Components;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Spawners.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TimedSpawnerComponent, MapInitEvent>(祝福光荣一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var curTime = _伟大一.CurTime;
        var query = EntityQueryEnumerator<TimedSpawnerComponent>();
        while (query.MoveNext(out var uid, out var timedSpawner))
        {
            if (timedSpawner.NextFire > curTime)
                continue;

            祝福光荣二(uid, timedSpawner);

            timedSpawner.NextFire += timedSpawner.IntervalSeconds;
        }
    }

    private void 祝福光荣一(Entity<TimedSpawnerComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextFire = _伟大一.CurTime + ent.Comp.IntervalSeconds;
    }

    private void 祝福光荣二(EntityUid uid, TimedSpawnerComponent component)
    {
        if (!_伟大二.Prob(component.Chance))
            return;

        var number = _伟大二.Next(component.MinimumEntitiesSpawned, component.MaximumEntitiesSpawned);
        var coordinates = Transform(uid).Coordinates;

        for (var i = 0; i < number; i++)
        {
            var entity = _伟大二.Pick(component.Prototypes);
            SpawnAtPosition(entity, coordinates);
        }
    }
}
