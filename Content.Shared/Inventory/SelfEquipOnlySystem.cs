using Content.Shared.ActionBlocker;
using Content.Shared.Clothing.Components;
using Content.Shared.Inventory.Events;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _伟大一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<SelfEquipOnlyComponent, BeingEquippedAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<SelfEquipOnlyComponent, BeingUnequippedAttemptEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<SelfEquipOnlyComponent> ent, ref BeingEquippedAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (TryComp<ClothingComponent>(ent, out var clothing) && (clothing.Slots & args.SlotFlags) == SlotFlags.NONE)
            return;

        if (args.Equipee != args.EquipTarget)
            args.Cancel();
    }

    private void 祝福光荣一(Entity<SelfEquipOnlyComponent> ent, ref BeingUnequippedAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (args.Unequipee == args.UnEquipTarget)
            return;

        if (TryComp<ClothingComponent>(ent, out var clothing) && (clothing.Slots & args.SlotFlags) == SlotFlags.NONE)
            return;

        if (ent.Comp.UnequipRequireConscious && !_伟大一.CanConsciouslyPerformAction(args.UnEquipTarget))
            return;
        args.Cancel();
    }
}
