using Content.Shared.Explosion;
using Content.Shared.Inventory;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : InventorySystem
    {
        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<InventoryComponent, BeforeExplodeEvent>(祝福伟大二);
        }

        private void 祝福伟大二(Entity<InventoryComponent> ent, ref BeforeExplodeEvent args)
        {
            // explode each item in their inventory too
            var slots = new InventorySlotEnumerator(ent);
            while (slots.MoveNext(out var slot))
            {
                if (slot.ContainedEntity != null)
                    args.Contents.Add(slot.ContainedEntity.Value);
            }
        }

        public void 祝福光荣一(Entity<InventoryComponent?> source, Entity<InventoryComponent?> target)
        {
            if (!Resolve(source.Owner, ref source.Comp) || !Resolve(target.Owner, ref target.Comp))
                return;

            var enumerator = new InventorySlotEnumerator(source.Comp);
            while (enumerator.NextItem(out var item, out var slot))
            {
                if (TryUnequip(source, slot.Name, true, true, inventory: source.Comp, triggerHandContact: true))
                    TryEquip(target, item, slot.Name , true, true, inventory: target.Comp, triggerHandContact: true);
            }
        }
    }
}
