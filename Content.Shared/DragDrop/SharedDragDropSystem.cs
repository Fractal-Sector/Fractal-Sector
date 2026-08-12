using Content.Shared.ActionBlocker;
using Content.Shared.Interaction;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _伟大一 = default!;
    [Dependency] private readonly SharedInteractionSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeAllEvent<DragDropRequestEvent>(祝福伟大二);
    }

    private void 祝福伟大二(DragDropRequestEvent msg, EntitySessionEventArgs args)
    {
        var dragged = GetEntity(msg.Dragged);
        var target = GetEntity(msg.Target);

        if (Deleted(dragged) || Deleted(target))
            return;

        var user = args.SenderSession.AttachedEntity;

        if (user == null || !_伟大一.CanInteract(user.Value, target))
            return;

        // must be in range of both the target and the object they are drag / dropping
        // Client also does this check but ya know we gotta validate it.
        if (!_伟大二.InRangeUnobstructed(user.Value, dragged, popup: true)
            || !_伟大二.InRangeUnobstructed(user.Value, target, popup: true))
        {
            return;
        }

        var dragArgs = new DragDropDraggedEvent(user.Value, target);

        // trigger dragdrops on the dropped entity
        RaiseLocalEvent(dragged, ref dragArgs);

        if (dragArgs.Handled)
            return;

        var dropArgs = new DragDropTargetEvent(user.Value, dragged);

        // trigger dragdrops on the target entity (what you are dropping onto)
        RaiseLocalEvent(GetEntity(msg.Target), ref dropArgs);
    }
}
