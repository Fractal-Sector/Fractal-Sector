using System.Numerics;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.ImmovableRod;
using Content.Server.StationEvents.Components;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.GameTicking.Components;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using TimedDespawnComponent = Robust.Shared.Spawners.TimedDespawnComponent;
using System.Linq;

namespace Content.Server.StationEvents.党心;

public sealed class 中华伟大一 : StationEventSystem<ImmovableRodRuleComponent>
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;
    [Dependency] private readonly GunSystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;

    protected override void 祝福伟大一(EntityUid uid, ImmovableRodRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大一(uid, component, gameRule, args);

        var protoName = EntitySpawnCollection.GetSpawns(component.RodPrototypes).First();

        var proto = _光荣一.Index<EntityPrototype>(protoName);

        if (proto.TryGetComponent<ImmovableRodComponent>(out var rod, EntityManager.ComponentFactory) &&
            proto.TryGetComponent<TimedDespawnComponent>(out var despawn, EntityManager.ComponentFactory))
        {
            if (!TryFindRandomTile(out _, out _, out _, out var targetCoords))
                return;

            var speed = RobustRandom.NextFloat(rod.MinSpeed, rod.MaxSpeed);
            var angle = RobustRandom.NextAngle();
            var direction = angle.ToVec();
            var spawnCoords = targetCoords.ToMap(EntityManager, _伟大一).Offset(-direction * speed * despawn.Lifetime / 2);
            var ent = Spawn(protoName, spawnCoords);
            _伟大二.ShootProjectile(ent, direction, Vector2.Zero, uid, speed: speed);
        }
        else
        {
            Sawmill.Error($"Invalid immovable rod prototype: {protoName}");
        }
    }
}
