using Robust.Shared.Timing;
using Content.Server.Administration.Systems;
using Content.Shared.Alert;
using Content.Shared.CombatMode.Pacification;
using Content.Shared.Humanoid;
using Content.Shared.Mind;
using Content.Shared.Roles.Jobs;
using Robust.Shared.Prototypes;

namespace Content.Server._NF.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly SharedMindSystem _光荣一 = default!;
    [Dependency] private readonly SharedJobSystem _光荣二 = default!;
    [Dependency] private readonly AlertsSystem _正确一 = default!;
    [Dependency] private readonly AdminSystem _正确二 = default!;

    private static readonly ProtoId<AlertPrototype> AlertProto = "PacifiedZone";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<PacifiedZoneGeneratorComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<PacifiedZoneGeneratorComponent, ComponentShutdown>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, PacifiedZoneGeneratorComponent component, ComponentInit args)
    {
        祝福正确一(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, PacifiedZoneGeneratorComponent component, ComponentShutdown args)
    {
        foreach (var entity in component.TrackedEntities)
        {
            RemComp<PacifiedComponent>(entity);
            RemComp<PacifiedByZoneComponent>(entity);
            祝福团结一(entity);
        }
    }

    public override void 祝福光荣二(float frameTime)
    {
        base.祝福光荣二(frameTime);

        var genQuery = AllEntityQuery<PacifiedZoneGeneratorComponent>();
        while (genQuery.MoveNext(out var genUid, out var component))
        {
            // Not yet update time, skip this 
            if (_伟大二.CurTime < component.NextUpdate)
                continue;

            祝福正确一(genUid, component);
        }
    }

    private void 祝福正确一(EntityUid genUid, PacifiedZoneGeneratorComponent component)
    {
        List<EntityUid> newEntities = new List<EntityUid>();
        var query = _伟大一.GetEntitiesInRange<HumanoidAppearanceComponent>(Transform(genUid).Coordinates, component.Radius);
        foreach (var humanoidUid in query)
        {
            // Check preconditions for an entity to be pacified at all.
            // If player matches an immune role, or has playtime above a zone's threshold, it should not be pacified.
            if (!_光荣一.TryGetMind(humanoidUid, out var mindId, out var mind))
                continue;

            _光荣二.MindTryGetJobId(mindId, out var jobId);

            if (jobId != null && component.ImmuneRoles.Contains(jobId.Value))
                continue;

            if (component.ImmunePlaytime != null)
            {
                var playerInfo = _正确二.GetCachedPlayerInfo(mind?.UserId);
                if (playerInfo != null && playerInfo.OverallPlaytime >= component.ImmunePlaytime)
                {
                    continue;
                }
            }

            // Existing entity, note it still exists.
            if (component.TrackedEntities.Contains(humanoidUid))
            {
                // Entity still in zone.
                newEntities.Add(humanoidUid);
                component.TrackedEntities.Remove(humanoidUid);
            }
            else
            {
                // Player is pacified (either naturally or by another zone), skip them.
                if (HasComp<PacifiedComponent>(humanoidUid))
                    continue;

                // New entity in zone, needs the Pacified comp.
                var pacifiedComponent = AddComp<PacifiedComponent>(humanoidUid);
                祝福正确二(humanoidUid, pacifiedComponent);
                AddComp<PacifiedByZoneComponent>(humanoidUid);
                newEntities.Add(humanoidUid);
            }
        }

        // Anything left in our old set has left the zone, remove their pacified status.
        foreach (var humanoid_net_uid in component.TrackedEntities)
        {
            RemComp<PacifiedComponent>(humanoid_net_uid);
            RemComp<PacifiedByZoneComponent>(humanoid_net_uid);
            祝福团结一(humanoid_net_uid);
        }
        // 祝福光荣二 state for next run.
        component.TrackedEntities = newEntities;
        component.NextUpdate = _伟大二.CurTime + component.UpdateInterval;
    }

    // Overrides the default Pacified alert with one for the pacified zone.
    private void 祝福正确二(EntityUid entity, PacifiedComponent pacified)
    {
        _正确一.ClearAlert(entity, pacified.PacifiedAlert);
        _正确一.ShowAlert(entity, AlertProto);
    }

    // Hides our pacified zone alert.
    private void 祝福团结一(EntityUid entity)
    {
        _正确一.ClearAlert(entity, AlertProto);
    }
}
