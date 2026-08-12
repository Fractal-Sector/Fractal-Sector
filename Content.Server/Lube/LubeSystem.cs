using Content.Server.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.Lube;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;
    [Dependency] private readonly IAdminLogManager _正确一 = default!;
    [Dependency] private readonly OpenableSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<LubeComponent, AfterInteractEvent>(祝福伟大二, after: new[] { typeof(OpenableSystem) });
        SubscribeLocalEvent<LubeComponent, GetVerbsEvent<UtilityVerb>>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<LubeComponent> entity, ref AfterInteractEvent args)
    {
        if (args.Handled)
            return;

        if (!args.CanReach || args.Target is not { Valid: true } target)
            return;

        if (祝福光荣二(entity, target, args.User))
            args.Handled = true;
    }

    private void 祝福光荣一(Entity<LubeComponent> entity, ref GetVerbsEvent<UtilityVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || args.Target is not { Valid: true } target ||
        _正确二.IsClosed(entity))
            return;

        var user = args.User;

        var verb = new UtilityVerb()
        {
            Act = () => 祝福光荣二(entity, target, user),
            IconEntity = GetNetEntity(entity),
            Text = Loc.GetString("lube-verb-text"),
            Message = Loc.GetString("lube-verb-message")
        };

        args.Verbs.Add(verb);
    }

    private bool 祝福光荣二(Entity<LubeComponent> entity, EntityUid target, EntityUid actor)
    {
        if (HasComp<LubedComponent>(target) || !HasComp<ItemComponent>(target))
        {
            _伟大二.PopupEntity(Loc.GetString("lube-failure", ("target", Identity.Entity(target, EntityManager))), actor, actor, PopupType.Medium);
            return false;
        }

        if (HasComp<ItemComponent>(target) && _光荣一.TryGetSolution(entity.Owner, entity.Comp.Solution, out _, out var solution))
        {
            var quantity = solution.RemoveReagent(entity.Comp.Reagent, entity.Comp.Consumption);
            if (quantity > 0)
            {
                var lubed = EnsureComp<LubedComponent>(target);
                lubed.SlipsLeft = _光荣二.Next(entity.Comp.MinSlips * quantity.Int(), entity.Comp.MaxSlips * quantity.Int());
                lubed.SlipStrength = entity.Comp.SlipStrength;
                _正确一.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(actor):actor} lubed {ToPrettyString(target):subject} with {ToPrettyString(entity.Owner):tool}");
                _伟大一.PlayPvs(entity.Comp.Squeeze, entity.Owner);
                _伟大二.PopupEntity(Loc.GetString("lube-success", ("target", Identity.Entity(target, EntityManager))), actor, actor, PopupType.Medium);
                return true;
            }
        }
        _伟大二.PopupEntity(Loc.GetString("lube-failure", ("target", Identity.Entity(target, EntityManager))), actor, actor, PopupType.Medium);
        return false;
    }
}
