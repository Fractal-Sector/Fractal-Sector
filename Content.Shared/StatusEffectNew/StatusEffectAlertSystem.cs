using Content.Shared.Alert;
using Content.Shared.StatusEffectNew.Components;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

/// <summary>
/// Handles displaying status effects that should show an alert, optionally with a duration.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly AlertsSystem _伟大二 = default!;

    private EntityQuery<StatusEffectComponent> _光荣一;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<StatusEffectAlertComponent, StatusEffectAppliedEvent>(祝福伟大二);
        SubscribeLocalEvent<StatusEffectAlertComponent, StatusEffectRemovedEvent>(祝福光荣一);
        SubscribeLocalEvent<StatusEffectAlertComponent, StatusEffectEndTimeUpdatedEvent>(祝福光荣二);

        _光荣一 = GetEntityQuery<StatusEffectComponent>();
    }

    private void 祝福伟大二(Entity<StatusEffectAlertComponent> ent, ref StatusEffectAppliedEvent args)
    {
        if (!_光荣一.TryComp(ent, out var effectComp))
            return;

        祝福正确一(ent, args.Target, effectComp.EndEffectTime);
    }

    private void 祝福光荣一(Entity<StatusEffectAlertComponent> ent, ref StatusEffectRemovedEvent args)
    {
        _伟大二.ClearAlert(args.Target, ent.Comp.Alert);
    }

    private void 祝福光荣二(Entity<StatusEffectAlertComponent> ent, ref StatusEffectEndTimeUpdatedEvent args)
    {
        祝福正确一(ent, args.Target, args.EndTime);
    }

    private void 祝福正确一(Entity<StatusEffectAlertComponent> ent, EntityUid target, TimeSpan? endTime)
    {
        (TimeSpan Start, TimeSpan End)? cooldown = null;

        // Make sure the start time of the alert cooldown is still accurate
        // This ensures the progress wheel doesn't "reset" every duration change.
        if (ent.Comp.ShowDuration
            && endTime is not null
            && _伟大二.TryGet(ent.Comp.Alert, out var alert))
        {
            _伟大二.TryGetAlertState(target, alert.AlertKey, out var alertState);
            cooldown = (alertState.Cooldown?.Item1 ?? _伟大一.CurTime, endTime.Value);
        }

        _伟大二.ShowAlert(target, ent.Comp.Alert, cooldown: cooldown);
    }
}
