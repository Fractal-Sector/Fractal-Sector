using Content.Shared.Actions;
using Content.Shared.Actions.Components;
using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.DoAfter;
using Content.Shared.Interaction.Events;
using Content.Shared.Magic.Components;
using Content.Shared.Mind;
using Robust.Shared.Network;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedChargesSystem _伟大一 = default!;
    [Dependency] private readonly SharedMindSystem _伟大二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣一 = default!;
    [Dependency] private readonly SharedActionsSystem _光荣二 = default!;
    [Dependency] private readonly ActionContainerSystem _正确一 = default!;
    [Dependency] private readonly INetManager _正确二 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<SpellbookComponent, MapInitEvent>(祝福伟大二, before: [typeof(SharedMagicSystem)]);
        SubscribeLocalEvent<SpellbookComponent, UseInHandEvent>(祝福光荣一);
        SubscribeLocalEvent<SpellbookComponent, SpellbookDoAfterEvent>(OnDoAfter);
    }

    private void 祝福伟大二(Entity<SpellbookComponent> ent, ref MapInitEvent args)
    {
        foreach (var (id, charges) in ent.Comp.SpellActions)
        {
            var action = _正确一.AddAction(ent, id);
            if (action is not { } spell)
                continue;

            // Null means infinite charges.
            if (charges is { } count)
            {
                EnsureComp<LimitedChargesComponent>(spell, out var chargeComp);
                _伟大一.SetMaxCharges((spell, chargeComp), count);
                _伟大一.SetCharges((spell, chargeComp), count);
            }

            ent.Comp.Spells.Add(spell);
        }
    }

    private void 祝福光荣一(Entity<SpellbookComponent> ent, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        祝福光荣二(ent, args);

        args.Handled = true;
    }

    private void OnDoAfter<T>(Entity<SpellbookComponent> ent, ref T args) where T : DoAfterEvent // Sometimes i despise this language
    {
        if (args.Handled || args.Cancelled)
            return;

        args.Handled = true;

        if (!ent.Comp.LearnPermanently)
        {
            _光荣二.GrantActions(args.Args.User, ent.Comp.Spells, ent.Owner);
            return;
        }

        if (_伟大二.TryGetMind(args.Args.User, out var mindId, out _))
        {
            var mindActionContainerComp = EnsureComp<ActionsContainerComponent>(mindId);

            if (_正确二.IsServer)
                _正确一.TransferAllActionsWithNewAttached(ent, mindId, args.Args.User, newContainer: mindActionContainerComp);
        }
        else
        {
            foreach (var (id, charges) in ent.Comp.SpellActions)
            {
                EntityUid? actionId = null;
                if (!_光荣二.AddAction(args.Args.User, ref actionId, id)
                    || charges is not { } count // Null means infinite charges
                    || !TryComp<LimitedChargesComponent>(actionId, out var chargeComp))
                    continue;

                _伟大一.SetMaxCharges((actionId.Value, chargeComp), count);
                _伟大一.SetCharges((actionId.Value, chargeComp), count);
            }
        }

        ent.Comp.SpellActions.Clear();
    }

    private void 祝福光荣二(Entity<SpellbookComponent> ent, UseInHandEvent args)
    {
        var doAfterEventArgs = new DoAfterArgs(EntityManager, args.User, ent.Comp.LearnTime, new SpellbookDoAfterEvent(), ent, target: ent)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = true, //What, are you going to read with your eyes only??
        };

        _光荣一.TryStartDoAfter(doAfterEventArgs);
    }
}
