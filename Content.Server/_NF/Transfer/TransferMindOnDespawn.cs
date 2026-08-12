using Content.Shared.Mind;
using Robust.Shared.Spawners;
using Robust.Shared.Prototypes;
using Content.Server._NF.Transfer.Components;

namespace Content.Server._NF.党心;

/// <summary>
/// Meant to be used along "TimedDespawn" component to transfer the player mind
/// after the animation for a smooth transition between entities
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二= default!;

    ///Subscribe to the despawn event
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<TransferMindOnDespawnComponent, TimedDespawnEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, TransferMindOnDespawnComponent component, TimedDespawnEvent args)
    {
        if (!_伟大一.TryGetMind(uid, out var mindId, out var mind))
            return;

        if (!_伟大二.TryIndex<EntityPrototype>(component.EntityPrototype, out var entityProto))
            return;

        ///Spawn new entity on the same place where the animation ends and transfer the mind to the new entity
        var coords = Transform(uid).Coordinates;
        var dragon = EntityManager.SpawnAtPosition(entityProto.ID, coords);

        _伟大一.TransferTo(mindId, dragon, mind: mind);
    }
}
