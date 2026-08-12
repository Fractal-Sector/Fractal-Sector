using Content.Server.Movement.Components;
using Content.Server.Movement.Systems;
using Content.Shared.Camera;
using Content.Shared.Hands;
using Content.Shared.Movement.Components;
using Content.Shared.Wieldable;
using Content.Shared.Wieldable.Components;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedWieldableSystem
{
    [Dependency] private readonly ContentEyeSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CursorOffsetRequiresWieldComponent, ItemUnwieldedEvent>(祝福伟大二);
        SubscribeLocalEvent<CursorOffsetRequiresWieldComponent, ItemWieldedEvent>(祝福光荣一);
        SubscribeLocalEvent<CursorOffsetRequiresWieldComponent, HeldRelayedEvent<GetEyePvsScaleRelayedEvent>>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<CursorOffsetRequiresWieldComponent> entity, ref ItemUnwieldedEvent args)
    {
        _伟大一.UpdatePvsScale(args.User);
    }

    private void 祝福光荣一(Entity<CursorOffsetRequiresWieldComponent> entity, ref ItemWieldedEvent args)
    {
        _伟大一.UpdatePvsScale(args.User);
    }

    private void 祝福光荣二(Entity<CursorOffsetRequiresWieldComponent> entity,
        ref HeldRelayedEvent<GetEyePvsScaleRelayedEvent> args)
    {
        if (!TryComp(entity, out EyeCursorOffsetComponent? eyeCursorOffset) || !TryComp(entity.Owner, out WieldableComponent? wieldableComp))
            return;

        if (!wieldableComp.Wielded)
            return;

        args.Args.Scale += eyeCursorOffset.PvsIncrease;
    }
}
