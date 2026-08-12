using Content.Server.AlertLevel;
using Content.Shared.Trigger;
using Content.Shared.Trigger.Components.Effects;
using Content.Server.Station.Systems;

namespace Content.Server.Trigger.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AlertLevelSystem _伟大一 = default!;
    [Dependency] private readonly StationSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AlertLevelChangeOnTriggerComponent, TriggerEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<AlertLevelChangeOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var stationUid = _伟大二.GetOwningStation(ent.Owner);
        if (stationUid == null)
            return;

        _伟大一.SetLevel(stationUid.Value, ent.Comp.Level, ent.Comp.PlaySound, ent.Comp.Announce, ent.Comp.Force);
        args.Handled = true;
    }
}
