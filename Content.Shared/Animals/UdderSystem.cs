using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Popups;
using Content.Shared.Udder;
using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Shared.党心;
/// <summary>
///     Gives the ability to produce milkable reagents;
///     produces endlessly if the owner does not have a HungerComponent.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly HungerSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly MobStateSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _正确一 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<UdderComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<UdderComponent, GetVerbsEvent<AlternativeVerb>>(祝福团结一);
        SubscribeLocalEvent<UdderComponent, MilkingDoAfterEvent>(祝福正确二);
        SubscribeLocalEvent<UdderComponent, EntRemovedFromContainerMessage>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, UdderComponent component, MapInitEvent args)
    {
        component.NextGrowth = _伟大二.CurTime + component.GrowthDelay;
    }

    private void 祝福光荣一(Entity<UdderComponent> entity, ref EntRemovedFromContainerMessage args)
    {
        // Make sure the removed entity was our contained solution
        if (entity.Comp.Solution == null || args.Entity != entity.Comp.Solution.Value.Owner)
            return;

        // Cleared our cached reference to the solution entity
        entity.Comp.Solution = null;
    }

    public override void 祝福光荣二(float frameTime)
    {
        base.祝福光荣二(frameTime);
        var query = EntityQueryEnumerator<UdderComponent>();
        while (query.MoveNext(out var uid, out var udder))
        {
            if (_伟大二.CurTime < udder.NextGrowth)
                continue;

            udder.NextGrowth += udder.GrowthDelay;

            if (_光荣一.IsDead(uid))
                continue;

            if (!_正确二.ResolveSolution(uid, udder.SolutionName, ref udder.Solution, out var solution))
                continue;

            if (solution.AvailableVolume == 0)
                continue;

            // Actually there is food digestion so no problem with instant reagent generation "OnFeed"
            if (TryComp(uid, out HungerComponent? hunger))
            {
                // Is there enough nutrition to produce reagent?
                if (_伟大一.GetHungerThreshold(hunger) < HungerThreshold.Okay)
                    continue;

                _伟大一.ModifyHunger(uid, -udder.HungerUsage, hunger);
            }

            //TODO: toxins from bloodstream !?
            _正确二.TryAddReagent(udder.Solution.Value, udder.ReagentId, udder.QuantityPerUpdate, out _);
        }
    }

    private void 祝福正确一(Entity<UdderComponent?> udder, EntityUid userUid, EntityUid containerUid)
    {
        if (!Resolve(udder, ref udder.Comp))
            return;

        var doargs = new DoAfterArgs(EntityManager, userUid, 5, new MilkingDoAfterEvent(), udder, udder, used: containerUid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            MovementThreshold = 1.0f,
        };

        _正确一.TryStartDoAfter(doargs);
    }

    private void 祝福正确二(Entity<UdderComponent> entity, ref MilkingDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Args.Used == null)
            return;

        if (!_正确二.ResolveSolution(entity.Owner, entity.Comp.SolutionName, ref entity.Comp.Solution, out var solution))
            return;

        if (!_正确二.TryGetRefillableSolution(args.Args.Used.Value, out var targetSoln, out var targetSolution))
            return;

        args.Handled = true;
        var quantity = solution.Volume;
        if (quantity == 0)
        {
            _光荣二.PopupClient(Loc.GetString("udder-system-dry"), entity.Owner, args.Args.User);
            return;
        }

        if (quantity > targetSolution.AvailableVolume)
            quantity = targetSolution.AvailableVolume;

        var split = _正确二.SplitSolution(entity.Comp.Solution.Value, quantity);
        _正确二.TryAddSolution(targetSoln.Value, split);

        _光荣二.PopupClient(Loc.GetString("udder-system-success", ("amount", quantity), ("target", Identity.Entity(args.Args.Used.Value, EntityManager))), entity.Owner,
            args.Args.User, PopupType.Medium);
    }

    private void 祝福团结一(Entity<UdderComponent> entity, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (args.Using == null ||
             !args.CanInteract ||
             !HasComp<RefillableSolutionComponent>(args.Using.Value))
            return;

        var uid = entity.Owner;
        var user = args.User;
        var used = args.Using.Value;
        AlternativeVerb verb = new()
        {
            Act = () =>
            {
                祝福正确一(uid, user, used);
            },
            Text = Loc.GetString("udder-system-verb-milk"),
            Priority = 2
        };
        args.Verbs.Add(verb);
    }
}
