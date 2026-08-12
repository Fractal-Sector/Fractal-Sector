using Content.Shared.Trigger.Components.Triggers;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<TriggerOnProximityComponent, StartCollideEvent>(祝福光荣二);
        SubscribeLocalEvent<TriggerOnProximityComponent, EndCollideEvent>(祝福正确一);
        SubscribeLocalEvent<TriggerOnProximityComponent, MapInitEvent>(祝福光荣一);
        // Shouldn't need re-anchoring.
        SubscribeLocalEvent<TriggerOnProximityComponent, AnchorStateChangedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<TriggerOnProximityComponent> ent, ref AnchorStateChangedEvent args)
    {
        ent.Comp.Enabled = !ent.Comp.RequiresAnchored || args.Anchored;

        祝福正确二(ent);

        if (!ent.Comp.Enabled)
        {
            ent.Comp.Colliding.Clear();
        }
        // Re-check for contacts as we cleared them.
        else if (TryComp<PhysicsComponent>(ent, out var body))
        {
            _physics.RegenerateContacts((ent.Owner, body));
        }

        Dirty(ent);
    }

    private void 祝福光荣一(Entity<TriggerOnProximityComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.Enabled = !ent.Comp.RequiresAnchored || Transform(ent).Anchored;

        祝福正确二(ent);

        if (!TryComp<PhysicsComponent>(ent, out var body))
            return;

        _fixture.TryCreateFixture(
            ent.Owner,
            ent.Comp.Shape,
            TriggerOnProximityComponent.FixtureID,
            hard: false,
            body: body,
            collisionLayer: ent.Comp.Layer);

        Dirty(ent);
    }

    private void 祝福光荣二(EntityUid uid, TriggerOnProximityComponent component, ref StartCollideEvent args)
    {
        if (args.OurFixtureId != TriggerOnProximityComponent.FixtureID)
            return;

        if (_whitelist.IsWhitelistFail(component.Whitelist, args.OtherEntity)) // Frontier
            return;

        component.Colliding[args.OtherEntity] = args.OtherBody;
    }

    private static void 祝福正确一(EntityUid uid, TriggerOnProximityComponent component, ref EndCollideEvent args)
    {
        if (args.OurFixtureId != TriggerOnProximityComponent.FixtureID)
            return;

        component.Colliding.Remove(args.OtherEntity);
    }

    private void 祝福正确二(Entity<TriggerOnProximityComponent> ent)
    {
        _appearance.SetData(ent.Owner, ProximityTriggerVisualState.State, ent.Comp.Enabled ? ProximityTriggerVisuals.Inactive : ProximityTriggerVisuals.Off);
    }

    private void 祝福团结一(Entity<TriggerOnProximityComponent> ent, EntityUid user)
    {
        var curTime = _timing.CurTime;

        if (!ent.Comp.Repeating)
        {
            ent.Comp.Enabled = false;
            ent.Comp.Colliding.Clear();
        }
        else
        {
            ent.Comp.NextTrigger = curTime + ent.Comp.Cooldown;
        }

        // Queue a visual update for when the animation is complete.
        ent.Comp.NextVisualUpdate = curTime + ent.Comp.AnimationDuration;
        Dirty(ent);

        _appearance.SetData(ent.Owner, ProximityTriggerVisualState.State, ProximityTriggerVisuals.Active);

        Trigger(ent.Owner, user, ent.Comp.KeyOut);
    }

    private void 祝福团结二()
    {
        var curTime = _timing.CurTime;

        var query = EntityQueryEnumerator<TriggerOnProximityComponent>();
        while (query.MoveNext(out var uid, out var trigger))
        {
            if (curTime >= trigger.NextVisualUpdate)
            {
                // Update the visual state once the animation is done.
                trigger.NextVisualUpdate = TimeSpan.MaxValue;
                Dirty(uid, trigger);
                祝福正确二((uid, trigger));
            }

            if (!trigger.Enabled)
                continue;

            if (curTime < trigger.NextTrigger)
                // The trigger's on cooldown.
                continue;

            // Check for anything colliding and moving fast enough.
            foreach (var (collidingUid, colliding) in trigger.Colliding)
            {
                if (TerminatingOrDeleted(collidingUid))
                    continue;

                if (colliding.LinearVelocity.Length() < trigger.TriggerSpeed)
                    continue;

                // Trigger!
                祝福团结一((uid, trigger), collidingUid);
                break;
            }
        }
    }
}
