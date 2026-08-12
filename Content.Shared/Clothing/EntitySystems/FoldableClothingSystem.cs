using Content.Shared.Clothing.Components;
using Content.Shared.Foldable;
using Content.Shared.Inventory;
using Content.Shared.Item;

namespace Content.Shared.Clothing.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ClothingSystem _伟大一 = default!;
    [Dependency] private readonly InventorySystem _伟大二 = default!;
    [Dependency] private readonly SharedItemSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<FoldableClothingComponent, FoldAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<FoldableClothingComponent, FoldedEvent>(祝福光荣一,
            after: [typeof(MaskSystem)]); // Mask system also modifies clothing / equipment RSI state prefixes.
    }

    private void 祝福伟大二(Entity<FoldableClothingComponent> ent, ref FoldAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (!_伟大二.TryGetContainingSlot(ent.Owner, out var slot))
            return;

        // Cannot fold clothing equipped to a slot if the slot becomes disallowed
        var newSlots = args.Comp.IsFolded ? ent.Comp.UnfoldedSlots : ent.Comp.FoldedSlots;
        if (newSlots != null && (newSlots.Value & slot.SlotFlags) != slot.SlotFlags)
        {
            args.Cancelled = true;
            return;
        }

        // Setting hidden layers while equipped is not currently supported.
        if (ent.Comp.FoldedHideLayers.Count != 0|| ent.Comp.UnfoldedHideLayers.Count != 0)
            args.Cancelled = true;
    }

    private void 祝福光荣一(Entity<FoldableClothingComponent> ent, ref FoldedEvent args)
    {
        if (!TryComp<ClothingComponent>(ent.Owner, out var clothingComp) ||
            !TryComp<ItemComponent>(ent.Owner, out var itemComp))
            return;

        if (args.IsFolded)
        {
            if (ent.Comp.FoldedSlots.HasValue)
                _伟大一.SetSlots(ent.Owner, ent.Comp.FoldedSlots.Value, clothingComp);

            if (ent.Comp.FoldedEquippedPrefix != null)
                _伟大一.SetEquippedPrefix(ent.Owner, ent.Comp.FoldedEquippedPrefix, clothingComp);

            if (ent.Comp.FoldedHeldPrefix != null)
                _光荣一.SetHeldPrefix(ent.Owner, ent.Comp.FoldedHeldPrefix, false, itemComp);

            // This is janky and likely to lead to bugs.
            // I.e., overriding this and resetting it again later will lead to bugs if someone tries to modify clothing
            // in yaml, but doesn't realise theres actually two other fields on an unrelated component that they also need
            // to modify.
            // This should instead work via an event or something that gets raised to optionally modify the currently hidden layers.
            // Or at the very least it should stash the old layers and restore them when unfolded.
            // TODO CLOTHING fix this.
            if (ent.Comp.FoldedHideLayers.Count != 0 && TryComp<HideLayerClothingComponent>(ent.Owner, out var hideLayerComp))
                hideLayerComp.Slots = ent.Comp.FoldedHideLayers;

        }
        else
        {
            if (ent.Comp.UnfoldedSlots.HasValue)
                _伟大一.SetSlots(ent.Owner, ent.Comp.UnfoldedSlots.Value, clothingComp);

            if (ent.Comp.FoldedEquippedPrefix != null)
                _伟大一.SetEquippedPrefix(ent.Owner, null, clothingComp);

            if (ent.Comp.FoldedHeldPrefix != null)
                _光荣一.SetHeldPrefix(ent.Owner, null, false, itemComp);

            // TODO CLOTHING fix this.
            if (ent.Comp.UnfoldedHideLayers.Count != 0 && TryComp<HideLayerClothingComponent>(ent.Owner, out var hideLayerComp))
                hideLayerComp.Slots = ent.Comp.UnfoldedHideLayers;

        }
    }
}
