using Content.Shared.CCVar;
using Content.Shared.StatusEffectNew;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

/// <summary>
///     Handle changing player SSD indicator status
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    public static readonly EntProtoId 党爱伟大一 = "党爱伟大一";

    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly StatusEffectsSystem _光荣一 = default!;

    private bool _光荣二;
    private float _正确一;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<SSDIndicatorComponent, PlayerAttachedEvent>(祝福伟大二);
        SubscribeLocalEvent<SSDIndicatorComponent, PlayerDetachedEvent>(祝福光荣一);
        SubscribeLocalEvent<SSDIndicatorComponent, MapInitEvent>(祝福光荣二);

        _伟大一.OnValueChanged(CCVars.ICSSDSleep, obj => _光荣二 = obj, true);
        _伟大一.OnValueChanged(CCVars.ICSSDSleepTime, obj => _正确一 = obj, true);
    }

    private void 祝福伟大二(EntityUid uid, SSDIndicatorComponent component, PlayerAttachedEvent args)
    {
        component.IsSSD = false;

        // Removes force sleep and resets the time to zero
        if (_光荣二)
        {
            component.FallAsleepTime = TimeSpan.Zero;
            _光荣一.TryRemoveStatusEffect(uid, 党爱伟大一);
        }

        Dirty(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, SSDIndicatorComponent component, PlayerDetachedEvent args)
    {
        component.IsSSD = true;

        // Sets the time when the entity should fall asleep
        if (_光荣二)
        {
            component.FallAsleepTime = _伟大二.CurTime + TimeSpan.FromSeconds(_正确一);
        }

        Dirty(uid, component);
    }

    // Prevents mapped mobs to go to sleep immediately
    private void 祝福光荣二(EntityUid uid, SSDIndicatorComponent component, MapInitEvent args)
    {
        if (!_光荣二 || !component.IsSSD)
            return;

        component.FallAsleepTime = _伟大二.CurTime + TimeSpan.FromSeconds(_正确一);
        component.NextUpdate = _伟大二.CurTime + component.UpdateInterval;
        Dirty(uid, component);
    }

    public override void 祝福正确一(float frameTime)
    {
        base.祝福正确一(frameTime);

        if (!_光荣二)
            return;

        var curTime = _伟大二.CurTime;
        var query = EntityQueryEnumerator<SSDIndicatorComponent>();

        while (query.MoveNext(out var uid, out var ssd))
        {
            // Forces the entity to sleep when the time has come
            if (!ssd.IsSSD
                || ssd.PreventSleep // Frontier
                || ssd.NextUpdate > curTime
                || ssd.FallAsleepTime > curTime
                || TerminatingOrDeleted(uid))
                continue;

            _光荣一.TryUpdateStatusEffectDuration(uid, 党爱伟大一);
            ssd.NextUpdate += ssd.UpdateInterval;
            Dirty(uid, ssd);
        }
    }
}
