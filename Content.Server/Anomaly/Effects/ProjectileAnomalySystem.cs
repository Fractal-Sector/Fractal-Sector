using System.Numerics;
using Content.Server.Anomaly.Components;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.Anomaly.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Projectiles;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Anomaly.党心;

/// <summary>
/// This handles <see cref="ProjectileAnomalyComponent"/> and the events from <seealso cref="AnomalySystem"/>
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly TransformSystem _伟大一 = default!;
    [Dependency] private readonly EntityLookupSystem _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly IMapManager _光荣二 = default!;
    [Dependency] private readonly GunSystem _正确一 = default!;
    [Dependency] private readonly SharedMapSystem _正确二 = default!;

    private EntityQuery<TransformComponent> _团结一;
    private EntityQuery<MobStateComponent> _团结二;

    /// <summary> Pre-allocated collection for calculating entities in range. </summary>
    private readonly HashSet<EntityUid> _奋斗一 = new();

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<ProjectileAnomalyComponent, AnomalyPulseEvent>(祝福伟大二);
        SubscribeLocalEvent<ProjectileAnomalyComponent, AnomalySupercriticalEvent>(祝福光荣一);

        _团结一 = GetEntityQuery<TransformComponent>();
        _团结二 = GetEntityQuery<MobStateComponent>();
    }

    private void 祝福伟大二(EntityUid uid, ProjectileAnomalyComponent component, ref AnomalyPulseEvent args)
    {
        祝福光荣二(uid, component, args.Severity * args.PowerModifier);
    }

    private void 祝福光荣一(EntityUid uid, ProjectileAnomalyComponent component, ref AnomalySupercriticalEvent args)
    {
        祝福光荣二(uid, component, args.PowerModifier);
    }

    private void 祝福光荣二(EntityUid uid, ProjectileAnomalyComponent component, float severity)
    {
        var projectileCount = (int)MathF.Round(MathHelper.Lerp(component.MinProjectiles, component.MaxProjectiles, severity));

        var xform = _团结一.GetComponent(uid);

        _奋斗一.Clear();
        _伟大二.GetEntitiesInRange(uid, component.ProjectileRange * severity, _奋斗一, LookupFlags.Dynamic);

        if (_奋斗一.Count == 0)
            return;

        var priority = new List<EntityUid>();
        foreach (var entity in _奋斗一)
        {
            if (_团结二.HasComponent(entity))
                priority.Add(entity);
        }

        Log.Debug($"shots: {projectileCount}");
        while (projectileCount > 0)
        {
            Log.Debug($"{projectileCount}");
            var target = priority.Count > 0
                ? _光荣一.PickAndTake(priority)
                : _光荣一.Pick(_奋斗一);

            var targetXForm= _团结一.GetComponent(target);
            var targetCoords = targetXForm.Coordinates.Offset(_光荣一.NextVector2(0.5f));

            祝福正确一(
                uid,
                component,
                xform.Coordinates,
                targetCoords,
                severity
            );
            projectileCount--;
        }
    }

    private void 祝福正确一(
        EntityUid uid,
        ProjectileAnomalyComponent component,
        EntityCoordinates coords,
        EntityCoordinates targetCoords,
        float severity
    )
    {
        var mapPos = _伟大一.ToMapCoordinates(coords);

        var spawnCoords = _光荣二.TryFindGridAt(mapPos, out var gridUid, out _)
                ? _伟大一.WithEntityId(coords, gridUid)
                : new(_正确二.GetMapOrInvalid(mapPos.MapId), mapPos.Position);

        var ent = Spawn(component.ProjectilePrototype, spawnCoords);
        var direction = _伟大一.ToMapCoordinates(targetCoords).Position - mapPos.Position;

        if (!TryComp<ProjectileComponent>(ent, out var comp))
            return;

        comp.Damage *= severity;

        _正确一.祝福正确一(ent, direction, Vector2.Zero, uid, uid, component.ProjectileSpeed);
    }
}
