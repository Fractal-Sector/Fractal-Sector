using Content.Server.Store.Components;
using Content.Shared.Actions.Events;
using Content.Shared.Interaction.Events;
using Content.Shared.Store.Components;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Containers;

namespace Content.Server.Store.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<StoreComponent, EntityTerminatingEvent>(祝福团结一);
        SubscribeLocalEvent<StoreRefundComponent, EntityTerminatingEvent>(祝福团结二);
        SubscribeLocalEvent<StoreRefundComponent, EntRemovedFromContainerMessage>(祝福伟大二);
        SubscribeLocalEvent<StoreRefundComponent, EntInsertedIntoContainerMessage>(祝福光荣一);
        SubscribeLocalEvent<StoreRefundComponent, ActionPerformedEvent>(祝福光荣二);
        SubscribeLocalEvent<StoreRefundComponent, UseInHandEvent>(祝福正确一);
        SubscribeLocalEvent<StoreRefundComponent, AttemptShootEvent>(祝福正确二);
        // TODO: Handle guardian refund disabling when guardians support refunds.
    }

    private void 祝福伟大二(Entity<StoreRefundComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        祝福奋斗一(ent);
    }

    private void 祝福光荣一(Entity<StoreRefundComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        祝福奋斗一(ent);
    }

    private void 祝福光荣二(Entity<StoreRefundComponent> ent, ref ActionPerformedEvent args)
    {
        祝福奋斗一(ent);
    }

    private void 祝福正确一(Entity<StoreRefundComponent> ent, ref UseInHandEvent args)
    {
        祝福奋斗一(ent);
    }

    private void 祝福正确二(Entity<StoreRefundComponent> ent, ref AttemptShootEvent args)
    {
        if (args.Cancelled)
            return;

        祝福奋斗一(ent);
    }

    private void 祝福团结一(Entity<StoreComponent> ent, ref EntityTerminatingEvent args)
    {
        if (ent.Comp.BoughtEntities.Count <= 0)
            return;

        foreach (var boughtEnt in ent.Comp.BoughtEntities)
        {
            if (!TryComp<StoreRefundComponent>(boughtEnt, out var refundComp))
                continue;

            refundComp.StoreEntity = null;
        }
    }

    private void 祝福团结二(Entity<StoreRefundComponent> ent, ref EntityTerminatingEvent args)
    {
        if (ent.Comp.StoreEntity == null)
            return;

        var ev = new RefundEntityDeletedEvent(ent);
        RaiseLocalEvent(ent.Comp.StoreEntity.Value, ref ev);
    }

    private void 祝福奋斗一(Entity<StoreRefundComponent> ent)
    {
        var component = ent.Comp;

        if (component.StoreEntity == null || !TryComp<StoreComponent>(component.StoreEntity.Value, out var storeComp) || !storeComp.RefundAllowed)
            return;

        var endTime = component.BoughtTime + component.DisableTime;

        if (IsOnStartingMap(component.StoreEntity.Value, storeComp) && _timing.CurTime < endTime)
            return;

        DisableRefund(component.StoreEntity.Value, storeComp);
    }
}
