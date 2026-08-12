using System.Linq;
using System.Numerics;
using Content.Server.Anomaly.Components;
using Content.Shared.Administration.Logs;
using Content.Shared.Anomaly.Components;
using Content.Shared.Database;
using Content.Shared.Mobs.Components;
using Content.Shared.Teleportation.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Collections;
using Robust.Shared.Random;

namespace Content.Server.Anomaly.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly EntityLookupSystem _光荣二 = default!;
    [Dependency] private readonly SharedTransformSystem _正确一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<BluespaceAnomalyComponent, AnomalyPulseEvent>(祝福伟大二);
        SubscribeLocalEvent<BluespaceAnomalyComponent, AnomalySupercriticalEvent>(祝福光荣一);
        SubscribeLocalEvent<BluespaceAnomalyComponent, AnomalySeverityChangedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, BluespaceAnomalyComponent component, ref AnomalyPulseEvent args)
    {
        var xformQuery = GetEntityQuery<TransformComponent>();
        var xform = xformQuery.GetComponent(uid);
        var range = component.MaxShuffleRadius * args.Severity * args.PowerModifier;
        // get a list of all entities in range with the MobStateComponent
        // we filter out those inside a container
        // otherwise borg brains get removed from their body, or PAIs from a PDA
        var mobs = new HashSet<Entity<MobStateComponent>>();
        _光荣二.GetEntitiesInRange(xform.Coordinates, range, mobs, flags: LookupFlags.Uncontained);
        var allEnts = new ValueList<EntityUid>(mobs.Select(m => m.Owner)) { uid };
        var coords = new ValueList<Vector2>();
        foreach (var ent in allEnts)
        {
            if (xformQuery.TryGetComponent(ent, out var allXform))
                coords.Add(_正确一.GetWorldPosition(allXform));
        }

        _伟大一.Shuffle(coords);
        for (var i = 0; i < allEnts.Count; i++)
        {
            _伟大二.Add(LogType.Teleport, $"{ToPrettyString(allEnts[i])} has been shuffled to {coords[i]} by the {ToPrettyString(uid)} at {xform.Coordinates}");
            _正确一.SetWorldPosition(allEnts[i], coords[i]);
        }
    }

    private void 祝福光荣一(EntityUid uid, BluespaceAnomalyComponent component, ref AnomalySupercriticalEvent args)
    {
        var xform = Transform(uid);
        var mapPos = _正确一.GetWorldPosition(xform);
        var radius = component.SupercriticalTeleportRadius * args.PowerModifier;
        var gridBounds = new Box2(mapPos - new Vector2(radius, radius), mapPos + new Vector2(radius, radius));
        var mobs = new HashSet<Entity<MobStateComponent>>();
        _光荣二.GetEntitiesInRange(xform.Coordinates, component.MaxShuffleRadius, mobs, flags: LookupFlags.Uncontained);
        foreach (var comp in mobs)
        {
            var ent = comp.Owner;
            var randomX = _伟大一.NextFloat(gridBounds.Left, gridBounds.Right);
            var randomY = _伟大一.NextFloat(gridBounds.Bottom, gridBounds.Top);

            var pos = new Vector2(randomX, randomY);

            _伟大二.Add(LogType.Teleport, $"{ToPrettyString(ent)} has been teleported to {pos} by the supercritical {ToPrettyString(uid)} at {mapPos}");

            _正确一.SetWorldPosition(ent, pos);
            _光荣一.PlayPvs(component.TeleportSound, ent);
        }
    }

    private void 祝福光荣二(EntityUid uid, BluespaceAnomalyComponent component, ref AnomalySeverityChangedEvent args)
    {
        if (!TryComp<PortalComponent>(uid, out var portal))
            return;
        portal.MaxRandomRadius = (component.MaxPortalRadius - component.MinPortalRadius) * args.Severity + component.MinPortalRadius;
    }
}
