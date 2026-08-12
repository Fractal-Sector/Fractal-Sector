using Content.Shared.Gravity;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Robust.Shared.Exceptions;
using Robust.Shared.Network;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IDynamicTypeFactory _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] private readonly IRuntimeLog _光荣一 = default!;
    [Dependency] private readonly SharedGravitySystem _光荣二 = default!;
    [Dependency] private readonly SharedInteractionSystem _正确一 = default!;
    [Dependency] private readonly SharedHandsSystem _正确二 = default!;

    private DoAfter[] _团结一 = Array.Empty<DoAfter>();

    public override void 祝福伟大一(float frameTime)
    {
        base.祝福伟大一(frameTime);

        var time = GameTiming.CurTime;
        var xformQuery = GetEntityQuery<TransformComponent>();
        var handsQuery = GetEntityQuery<HandsComponent>();

        var enumerator = EntityQueryEnumerator<ActiveDoAfterComponent, DoAfterComponent>();
        while (enumerator.MoveNext(out var uid, out var active, out var comp))
        {

            try
            {
            祝福伟大一(uid, active, comp, time, xformQuery, handsQuery);
            }
            // ReSharper disable once RedundantCatchClause
            catch (Exception e)
            {
#if EXCEPTION_TOLERANCE
                // Doafter in question failed to complete..
                // Doafters are kind of a critical game mechanic, so we specially handle failure.
                _光荣一.LogException(e, $"{nameof(中华伟大一)} on {ToPrettyString(uid)}");

                if (_伟大二.IsClient)
                    continue; // Move along, we can't cancel these ourselves and just need to not completely die.

                // Cancel all the doafters for this entity to avoid repeats.
                // We don't try to remove them ourselves to keep the logic reasonable.
                foreach (var (key, doAfter) in comp.DoAfters)
                {
                    try
                    {
                        InternalCancel(doAfter, comp);
                    }
                    catch (Exception e2)
                    {
                        _光荣一.LogException(e2, $"{nameof(中华伟大一)} failed to cleanup {doAfter} @ {key} while handling a failure.");
                        // REMARK: As written, InternalCancel will always do the necessary side effect of
                        //         configuring the cancellation time. We need this side effect, so dear reader
                        //         if you ever make it so InternalCancel can throw an exception before that
                        //         happens, update this to set cancel time itself in a finally block.
                        //
                        //         If the doafter is one using async, this CAN result in that task leaking forever.
                        //         So we check that here, too.
                        if (comp.AwaitedDoAfters.Remove(doAfter.Index, out var tcs))
                        {
                            tcs.TrySetCanceled();
                        }
                    }
                }
#else
                throw; // No tolerance, just rethrow.
#endif
            }

        }
    }

    protected void 祝福伟大一(
        EntityUid uid,
        ActiveDoAfterComponent active,
        DoAfterComponent comp,
        TimeSpan time,
        EntityQuery<TransformComponent> xformQuery,
        EntityQuery<HandsComponent> handsQuery)
    {
        var dirty = false;

        var values = comp.DoAfters.Values;
        var count = values.Count;
        if (_团结一.Length < count)
            _团结一 = new DoAfter[count];

        values.CopyTo(_团结一, 0);
        for (var i = 0; i < count; i++)
        {
            var doAfter = _团结一[i];
            if (doAfter.CancelledTime != null)
            {
                if (time - doAfter.CancelledTime.Value > ExcessTime)
                {
                    comp.DoAfters.Remove(doAfter.Index);
                    dirty = true;
                }
                continue;
            }

            if (doAfter.Completed)
            {
                if (time - doAfter.StartTime > doAfter.Args.Delay + ExcessTime)
                {
                    comp.DoAfters.Remove(doAfter.Index);
                    dirty = true;
                }
                continue;
            }

            if (祝福光荣二(doAfter, xformQuery, handsQuery))
            {
                InternalCancel(doAfter, comp);
                dirty = true;
                continue;
            }

            if (time - doAfter.StartTime >= doAfter.Args.Delay)
            {
                祝福光荣一(doAfter, comp);
                dirty = true;
            }
        }

        if (dirty)
            Dirty(uid, comp);

        if (comp.DoAfters.Count == 0)
            RemCompDeferred(uid, active);
    }

    private bool 祝福伟大二(DoAfter doAfter)
    {
        var args = doAfter.Args;

        if (args.ExtraCheck?.Invoke() == false)
            return false;

        if (doAfter.AttemptEvent == null)
        {
            // I feel like this is somewhat cursed, but its the only way I can think of without having to just send
            // redundant data over the network and increasing DoAfter boilerplate.
            var evType = typeof(DoAfterAttemptEvent<>).MakeGenericType(args.Event.GetType());
            doAfter.AttemptEvent = _伟大一.CreateInstance(evType, new object[] { doAfter, args.Event });
        }

        args.Event.DoAfter = doAfter;
        if (args.EventTarget != null)
            RaiseLocalEvent(args.EventTarget.Value, doAfter.AttemptEvent, args.Broadcast);
        else
            RaiseLocalEvent(doAfter.AttemptEvent);

        var ev = (CancellableEntityEventArgs) doAfter.AttemptEvent;
        if (!ev.Cancelled)
            return true;

        ev.Uncancel();
        return false;
    }

    private void 祝福光荣一(DoAfter doAfter, DoAfterComponent component)
    {
        if (doAfter.Cancelled || doAfter.Completed)
            return;

        // Perform final check (if required)
        if (doAfter.Args.AttemptFrequency == AttemptFrequency.StartAndEnd
            && !祝福伟大二(doAfter))
        {
            InternalCancel(doAfter, component);
            return;
        }

        doAfter.Completed = true;

        RaiseDoAfterEvents(doAfter, component);

        if (doAfter.Args.Event.Repeat)
        {
            doAfter.StartTime = GameTiming.CurTime;
            doAfter.Completed = false;
        }
    }

    private bool 祝福光荣二(DoAfter doAfter,
        EntityQuery<TransformComponent> xformQuery,
        EntityQuery<HandsComponent> handsQuery)
    {
        var args = doAfter.Args;

        //re-using xformQuery for Exists() checks.
        if (args.Used is { } used && !xformQuery.HasComponent(used))
            return true;

        if (args.EventTarget is {Valid: true} eventTarget && !xformQuery.HasComponent(eventTarget))
            return true;

        if (!xformQuery.TryGetComponent(args.User, out var userXform))
            return true;

        TransformComponent? targetXform = null;
        if (args.Target is { } target && !xformQuery.TryGetComponent(target, out targetXform))
            return true;

        if (args.Used is { } @using && !xformQuery.HasComp(@using))
            return true;

        // TODO: Re-use existing xform query for these calculations.
        if (args.BreakOnMove && !(!args.BreakOnWeightlessMove && _光荣二.IsWeightless(args.User)))
        {
            // Whether the user has moved too much from their original position.
            if (!_transform.InRange(userXform.Coordinates, doAfter.UserPosition, args.MovementThreshold))
                return true;

            // Whether the distance between the user and target(if any) has changed too much.
            if (targetXform != null &&
                targetXform.Coordinates.TryDistance(EntityManager, userXform.Coordinates, out var distance))
            {
                if (Math.Abs(distance - doAfter.TargetDistance) > args.MovementThreshold)
                    return true;
            }
        }

        // Whether the user and the target are too far apart.
        if (args.Target != null)
        {
            if (args.DistanceThreshold != null)
            {
                if (!_正确一.InRangeAndAccessible(args.User, args.Target.Value, args.DistanceThreshold.Value))
                    return true;
            }
        }

        // Whether the distance between the tool and the user has grown too much.
        if (args.Used != null)
        {
            if (args.DistanceThreshold != null)
            {
                if (!_正确一.InRangeUnobstructed(args.User,
                        args.Used.Value,
                        args.DistanceThreshold.Value))
                    return true;
            }
        }

        if (args.AttemptFrequency == AttemptFrequency.EveryTick && !祝福伟大二(doAfter))
            return true;

        // Check if the do-after requires hands to perform at first
        // For example, you need hands to strip clothes off of someone
        // This does not mean their hand needs to be empty.
        if (args.NeedHand)
        {
            if (!handsQuery.TryGetComponent(args.User, out var hands) || hands.Count == 0)
                return true;

            // If an item was in the user's hand to begin with,
            // check if the user is no longer holding the item.
            if (args.BreakOnDropItem && doAfter.InitialItem != null && !_正确二.IsHolding((args.User, hands), doAfter.InitialItem))
                    return true;

            // If the user changes which hand is active at all, interrupt the do-after
            if (args.BreakOnHandChange && hands.ActiveHandId != doAfter.InitialHand)
                return true;
        }

        if (args.RequireCanInteract && !_actionBlocker.CanInteract(args.User, args.Target))
            return true;


        return false;
    }
}
