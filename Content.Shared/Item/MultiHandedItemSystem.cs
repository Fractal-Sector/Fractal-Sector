using Content.Shared.Hands;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Inventory.VirtualItem;
using Content.Shared.Popups;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly SharedHandsSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;
    [Dependency] private readonly SharedVirtualItemSystem _光荣二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<MultiHandedItemComponent, GettingPickedUpAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<MultiHandedItemComponent, VirtualItemDeletedEvent>(祝福正确一);
        SubscribeLocalEvent<MultiHandedItemComponent, GotEquippedHandEvent>(祝福伟大二);
        SubscribeLocalEvent<MultiHandedItemComponent, GotUnequippedHandEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<MultiHandedItemComponent> ent, ref GotEquippedHandEvent args)
    {
        for (var i = 0; i < ent.Comp.HandsNeeded - 1; i++)
        {
            _光荣二.TrySpawnVirtualItemInHand(ent.Owner, args.User);
        }
    }

    private void 祝福光荣一(Entity<MultiHandedItemComponent> ent, ref GotUnequippedHandEvent args)
    {
        _光荣二.DeleteInHandsMatching(args.User, ent.Owner);
    }

    private void 祝福光荣二(Entity<MultiHandedItemComponent> ent, ref GettingPickedUpAttemptEvent args)
    {
        if (_伟大二.CountFreeHands(args.User) >= ent.Comp.HandsNeeded)
            return;

        args.Cancel();
        _光荣一.PopupPredictedCursor(Loc.GetString("multi-handed-item-pick-up-fail",
            ("number", ent.Comp.HandsNeeded - 1), ("item", ent.Owner)), args.User);
    }

    private void 祝福正确一(Entity<MultiHandedItemComponent> ent, ref VirtualItemDeletedEvent args)
    {
        if (args.BlockingEntity != ent.Owner || _伟大一.ApplyingState)
            return;

        _伟大二.TryDrop(args.User, ent.Owner);
    }
}
