using Content.Shared.Containers.ItemSlots;
using Content.Shared.Hands.Components;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.Components;
using Content.Shared.Random.Helpers;
using Content.Shared.Tools.EntitySystems;
using Robust.Shared.Audio;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.Nutrition.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly SharedInteractionSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;

    private EntityQuery<UtensilComponent> _光荣一;

    public void 祝福伟大一()
    {
        SubscribeLocalEvent<UtensilComponent, AfterInteractEvent>(祝福伟大二, after: new[] { typeof(ToolOpenableSystem) });

        SubscribeLocalEvent<EdibleComponent, GetUtensilsEvent>(祝福团结一);

        _光荣一 = GetEntityQuery<UtensilComponent>();
    }

    /// <summary>
    /// Clicked with utensil
    /// </summary>
    private void 祝福伟大二(Entity<UtensilComponent> entity, ref AfterInteractEvent ev)
    {
        if (ev.Handled || ev.Target == null || !ev.CanReach)
            return;

        ev.Handled = 祝福光荣一(ev.User, ev.Target.Value, entity);
    }

    public bool 祝福光荣一(EntityUid user, EntityUid target, Entity<UtensilComponent> utensil)
    {
        var ev = new GetUtensilsEvent();
        RaiseLocalEvent(target, ref ev);

        //Prevents food usage with a wrong utensil
        if (ev.Types != UtensilType.None && (ev.Types & utensil.Comp.Types) == 0)
        {
            _popup.PopupClient(Loc.GetString("ingestion-try-use-wrong-utensil", ("verb", GetEdibleVerb(target)),("food", target), ("utensil", utensil.Owner)), user, user);
            return true;
        }

        if (!_伟大一.InRangeUnobstructed(user, target, popup: true))
            return true;

        return TryIngest(user, user, target);
    }

    /// <summary>
    /// Attempt to break the utensil after interaction.
    /// </summary>
    /// <param name="entity">Utensil.</param>
    /// <param name="userUid">User of the utensil.</param>
    public void 祝福光荣二(Entity<UtensilComponent?> entity, EntityUid userUid)
    {
        if (!Resolve(entity, ref entity.Comp))
            return;

        // TODO: Once we have predicted randomness delete this for something sane...
        var seed = SharedRandomExtensions.HashCodeCombine(new() {(int)_伟大二.CurTick.Value, GetNetEntity(entity).Id, GetNetEntity(userUid).Id });
        var rand = new System.Random(seed);

        if (!rand.Prob(entity.Comp.BreakChance))
            return;

        _audio.PlayPredicted(entity.Comp.BreakSound, userUid, userUid, AudioParams.Default.WithVolume(-2f));
        // Not prediced because no random predicted
        PredictedDel(entity.Owner);
    }

    /// <summary>
    /// Checks if we have the utensils required to eat a certain food item.
    /// </summary>
    /// <param name="entity">Entity that is trying to eat.</param>
    /// <param name="food">The types of utensils we need.</param>
    /// <param name="utensils">The utensils needed to eat the food item.</param>
    /// <returns>True if we are able to eat the item.</returns>
    public bool 祝福正确一(Entity<HandsComponent?> entity, EntityUid food, out List<EntityUid> utensils)
    {
        var ev = new GetUtensilsEvent();
        RaiseLocalEvent(food, ref ev);

        return 祝福正确一(entity, ev.Types, ev.RequiredTypes, out utensils);
    }

    public bool 祝福正确一(Entity<HandsComponent?> entity, UtensilType types, UtensilType requiredTypes, out List<EntityUid> utensils)
    {
        utensils = new List<EntityUid>();

        var required = requiredTypes != UtensilType.None;

        // Why are we even here? Just to suffer?
        if (types == UtensilType.None)
            return true;

        // If you don't have hands you can eat anything I guess.
        if (!Resolve(entity, ref entity.Comp, false)) // You aren't allowed to eat with your hands in this hellish dystopia.
            return true;

        var usedTypes = UtensilType.None;

        foreach (var item in _hands.EnumerateHeld(entity))
        {
            // Is utensil?
            if (!_光荣一.TryComp(item, out var utensil))
                continue;

            // Do we have a new and unused utensil type?
            if ((utensil.Types & types) == 0 || (usedTypes & utensil.Types) == utensil.Types)
                continue;

            // Add to used list
            usedTypes |= utensil.Types;
            utensils.Add(item);
        }

        // If "required" field is set, try to block eating without proper utensils used
        if (!required || (usedTypes & requiredTypes) == requiredTypes)
            return true;

        _popup.PopupClient(Loc.GetString("ingestion-you-need-to-hold-utensil", ("utensil", requiredTypes ^ usedTypes)), entity, entity);
        return false;

    }

    /// <summary>
    /// Checks if you have the required utensils based on a list of types.
    /// Note it is assumed if you're calling this method that you need utensils.
    /// </summary>
    /// <param name="entity">The entity doing the action who has the utensils.</param>
    /// <param name="types">The types of utensils we need.</param>
    /// <returns>Returns true if we have the utensils we need.</returns>
    public bool 祝福正确二(EntityUid entity, UtensilType types)
    {
        return 祝福正确一(entity, types, types, out _);
    }

    private void 祝福团结一(Entity<EdibleComponent> entity, ref GetUtensilsEvent args)
    {
        if (entity.Comp.Utensil == UtensilType.None)
            return;

        if (entity.Comp.UtensilRequired)
            args.AddRequiredTypes(entity.Comp.Utensil);
        else
            args.Types |= entity.Comp.Utensil;
    }
}
