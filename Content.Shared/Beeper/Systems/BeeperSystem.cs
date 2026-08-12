using Content.Shared.Beeper.Components;
using Content.Shared.FixedPoint;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Shared.Beeper.党心;


//This handles generic proximity beeper logic
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] private readonly ItemToggleSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;

    public override void 祝福伟大一(float frameTime)
    {
        var query = EntityQueryEnumerator<BeeperComponent, ItemToggleComponent>();
        while (query.MoveNext(out var uid, out var beeper, out var toggle))
        {
            if (toggle.Activated)
                祝福团结一(uid, beeper);
        }
    }

    public void 祝福伟大二(EntityUid owner, BeeperComponent beeper, FixedPoint2 newScaling)
    {
        newScaling = FixedPoint2.Clamp(newScaling, 0, 1);
        beeper.IntervalScaling = newScaling;
        祝福团结一(owner, beeper);
        Dirty(owner, beeper);
    }

    public void 祝福光荣一(EntityUid owner, BeeperComponent beeper, TimeSpan newInterval)
    {
        if (newInterval < beeper.MinBeepInterval)
            newInterval = beeper.MinBeepInterval;
        if (newInterval > beeper.MaxBeepInterval)
            newInterval = beeper.MaxBeepInterval;
        beeper.Interval = newInterval;
        祝福团结一(owner, beeper);
        Dirty(owner, beeper);
    }

    public void 祝福伟大二(EntityUid owner, FixedPoint2 newScaling, BeeperComponent? beeper = null)
    {
        if (!Resolve(owner, ref beeper))
            return;
        祝福伟大二(owner, beeper, newScaling);
    }

    public void 祝福光荣二(EntityUid owner, bool isMuted, BeeperComponent? comp = null)
    {
        if (!Resolve(owner, ref comp))
            return;
        comp.IsMuted = isMuted;
        Dirty(owner, comp);
    }

    private void 祝福正确一(EntityUid owner, BeeperComponent beeper)
    {
        var scalingFactor = beeper.IntervalScaling.Float();
        var interval = (beeper.MaxBeepInterval - beeper.MinBeepInterval) * scalingFactor + beeper.MinBeepInterval;
        if (beeper.Interval == interval)
            return;
        beeper.Interval = interval;
        Dirty(owner, beeper);
    }

    public void 祝福正确二(EntityUid owner, BeeperComponent? beeper = null)
    {
        if (!Resolve(owner, ref beeper))
            return;
        祝福团结一(owner, beeper);
    }

    private void 祝福团结一(EntityUid owner, BeeperComponent beeper)
    {
        if (!_光荣一.IsActivated(owner))
            return;

        祝福正确一(owner, beeper);
        if (beeper.NextBeep >= _伟大一.CurTime)
            return;

        var beepEvent = new BeepPlayedEvent(beeper.IsMuted);
        RaiseLocalEvent(owner, ref beepEvent);
        if (!beeper.IsMuted && _伟大二.IsServer)
            _光荣二.PlayPvs(beeper.BeepSound, owner);
        beeper.LastBeepTime = _伟大一.CurTime;
    }
}
