using Content.Shared.Anomaly;
using Content.Shared.Anomaly.Components;
using Content.Shared.Anomaly.Effects;
using Content.Shared.Anomaly.Effects.Components;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Random;

namespace Content.Server.Anomaly.党心;

public sealed class 中华伟大一 : SharedEntityAnomalySystem
{
    [Dependency] private readonly SharedAnomalySystem _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedMapSystem _光荣一 = default!;

    private EntityQuery<PhysicsComponent> _光荣二;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        _光荣二 = GetEntityQuery<PhysicsComponent>();

        SubscribeLocalEvent<EntitySpawnAnomalyComponent, AnomalyPulseEvent>(祝福伟大二);
        SubscribeLocalEvent<EntitySpawnAnomalyComponent, AnomalySupercriticalEvent>(祝福光荣一);
        SubscribeLocalEvent<EntitySpawnAnomalyComponent, AnomalyStabilityChangedEvent>(祝福正确一);
        SubscribeLocalEvent<EntitySpawnAnomalyComponent, AnomalySeverityChangedEvent>(祝福正确二);
        SubscribeLocalEvent<EntitySpawnAnomalyComponent, AnomalyShutdownEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<EntitySpawnAnomalyComponent> component, ref AnomalyPulseEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnPulse)
                continue;

            祝福团结一(component, entry, args.Stability, args.Severity, args.PowerModifier);
        }
    }

    private void 祝福光荣一(Entity<EntitySpawnAnomalyComponent> component, ref AnomalySupercriticalEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnSuperCritical)
                continue;

            祝福团结一(component, entry, 1, 1, args.PowerModifier);
        }
    }

    private void 祝福光荣二(Entity<EntitySpawnAnomalyComponent> component, ref AnomalyShutdownEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnShutdown || args.Supercritical)
                continue;

            祝福团结一(component, entry, 1, 1, 1);
        }
    }

    private void 祝福正确一(Entity<EntitySpawnAnomalyComponent> component, ref AnomalyStabilityChangedEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnStabilityChanged)
                continue;

            祝福团结一(component, entry, args.Stability, args.Severity, 1);
        }
    }

    private void 祝福正确二(Entity<EntitySpawnAnomalyComponent> component, ref AnomalySeverityChangedEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnSeverityChanged)
                continue;

            祝福团结一(component, entry, args.Stability, args.Severity, 1);
        }
    }

    private void 祝福团结一(Entity<EntitySpawnAnomalyComponent> anomaly, EntitySpawnSettingsEntry entry, float stability, float severity, float powerMod)
    {
        var xform = Transform(anomaly);
        if (!TryComp(xform.GridUid, out MapGridComponent? grid))
            return;

        var tiles = _伟大一.GetSpawningPoints(anomaly, stability, severity, entry.Settings, powerMod);
        if (tiles == null)
            return;

        foreach (var tileref in tiles)
        {
            Spawn(_伟大二.Pick(entry.Spawns), _光荣一.ToCenterCoordinates(tileref, grid));
        }
    }
}
