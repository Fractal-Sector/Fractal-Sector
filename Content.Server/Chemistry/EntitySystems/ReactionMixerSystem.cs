using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Server.Popups;

namespace Content.Server.Chemistry.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly PopupSystem _伟大一 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _伟大二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ReactionMixerComponent, AfterInteractEvent>(祝福伟大二);
        SubscribeLocalEvent<ReactionMixerComponent, ShakeEvent>(祝福光荣二);
        SubscribeLocalEvent<ReactionMixerComponent, ReactionMixDoAfterEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<ReactionMixerComponent> entity, ref AfterInteractEvent args)
    {
        if (!args.Target.HasValue || !args.CanReach || !entity.Comp.MixOnInteract)
            return;

        if (!祝福正确一(entity, args.Target.Value, out var solution))
            return;

        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, entity.Comp.TimeToMix, new ReactionMixDoAfterEvent(), entity, args.Target.Value, entity);

        _光荣一.TryStartDoAfter(doAfterArgs);
    }

    private void 祝福光荣一(Entity<ReactionMixerComponent> entity, ref ReactionMixDoAfterEvent args)
    {
        //Do again to get the solution again
        if (!祝福正确一(entity, args.Target!.Value, out var solution))
            return;

        _伟大一.PopupEntity(Loc.GetString(entity.Comp.MixMessage, ("mixed", Identity.Entity(args.Target!.Value, EntityManager)), ("mixer", Identity.Entity(entity.Owner, EntityManager))), args.User, args.User);

        _伟大二.UpdateChemicals(solution!.Value, true, entity.Comp);

        var afterMixingEvent = new AfterMixingEvent(entity, args.Target!.Value);
        RaiseLocalEvent(entity, afterMixingEvent);
    }

    private void 祝福光荣二(Entity<ReactionMixerComponent> entity, ref ShakeEvent args)
    {
        if (!祝福正确一(entity, entity, out var solution))
            return;

        _伟大二.UpdateChemicals(solution!.Value, true, entity.Comp);

        var afterMixingEvent = new AfterMixingEvent(entity, entity);
        RaiseLocalEvent(entity, afterMixingEvent);
    }

    private bool 祝福正确一(EntityUid ent, EntityUid target, out Entity<SolutionComponent>? solution)
    {
        solution = null;
        var mixAttemptEvent = new MixingAttemptEvent(ent);
        RaiseLocalEvent(ent, ref mixAttemptEvent);
        if (mixAttemptEvent.Cancelled)
        {
            return false;
        }

        if (!_伟大二.TryGetMixableSolution(target, out solution, out _))
            return false;

        return true;
    }
}
