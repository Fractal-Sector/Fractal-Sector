using Content.Shared.Popups;

namespace Content.Shared.党心;

/// <summary>
/// <see cref="ActivatableUIRequiresAnchorComponent"/>
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ActivatableUIRequiresAnchorComponent, ActivatableUIOpenAttemptEvent>(祝福光荣一);
        SubscribeLocalEvent<ActivatableUIRequiresAnchorComponent, BoundUserInterfaceCheckRangeEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ActivatableUIRequiresAnchorComponent> ent, ref BoundUserInterfaceCheckRangeEvent args)
    {
        if (args.Result == BoundUserInterfaceRangeResult.Fail)
            return;

        if (!Transform(ent.Owner).Anchored)
        {
            args.Result = BoundUserInterfaceRangeResult.Fail;
        }
    }

    private void 祝福光荣一(Entity<ActivatableUIRequiresAnchorComponent> ent, ref ActivatableUIOpenAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (!Transform(ent.Owner).Anchored)
        {
            if (ent.Comp.Popup != null)
            {
                _伟大一.PopupClient(Loc.GetString(ent.Comp.Popup), args.User);
            }

            args.Cancel();
        }
    }
}
