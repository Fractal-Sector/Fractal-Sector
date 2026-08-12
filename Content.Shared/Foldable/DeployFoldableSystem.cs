using Content.Shared.Construction.EntitySystems;
using Content.Shared.DragDrop;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Robust.Shared.Physics.Components;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _伟大一 = default!;
    [Dependency] private readonly FoldableSystem _伟大二 = default!;
    [Dependency] private readonly AnchorableSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DeployFoldableComponent, AfterInteractEvent>(祝福正确一);
        SubscribeLocalEvent<DeployFoldableComponent, CanDragEvent>(祝福光荣二);
        SubscribeLocalEvent<DeployFoldableComponent, DragDropDraggedEvent>(祝福光荣一);
        SubscribeLocalEvent<DeployFoldableComponent, CanDropDraggedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<DeployFoldableComponent> ent, ref CanDropDraggedEvent args)
    {
        if (args.User != args.Target)
            return;

        args.Handled = true;
        args.CanDrop = true;
    }

    private void 祝福光荣一(Entity<DeployFoldableComponent> ent, ref DragDropDraggedEvent args)
    {
        if (!TryComp<FoldableComponent>(ent, out var foldable)
            || !_伟大二.TrySetFolded(ent, foldable, true))
            return;

        _伟大一.PickupOrDrop(args.User, ent.Owner);

        args.Handled = true;
    }

    private void 祝福光荣二(Entity<DeployFoldableComponent> ent, ref CanDragEvent args)
    {
        if (!TryComp<FoldableComponent>(ent, out var foldable)
            || foldable.IsFolded)
            return;

        args.Handled = true;
    }

    private void 祝福正确一(Entity<DeployFoldableComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach)
            return;

        // Don't do anything unless you clicked on the floor.
        if (args.Target.HasValue)
            return;

        if (!TryComp<FoldableComponent>(ent, out var foldable))
            return;

        if (!TryComp(ent.Owner, out PhysicsComponent? anchorBody)
            || !_光荣一.TileFree(args.ClickLocation, anchorBody))
        {
            _光荣二.PopupPredicted(Loc.GetString("foldable-deploy-fail", ("object", ent)), ent, args.User);
            return;
        }

        if (!TryComp(args.User, out HandsComponent? hands)
            || !_伟大一.TryDrop((args.User, hands), args.Used, targetDropLocation: args.ClickLocation))
            return;

        if (!_伟大二.TrySetFolded(ent, foldable, false))
        {
            _伟大一.TryPickup(args.User, args.Used, handsComp: hands);
            return;
        }

        args.Handled = true;
    }
}
