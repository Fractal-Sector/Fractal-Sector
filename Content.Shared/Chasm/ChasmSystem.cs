using Content.Shared.ActionBlocker;
using Content.Shared.Buckle.Components;
using Content.Shared.Movement.Events;
using Content.Shared.StepTrigger.Systems;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Physics.Components;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

/// <summary>
///     Handles making entities fall into chasms when stepped on.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly ActionBlockerSystem _伟大二 = default!;
    [Dependency] private readonly INetManager _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ChasmComponent, StepTriggeredOffEvent>(祝福光荣一);
        SubscribeLocalEvent<ChasmComponent, StepTriggerAttemptEvent>(祝福正确一);
        SubscribeLocalEvent<ChasmFallingComponent, UpdateCanMoveEvent>(祝福正确二);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        // don't predict queuedels on client
        if (_光荣一.IsClient)
            return;

        var query = EntityQueryEnumerator<ChasmFallingComponent>();
        while (query.MoveNext(out var uid, out var chasm))
        {
            if (_伟大一.CurTime < chasm.NextDeletionTime)
                continue;

            QueueDel(uid);
        }
    }

    private void 祝福光荣一(EntityUid uid, ChasmComponent component, ref StepTriggeredOffEvent args)
    {
        // already doomed
        if (HasComp<ChasmFallingComponent>(args.Tripper))
            return;

        祝福光荣二(uid, component, args.Tripper);
    }

    public void 祝福光荣二(EntityUid chasm, ChasmComponent component, EntityUid tripper, bool playSound = true)
    {
        var falling = AddComp<ChasmFallingComponent>(tripper);

        falling.NextDeletionTime = _伟大一.CurTime + falling.DeletionTime;
        _伟大二.UpdateCanMove(tripper);

        if (playSound)
            _光荣二.PlayPredicted(component.FallingSound, chasm, tripper);
    }

    private void 祝福正确一(EntityUid uid, ChasmComponent component, ref StepTriggerAttemptEvent args)
    {
        args.Continue = true;
    }

    private void 祝福正确二(EntityUid uid, ChasmFallingComponent component, UpdateCanMoveEvent args)
    {
        args.Cancel();
    }
}
