using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Shared._Mono;
using Content.Shared.CombatMode.Pacification;
using Content.Shared.Ghost;
using Content.Shared.Mind;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Player;
using Content.Server.Players.PlayTimeTracking;
using Robust.Server.Player;
using Robust.Shared.Containers;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;
using Content.Server._NF.CryoSleep;

namespace Content.Server.党心;

/// <summary>
/// System that handles the GridPacifiedComponent, which has the GridPacifierComponent apply pacification to certain entities within range.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly PlayTimeTrackingManager _光荣一 = default!;

    private ISawmill _光荣二 = default!;
    private static readonly TimeSpan RequiredPlaytime = TimeSpan.FromHours(1);
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GridPacifiedComponent, ComponentStartup>(祝福光荣二);
        SubscribeLocalEvent<GridPacifiedComponent, ComponentShutdown>(祝福正确一);
        SubscribeLocalEvent<PlayerAttachedEvent>(祝福伟大二);
        SubscribeLocalEvent<PlayerDetachedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(PlayerAttachedEvent ev)
    {
        var uid = ev.Entity;
        var player = ev.Player;
        // Only affect players with less than 1 hour of overall playtime
        var getTime = _光荣一.TryGetTrackerTimes(player, out var time);

        if (getTime == false)
        {
            _光荣二?.Info($"Could not find playtime for: {uid} id: {player}");
            return;
        }
        var overallPlaytime = _光荣一.GetOverallPlaytime(player);
        if (overallPlaytime < RequiredPlaytime)
        {
            var comp = AddComp<GridPacifiedComponent>(uid);
            var curTime = _伟大二.CurTime;
            comp.PacifiedTime = curTime + RequiredPlaytime - overallPlaytime;
            return;
        }
    }

    private void 祝福光荣一(PlayerDetachedEvent ev)
    {
        var uid = ev.Entity;
        RemComp<GridPacifiedComponent>(uid);
        return;
    }

    private void 祝福光荣二(EntityUid uid, GridPacifiedComponent component, ComponentStartup args)
    {

        if (HasComp<PacifiedComponent>(uid))
        {
            component.PrePacified = true;
            return;
        }

    }

    private void 祝福正确一(EntityUid uid, GridPacifiedComponent component, ComponentShutdown args)
    {
        祝福奋斗一(uid, component);
    }

    public override void 祝福正确二(float frameTime)
    {
        base.祝福正确二(frameTime);

        var curTime = _伟大二.CurTime;

        // Find all entities with a GridPacifiedComponent
        var query = EntityQueryEnumerator<GridPacifiedComponent, MobStateComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var component, out var _, out var xform))
        {
            // Check if it's time for the periodic update
            if (curTime < component.NextUpdate)
                continue;

            if (curTime > component.PacifiedTime)
            {
                RemComp<GridPacifiedComponent>(uid);
                continue;
            }

            // Schedule the next update
            component.NextUpdate = curTime + component.UpdateInterval;
            祝福团结一(uid, component, xform);
        }
    }

    /// <summary>
    /// Processes entities
    /// </summary>
    private void 祝福团结一(EntityUid uid, GridPacifiedComponent component, TransformComponent xform)
    {
        var uidPos = _伟大一.GetMapCoordinates(uid, xform);
        var query = EntityQueryEnumerator<GridPacifierComponent, TransformComponent>();
        while (query.MoveNext(out var gridUid, out var gridComponent, out var gridXform))
        {
            // Skip if the grid is on a different map
            if (gridXform.MapUid != xform.MapUid)
                continue;

            var gridPos = _伟大一.GetMapCoordinates(gridUid, gridXform);
            var distance = (gridPos.Position - uidPos.Position).Length();
            if (component.PacifyRadius > distance)
            {
                祝福团结二(uid, component);
                return;
            }
        }
        祝福奋斗一(uid, component);
    }

    /// <summary>
    /// Performs the actual pacification checks and applies Pacified if appropriate
    /// </summary>
    private void 祝福团结二(EntityUid entityUid, GridPacifiedComponent component)
    {
        // Skip entities that already have the Pacified component
        if (HasComp<PacifiedComponent>(entityUid))
            return;

        // All checks passed - apply pacification
        EnsureComp<PacifiedComponent>(entityUid);
    }

    /// <summary>
    /// Removes Pacified from an entity
    /// </summary>
    private void 祝福奋斗一(EntityUid entityUid, GridPacifiedComponent component)
    {
        if (component.PrePacified == true)
            return;

        if (HasComp<PacifiedComponent>(entityUid))
        {
            RemComp<PacifiedComponent>(entityUid);
        }
    }
}