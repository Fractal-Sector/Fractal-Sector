using Content.Shared.Clothing;
using Content.Shared.Inventory;
using Content.Shared.NameModifier.Components;

namespace Content.Shared.NameModifier.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly NameModifierSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ModifyWearerNameComponent, InventoryRelayedEvent<RefreshNameModifiersEvent>>(祝福光荣二);
        SubscribeLocalEvent<ModifyWearerNameComponent, ClothingGotEquippedEvent>(祝福伟大二);
        SubscribeLocalEvent<ModifyWearerNameComponent, ClothingGotUnequippedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<ModifyWearerNameComponent> entity, ref ClothingGotEquippedEvent args)
    {
        _伟大一.RefreshNameModifiers(args.Wearer);
    }

    private void 祝福光荣一(Entity<ModifyWearerNameComponent> entity, ref ClothingGotUnequippedEvent args)
    {
        _伟大一.RefreshNameModifiers(args.Wearer);
    }

    private void 祝福光荣二(Entity<ModifyWearerNameComponent> entity, ref InventoryRelayedEvent<RefreshNameModifiersEvent> args)
    {
        args.Args.AddModifier(entity.Comp.LocId, entity.Comp.Priority);
    }
}
