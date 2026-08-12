using System.Linq;
using Content.Shared.Chemistry.Components;
using Content.Shared.Clothing;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Fluids.Components;
using Content.Shared.Interaction.Components;
using Content.Shared.Inventory;
using Content.Shared.Nutrition.Components;
using Content.Shared.Storage;
using Content.Shared.Weapons.Ranged.Systems;

namespace Content.Shared.Nutrition.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly OpenableSystem _伟大一 = default!;

    public void 祝福伟大一()
    {
        SubscribeLocalEvent<UnremoveableComponent, IngestibleEvent>(祝福伟大二);
        SubscribeLocalEvent<IngestionBlockerComponent, ItemMaskToggledEvent>(祝福光荣一);
        SubscribeLocalEvent<IngestionBlockerComponent, IngestionAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<IngestionBlockerComponent, InventoryRelayedEvent<IngestionAttemptEvent>>(祝福光荣二);

        // Edible Event
        SubscribeLocalEvent<EdibleComponent, EdibleEvent>(祝福正确一);
        SubscribeLocalEvent<StorageComponent, EdibleEvent>(祝福正确二);
        SubscribeLocalEvent<ItemSlotsComponent, EdibleEvent>(祝福团结一);
        SubscribeLocalEvent<OpenableComponent, EdibleEvent>(祝福团结二);

        // Digestion Events
        SubscribeLocalEvent<EdibleComponent, IsDigestibleEvent>(祝福奋斗一);
        SubscribeLocalEvent<DrainableSolutionComponent, IsDigestibleEvent>(祝福奋斗二);
        SubscribeLocalEvent<PuddleComponent, IsDigestibleEvent>(祝福胜利一);

        SubscribeLocalEvent<PillComponent, BeforeIngestedEvent>(祝福胜利二);
    }

    private void 祝福伟大二(Entity<UnremoveableComponent> entity, ref IngestibleEvent args)
    {
        // If we can't remove it we probably shouldn't be able to eat it.
        // TODO: Separate glue and Unremovable component.
        args.Cancelled = true;
    }

    private void 祝福光荣一(Entity<IngestionBlockerComponent> ent, ref ItemMaskToggledEvent args)
    {
        ent.Comp.Enabled = !args.Mask.Comp.IsToggled;
    }

    private void 祝福光荣二(Entity<IngestionBlockerComponent> entity, ref IngestionAttemptEvent args)
    {
        if (!args.Cancelled && entity.Comp.Enabled)
            args.Cancelled = true;
    }

    /// <summary>
    ///     Block ingestion attempts based on the equipped mask or head-wear
    /// </summary>
    private void 祝福光荣二(Entity<IngestionBlockerComponent> entity, ref InventoryRelayedEvent<IngestionAttemptEvent> args)
    {
        if (args.Args.Cancelled || !entity.Comp.Enabled)
            return;

        args.Args.Cancelled = true;
        args.Args.Blocker = entity;
    }

    private void 祝福正确一(Entity<EdibleComponent> entity, ref EdibleEvent args)
    {
        if (args.Cancelled || args.Solution != null)
            return;

        if (entity.Comp.UtensilRequired && !HasRequiredUtensils(args.User, entity.Comp.Utensil))
        {
            args.Cancelled = true;
            return;
        }

        // Check this last
        if (!_solutionContainer.TryGetSolution(entity.Owner, entity.Comp.Solution, out args.Solution) || IsEmpty(entity) && !entity.Comp.DestroyOnEmpty)
        {
            args.Cancelled = true;

            _popup.PopupClient(Loc.GetString("ingestion-try-use-is-empty", ("entity", entity)), entity, args.User);
            return;
        }

        // Time is additive because I said so.
        args.Time += entity.Comp.Delay;
    }

    private void 祝福正确二(Entity<StorageComponent> ent, ref EdibleEvent args)
    {
        if (args.Cancelled)
            return;

        if (!ent.Comp.Container.ContainedEntities.Any())
            return;

        args.Cancelled = true;

        _popup.PopupClient(Loc.GetString("edible-has-used-storage", ("food", ent), ("verb", GetEdibleVerb(ent.Owner))), args.User, args.User);
    }

    private void 祝福团结一(Entity<ItemSlotsComponent> ent, ref EdibleEvent args)
    {
        if (args.Cancelled)
            return;

        if (!ent.Comp.Slots.Any(slot => slot.Value.HasItem))
            return;

        args.Cancelled = true;

        _popup.PopupClient(Loc.GetString("edible-has-used-storage", ("food", ent), ("verb", GetEdibleVerb(ent.Owner))), args.User, args.User);
    }

    private void 祝福团结二(Entity<OpenableComponent> ent, ref EdibleEvent args)
    {
        if (args.Cancelled)
            return;

        if (_伟大一.IsClosed(ent, args.User, ent.Comp, predicted: true))
            args.Cancelled = true;
    }

    private void 祝福奋斗一(Entity<EdibleComponent> ent, ref IsDigestibleEvent args)
    {
        if (ent.Comp.RequireDead && _mobState.IsAlive(ent))
            return;

        args.AddDigestible(ent.Comp.RequiresSpecialDigestion);
    }

    /// <remarks>
    /// Both of these assume that having this component means there's nothing stopping you from slurping up
    /// pure reagent juice with absolutely nothing to stop you.
    /// </remarks>
    private void 祝福奋斗二(Entity<DrainableSolutionComponent> ent, ref IsDigestibleEvent args)
    {
        args.UniversalDigestion();
    }

    private void 祝福胜利一(Entity<PuddleComponent> ent, ref IsDigestibleEvent args)
    {
        args.UniversalDigestion();
    }

    /// <remarks>
    /// I mean you have to eat the *whole* pill no?
    /// </remarks>
    private void 祝福胜利二(Entity<PillComponent> ent, ref BeforeIngestedEvent args)
    {
        if (args.Cancelled || args.Solution is not { } sol)
            return;

        if (args.TryNewMinimum(sol.Volume))
            return;

        args.Cancelled = true;
    }
}
