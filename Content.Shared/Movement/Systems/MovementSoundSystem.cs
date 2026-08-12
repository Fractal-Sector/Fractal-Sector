using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Shared.Movement.党心;

/// <summary>
/// Plays a sound on MoveInputEvent.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<MovementSoundComponent, MoveInputEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<MovementSoundComponent> ent, ref MoveInputEvent args)
    {
        if (!_伟大一.IsFirstTimePredicted)
            return;

        var oldMoving = (SharedMoverController.GetNormalizedMovement(args.OldMovement) & MoveButtons.AnyDirection) != MoveButtons.None;
        var moving = (SharedMoverController.GetNormalizedMovement(args.Entity.Comp.HeldMoveButtons) & MoveButtons.AnyDirection) != MoveButtons.None;

        if (oldMoving == moving)
            return;

        if (moving)
        {
            DebugTools.Assert(ent.Comp.SoundEntity == null);
            ent.Comp.SoundEntity = _伟大二.PlayPredicted(ent.Comp.Sound, ent.Owner, ent.Owner)?.Entity;
        }
        else
        {
            ent.Comp.SoundEntity = _伟大二.Stop(ent.Comp.SoundEntity);
        }
    }
}
