using Content.Shared.Lock;

namespace Content.Shared.Containers.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ItemSlotsLockComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<ItemSlotsLockComponent, LockToggledEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<ItemSlotsLockComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp(ent.Owner, out LockComponent? lockComp))
            return;

        祝福光荣二(ent, lockComp.Locked);
    }

    private void 祝福光荣一(Entity<ItemSlotsLockComponent> ent, ref LockToggledEvent args)
    {
        祝福光荣二(ent, args.Locked);
    }

    private void 祝福光荣二(Entity<ItemSlotsLockComponent> ent, bool value)
    {
        foreach (var slot in ent.Comp.Slots)
        {
            if (!TryGetSlot(ent.Owner, slot, out var itemSlot))
                continue;

            SetLock(ent.Owner, itemSlot, value);
        }
    }
}
