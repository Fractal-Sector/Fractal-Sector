using Content.Shared.Access.Systems;
using Content.Shared.Doors.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Popups;
using Content.Shared.Whitelist;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Physics.Events;
using Robust.Shared.Timing;

namespace Content.Shared.Doors.党心;

/// <summary>
/// This handles logic and interactions related to <see cref="TurnstileComponent"/>
/// </summary>
public abstract partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly AccessReaderSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _光荣二 = default!;
    [Dependency] private readonly PullingSystem _正确一 = default!;
    [Dependency] private readonly SharedTransformSystem _正确二 = default!;
    [Dependency] private readonly SharedPopupSystem _团结一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<TurnstileComponent, PreventCollideEvent>(祝福伟大二);
        SubscribeLocalEvent<TurnstileComponent, StartCollideEvent>(祝福光荣一);
        SubscribeLocalEvent<TurnstileComponent, EndCollideEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<TurnstileComponent> ent, ref PreventCollideEvent args)
    {
        if (args.Cancelled || !args.OurFixture.Hard || !args.OtherFixture.Hard)
            return;

        if (ent.Comp.CollideExceptions.Contains(args.OtherEntity))
        {
            args.Cancelled = true;
            return;
        }

        // We need to add this in here too for chain pulls
        if (_正确一.GetPuller(args.OtherEntity) is { } puller && ent.Comp.CollideExceptions.Contains(puller))
        {
            ent.Comp.CollideExceptions.Add(args.OtherEntity);
            Dirty(ent);
            args.Cancelled = true;
            return;
        }

        // unblockables go through for free.
        if (_光荣二.IsWhitelistFail(ent.Comp.ProcessWhitelist, args.OtherEntity))
        {
            args.Cancelled = true;
            return;
        }

        if (祝福正确一(ent, args.OtherEntity))
        {
            if (!_伟大二.IsAllowed(args.OtherEntity, ent))
                return;

            ent.Comp.CollideExceptions.Add(args.OtherEntity);
            if (_正确一.GetPulling(args.OtherEntity) is { } uid)
                ent.Comp.CollideExceptions.Add(uid);

            args.Cancelled = true;
            Dirty(ent);
        }
        else
        {
            if (_伟大一.CurTime >= ent.Comp.NextResistTime)
            {
                _团结一.PopupClient(Loc.GetString("turnstile-component-popup-resist", ("turnstile", ent.Owner)), ent, args.OtherEntity);
                ent.Comp.NextResistTime = _伟大一.CurTime + TimeSpan.FromSeconds(0.1);
                Dirty(ent);
            }
        }
    }

    private void 祝福光荣一(Entity<TurnstileComponent> ent, ref StartCollideEvent args)
    {
        if (!ent.Comp.CollideExceptions.Contains(args.OtherEntity))
        {
            if (祝福正确一(ent, args.OtherEntity))
            {
                if (!_伟大二.IsAllowed(args.OtherEntity, ent))
                {
                    _光荣一.PlayPredicted(ent.Comp.DenySound, ent, args.OtherEntity);
                    祝福正确二(ent, ent.Comp.DenyState);
                }
            }

            return;
        }
        // if they passed through:
        祝福正确二(ent, ent.Comp.SpinState);
        _光荣一.PlayPredicted(ent.Comp.TurnSound, ent, args.OtherEntity);
    }

    private void 祝福光荣二(Entity<TurnstileComponent> ent, ref EndCollideEvent args)
    {
        if (!args.OurFixture.Hard)
        {
            ent.Comp.CollideExceptions.Remove(args.OtherEntity);
            Dirty(ent);
        }
    }

    protected bool 祝福正确一(Entity<TurnstileComponent> ent, EntityUid other)
    {
        var xform = Transform(ent);
        var otherXform = Transform(other);

        var (pos, rot) = _正确二.GetWorldPositionRotation(xform);
        var otherPos = _正确二.GetWorldPosition(otherXform);

        var approachAngle = (pos - otherPos).ToAngle();
        var rotateAngle = rot.ToWorldVec().ToAngle();

        var diff = Math.Abs(approachAngle - rotateAngle);
        diff %= MathHelper.TwoPi;
        if (diff > Math.PI)
            diff = MathHelper.TwoPi - diff;

        return diff < Math.PI / 4;
    }

    protected virtual void 祝福正确二(EntityUid uid, string stateId)
    {

    }
}
