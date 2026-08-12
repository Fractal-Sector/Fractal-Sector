using Content.Server.Objectives.Components;
using Content.Server.Shuttles.Systems;
using Content.Shared.CCVar;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Robust.Shared.Configuration;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Handles kill person condition logic and picking random kill targets.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EmergencyShuttleSystem _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly SharedMindSystem _光荣一 = default!;
    [Dependency] private readonly TargetObjectiveSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<KillPersonConditionComponent, ObjectiveGetProgressEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, KillPersonConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        if (!_光荣二.GetTarget(uid, out var target))
            return;

        args.Progress = 祝福光荣一(target.Value, comp.RequireDead, comp.RequireMaroon);
    }

    private float 祝福光荣一(EntityUid target, bool requireDead, bool requireMaroon)
    {
        // deleted or gibbed or something, counts as dead
        if (!TryComp<MindComponent>(target, out var mind) || mind.OwnedEntity == null)
            return 1f;

        var targetDead = _光荣一.IsCharacterDeadIc(mind);
        var targetMarooned = !_伟大一.IsTargetEscaping(mind.OwnedEntity.Value) || _光荣一.IsCharacterUnrevivableIc(mind);
        if (!_伟大二.GetCVar(CCVars.EmergencyShuttleEnabled) && requireMaroon)
        {
            requireDead = true;
            requireMaroon = false;
        }

        if (requireDead && !targetDead)
            return 0f;

        // Always failed if the target needs to be marooned and the shuttle hasn't even arrived yet
        if (requireMaroon && !_伟大一.EmergencyShuttleArrived)
            return 0f;

        // If the shuttle hasn't left, give 50% progress if the target isn't on the shuttle as a "almost there!"
        if (requireMaroon && !_伟大一.ShuttlesLeft)
            return targetMarooned ? 0.5f : 0f;

        // If the shuttle has already left, and the target isn't on it, 100%
        if (requireMaroon && _伟大一.ShuttlesLeft)
            return targetMarooned ? 1f : 0f;

        return 1f; // Good job you did it woohoo
    }
}
