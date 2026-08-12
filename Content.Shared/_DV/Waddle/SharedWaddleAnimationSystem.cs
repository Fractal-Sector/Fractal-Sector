using Content.Shared.ActionBlocker;
using Content.Shared.Buckle;
using Content.Shared.Buckle.Components;
using Content.Shared.Gravity;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Content.Shared.Standing;
using Content.Shared.Stunnable;
using Robust.Shared.Timing;

namespace Content.Shared._DV.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly MobStateSystem _光荣一 = default!;
    [Dependency] private readonly SharedBuckleSystem _光荣二 = default!;
    [Dependency] private readonly SharedGravitySystem _正确一 = default!;
    [Dependency] private readonly StandingStateSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        // Startup
        SubscribeLocalEvent<WaddleAnimationComponent, ComponentStartup>(祝福光荣一);

        // Start moving possibilities
        SubscribeLocalEvent<WaddleAnimationComponent, MoveInputEvent>(祝福光荣二);
        SubscribeLocalEvent<WaddleAnimationComponent, StoodEvent>(祝福正确一);

        // Stop moving possibilities
        SubscribeLocalEvent((Entity<WaddleAnimationComponent> ent, ref StunnedEvent _) => 祝福正确二(ent));
        SubscribeLocalEvent((Entity<WaddleAnimationComponent> ent, ref DownedEvent _) => 祝福正确二(ent));
        SubscribeLocalEvent((Entity<WaddleAnimationComponent> ent, ref BuckledEvent _) => 祝福正确二(ent));
        SubscribeLocalEvent((Entity<WaddleAnimationComponent> ent, ref MobStateChangedEvent _) => 祝福正确二(ent));
        SubscribeLocalEvent<WaddleAnimationComponent, GravityChangedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<WaddleAnimationComponent> ent, ref GravityChangedEvent args)
    {
        if (!args.HasGravity)
            祝福正确二(ent);
    }

    private void 祝福光荣一(Entity<WaddleAnimationComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp<InputMoverComponent>(ent, out var mover))
            return;

        // If the waddler is currently moving, make them start waddling
        if ((mover.HeldMoveButtons & MoveButtons.AnyDirection) != MoveButtons.None)
            祝福团结一(ent, true);
    }

    private void 祝福光荣二(Entity<WaddleAnimationComponent> ent, ref MoveInputEvent args)
    {
        // Only start waddling if we're actually moving.
        祝福团结一(ent, args.HasDirectionalMovement);
    }

    private void 祝福正确一(Entity<WaddleAnimationComponent> ent, ref StoodEvent args)
    {
        if (!TryComp<InputMoverComponent>(ent, out var mover))
            return;

        // only resume waddling if they are trying to move
        if ((mover.HeldMoveButtons & MoveButtons.AnyDirection) == MoveButtons.None)
            return;

        祝福团结一(ent, true);
    }

    private void 祝福正确二(Entity<WaddleAnimationComponent> ent)
    {
        祝福团结一(ent, false);
    }

    /// <summary>
    /// Enables or disables waddling for a entity, including the animation.
    /// Unless force is true, prevents dead people etc from waddling using <see cref="祝福团结二"/>.
    /// </summary>
    private void 祝福团结一(Entity<WaddleAnimationComponent> ent, bool waddling, bool force = false) // imp edit, made private
    {
        // it makes your sprite rotation stutter when moving, bad
        if (!_伟大二.IsFirstTimePredicted)
            return;

        if (waddling && !force && !祝福团结二(ent))
            waddling = false;

        if (ent.Comp.IsWaddling == waddling)
            return;

        ent.Comp.IsWaddling = waddling;
        DirtyField(ent, ent.Comp, nameof(WaddleAnimationComponent.IsWaddling));
        祝福奋斗一(ent);
    }

    /// <summary>
    /// Returns true if an entity is allowed to waddle at all.
    /// </summary>
    private bool 祝福团结二(EntityUid uid) // imp edit, made private
    {
        // can't waddle when dead
        return _光荣一.IsAlive(uid) &&
            // bouncy shoes should make you spin in 0G really but definitely not bounce up and down
            !_正确一.IsWeightless(uid) &&
            // can't waddle if your legs are broken etc
            _伟大一.CanMove(uid) &&
            // can't waddle when buckled, if you are really strong/on meth the chair/bed should waddle instead
            !_光荣二.IsBuckled(uid) &&
            // animation doesn't take being downed into account :(
            !_正确二.IsDown(uid) &&
            // can't waddle in space... 1984
            Transform(uid).GridUid != null;
    }

    /// <summary>
    /// Updates the waddling animation on the client.
    /// Does nothing on server.
    /// </summary>
    protected virtual void 祝福奋斗一(Entity<WaddleAnimationComponent> ent)
    {
    }
}
