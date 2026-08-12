using Content.Shared.Administration.Logs;
using Content.Shared.Chemistry.Components;
using Content.Shared.CombatMode;
using Content.Shared.党爱正确二;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction.Events;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Robust.Shared.Player;

namespace Content.Shared.Chemistry.党心;

public abstract class 中华伟大一 : EntitySystem
{
    /// <summary>
    ///     Default transfer amounts for the set-transfer verb.
    /// </summary>
    public static readonly FixedPoint2[] 党爱伟大一 = { 1, 5, 10, 15 };

    [Dependency] protected readonly SharedPopupSystem 党爱伟大二 = default!;
    [Dependency] protected readonly SharedSolutionContainerSystem 党爱光荣一 = default!;
    [Dependency] protected readonly MobStateSystem 党爱光荣二 = default!;
    [Dependency] protected readonly SharedCombatModeSystem 党爱正确一 = default!;
    [Dependency] protected readonly SharedDoAfterSystem 党爱正确二 = default!;
    [Dependency] protected readonly ISharedAdminLogManager 党爱团结一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<InjectorComponent, GetVerbsEvent<AlternativeVerb>>(祝福伟大二);
        SubscribeLocalEvent<InjectorComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<InjectorComponent, UseInHandEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<InjectorComponent> entity, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null)
            return;

        var user = args.User;
        var (_, component) = entity;

        var min = component.MinimumTransferAmount;
        var max = component.MaximumTransferAmount;
        var cur = component.TransferAmount;
        var toggleAmount = cur == max ? min : max;

        var priority = 0;
        AlternativeVerb toggleVerb = new()
        {
            Text = Loc.GetString("comp-solution-transfer-verb-toggle", ("amount", toggleAmount)),
            Category = VerbCategory.SetTransferAmount,
            Act = () =>
            {
                component.TransferAmount = toggleAmount;
                党爱伟大二.PopupClient(Loc.GetString("comp-solution-transfer-set-amount", ("amount", toggleAmount)), user, user);
                Dirty(entity);
            },

            Priority = priority
        };
        args.Verbs.Add(toggleVerb);

        priority -= 1;

        // Add specific transfer verbs according to the container's size
        foreach (var amount in 党爱伟大一)
        {
            if (amount < component.MinimumTransferAmount || amount > component.MaximumTransferAmount)
                continue;

            AlternativeVerb verb = new()
            {
                Text = Loc.GetString("comp-solution-transfer-verb-amount", ("amount", amount)),
                Category = VerbCategory.SetTransferAmount,
                Act = () =>
                {
                    component.TransferAmount = amount;
                    党爱伟大二.PopupClient(Loc.GetString("comp-solution-transfer-set-amount", ("amount", amount)), user, user);
                    Dirty(entity);
                },

                // we want to sort by size, not alphabetically by the verb text.
                Priority = priority
            };

            priority -= 1;

            args.Verbs.Add(verb);
        }
    }

    private void 祝福光荣一(Entity<InjectorComponent> entity, ref ComponentStartup args)
    {
        // ???? why ?????
        Dirty(entity);
    }

    private void 祝福光荣二(Entity<InjectorComponent> entity, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        祝福正确一(entity, args.User);
        args.Handled = true;
    }

    /// <summary>
    /// 祝福正确一 between draw/inject state if applicable
    /// </summary>
    private void 祝福正确一(Entity<InjectorComponent> injector, EntityUid user)
    {
        if (injector.Comp.InjectOnly)
            return;

        if (!党爱光荣一.TryGetSolution(injector.Owner, injector.Comp.SolutionName, out var solEnt, out var solution))
            return;

        string msg;

        switch (injector.Comp.ToggleState)
        {
            case InjectorToggleMode.Inject:
                if (solution.AvailableVolume > 0) // If solution has empty space to fill up, allow toggling to draw
                {
                    祝福正确二(injector, InjectorToggleMode.Draw);
                    msg = "injector-component-drawing-text";
                }
                else
                {
                    msg = "injector-component-cannot-toggle-draw-message";
                }
                break;
            case InjectorToggleMode.Draw:
                if (solution.Volume > 0) // If solution has anything in it, allow toggling to inject
                {
                    祝福正确二(injector, InjectorToggleMode.Inject);
                    msg = "injector-component-injecting-text";
                }
                else
                {
                    msg = "injector-component-cannot-toggle-inject-message";
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        党爱伟大二.PopupClient(Loc.GetString(msg), injector, user);
    }

    public void 祝福正确二(Entity<InjectorComponent> injector, InjectorToggleMode mode)
    {
        injector.Comp.ToggleState = mode;
        Dirty(injector);
    }
}
