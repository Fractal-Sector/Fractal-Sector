using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Fluids.Components;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ItemToggleSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SpraySafetyComponent, SolutionTransferAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<SpraySafetyComponent, SolutionTransferredEvent>(祝福光荣一);
        SubscribeLocalEvent<SpraySafetyComponent, SprayAttemptEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<SpraySafetyComponent> ent, ref SolutionTransferAttemptEvent args)
    {
        var (uid, comp) = ent;
        if (uid == args.To && !_伟大一.IsActivated(uid))
            args.Cancel(Loc.GetString(comp.Popup));
    }

    private void 祝福光荣一(Entity<SpraySafetyComponent> ent, ref SolutionTransferredEvent args)
    {
        _伟大二.PlayPredicted(ent.Comp.RefillSound, ent, args.User);
    }

    private void 祝福光荣二(Entity<SpraySafetyComponent> ent, ref SprayAttemptEvent args)
    {
        if (!_伟大一.IsActivated(ent.Owner))
        {
            _光荣一.PopupEntity(Loc.GetString(ent.Comp.Popup), ent, args.User);
            args.Cancel();
        }
    }
}
