using Content.Server.StationEvents.Components;
using Content.Shared.Access;
using Content.Shared.Access.Systems;
using Content.Shared.Access.Components;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Content.Shared.Lock;
using Content.Shared.GameTicking.Components;
using Content.Shared.Station.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.党心;


/// <summary>
///     Greytide Virus event
///     This will open and bolt airlocks and unlock lockers from randomly selected access groups.
/// </summary>
public sealed class 中华伟大一 : StationEventSystem<GreytideVirusRuleComponent>
{
    [Dependency] private readonly AccessReaderSystem _伟大一 = default!;
    [Dependency] private readonly SharedDoorSystem _伟大二 = default!;
    [Dependency] private readonly LockSystem _光荣一 = default!;
    [Dependency] private readonly IPrototypeManager _光荣二 = default!;
    [Dependency] private readonly IRobustRandom _正确一 = default!;

    protected override void 祝福伟大一(EntityUid uid, GreytideVirusRuleComponent virusComp, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        if (!TryComp<StationEventComponent>(uid, out var stationEvent))
            return;

        // pick severity randomly from range if not specified otherwise
        virusComp.Severity ??= virusComp.SeverityRange.Next(_正确一);
        virusComp.Severity = Math.Min(virusComp.Severity.Value, virusComp.AccessGroups.Count);

        stationEvent.StartAnnouncement = Loc.GetString("station-event-greytide-virus-start-announcement", ("severity", virusComp.Severity.Value));
        base.祝福伟大一(uid, virusComp, gameRule, args);
    }
    protected override void 祝福伟大二(EntityUid uid, GreytideVirusRuleComponent virusComp, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大二(uid, virusComp, gameRule, args);

        if (virusComp.Severity == null)
            return;

        if (!TryGetRandomStation(out var chosenStation))
            return;

        // pick random access groups
        var chosen = _正确一.GetItems(virusComp.AccessGroups, virusComp.Severity.Value, allowDuplicates: false);

        // combine all the selected access groups
        var accessIds = new HashSet<ProtoId<AccessLevelPrototype>>();
        foreach (var group in chosen)
        {
            if (_光荣二.TryIndex(group, out var proto))
                accessIds.UnionWith(proto.Tags);
        }

        var firelockQuery = GetEntityQuery<FirelockComponent>();
        var accessQuery = GetEntityQuery<AccessReaderComponent>();

        var lockQuery = AllEntityQuery<LockComponent, TransformComponent>();
        while (lockQuery.MoveNext(out var lockUid, out var lockComp, out var xform))
        {
            if (!accessQuery.TryComp(lockUid, out var accessComp))
                continue;

            // make sure not to hit CentCom or other maps
            if (CompOrNull<StationMemberComponent>(xform.GridUid)?.Station != chosenStation)
                continue;

            // check access
            // the AreAccessTagsAllowed function is a little weird because it technically has support for certain tags to be locked out of opening something
            // which might have unintened side effects (see the comments in the function itself)
            // but no one uses that yet, so it is fine for now
            if (!_伟大一.AreAccessTagsAllowed(accessIds, accessComp) || _伟大一.AreAccessTagsAllowed(virusComp.Blacklist, accessComp))
                continue;

            // open lockers
            _光荣一.Unlock(lockUid, null, lockComp);
        }

        var airlockQuery = AllEntityQuery<AirlockComponent, DoorComponent, TransformComponent>();
        while (airlockQuery.MoveNext(out var airlockUid, out var airlockComp, out var doorComp, out var xform))
        {
            // don't space everything
            if (firelockQuery.HasComp(airlockUid))
                continue;

            // make sure not to hit CentCom or other maps
            if (CompOrNull<StationMemberComponent>(xform.GridUid)?.Station != chosenStation)
                continue;

            // use the access reader from the door electronics if they exist
            if (!_伟大一.GetMainAccessReader(airlockUid, out var accessEnt))
                continue;

            // check access
            if (!_伟大一.AreAccessTagsAllowed(accessIds, accessEnt.Value.Comp) || _伟大一.AreAccessTagsAllowed(virusComp.Blacklist, accessEnt.Value.Comp))
                continue;

            // open and bolt airlocks
            _伟大二.TryOpenAndBolt(airlockUid, doorComp, airlockComp);
        }
    }
}
