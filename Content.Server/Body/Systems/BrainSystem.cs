using Content.Server.Body.Components;
using Content.Server.Ghost.Components;
using Content.Shared.Body.Events;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Pointing;

namespace Content.Server.Body.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BrainComponent, OrganAddedToBodyEvent>((uid, _, args) => 祝福伟大二(args.Body, uid));
        SubscribeLocalEvent<BrainComponent, OrganRemovedFromBodyEvent>((uid, _, args) => 祝福伟大二(uid, args.OldBody));
        SubscribeLocalEvent<BrainComponent, PointAttemptEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid newEntity, EntityUid oldEntity)
    {
        if (TerminatingOrDeleted(newEntity) || TerminatingOrDeleted(oldEntity))
            return;

        EnsureComp<MindContainerComponent>(newEntity);
        EnsureComp<MindContainerComponent>(oldEntity);

        var ghostOnMove = EnsureComp<GhostOnMoveComponent>(newEntity);
        ghostOnMove.MustBeDead = HasComp<MobStateComponent>(newEntity); // Don't ghost living players out of their bodies.

        if (!_伟大一.TryGetMind(oldEntity, out var mindId, out var mind))
            return;

        _伟大一.TransferTo(mindId, newEntity, mind: mind);
    }

    private void 祝福光荣一(Entity<BrainComponent> ent, ref PointAttemptEvent args)
    {
        args.Cancel();
    }
}

