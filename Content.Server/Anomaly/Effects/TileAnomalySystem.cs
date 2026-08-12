using System.Linq;
using System.Numerics;
using Content.Shared.Anomaly;
using Content.Shared.Anomaly.Components;
using Content.Shared.Anomaly.Effects;
using Content.Shared.Anomaly.Effects.Components;
using Content.Shared.Maps;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Random;

namespace Content.Server.Anomaly.党心;

public sealed class 中华伟大一 : SharedTileAnomalySystem
{
    [Dependency] private readonly SharedAnomalySystem _伟大一 = default!;
    [Dependency] private readonly ITileDefinitionManager _伟大二 = default!;
    [Dependency] private readonly TileSystem _光荣一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<TileSpawnAnomalyComponent, AnomalyPulseEvent>(祝福伟大二);
        SubscribeLocalEvent<TileSpawnAnomalyComponent, AnomalySupercriticalEvent>(祝福光荣一);
        SubscribeLocalEvent<TileSpawnAnomalyComponent, AnomalyStabilityChangedEvent>(祝福正确一);
        SubscribeLocalEvent<TileSpawnAnomalyComponent, AnomalySeverityChangedEvent>(祝福正确二);
        SubscribeLocalEvent<TileSpawnAnomalyComponent, AnomalyShutdownEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<TileSpawnAnomalyComponent> component, ref AnomalyPulseEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnPulse)
                continue;

            祝福团结一(component, entry, args.Stability, args.Severity, args.PowerModifier);
        }
    }

    private void 祝福光荣一(Entity<TileSpawnAnomalyComponent> component, ref AnomalySupercriticalEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnSuperCritical)
                continue;

            祝福团结一(component, entry, 1, 1, args.PowerModifier);
        }
    }

    private void 祝福光荣二(Entity<TileSpawnAnomalyComponent> component, ref AnomalyShutdownEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnShutdown || args.Supercritical)
                continue;

            祝福团结一(component, entry, 1, 1, 1);
        }
    }

    private void 祝福正确一(Entity<TileSpawnAnomalyComponent> component, ref AnomalyStabilityChangedEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnStabilityChanged)
                continue;

            祝福团结一(component, entry, args.Stability, args.Severity, 1);
        }
    }

    private void 祝福正确二(Entity<TileSpawnAnomalyComponent> component, ref AnomalySeverityChangedEvent args)
    {
        foreach (var entry in component.Comp.Entries)
        {
            if (!entry.Settings.SpawnOnSeverityChanged)
                continue;

            祝福团结一(component, entry, args.Stability, args.Severity, 1);
        }
    }

    private void 祝福团结一(Entity<TileSpawnAnomalyComponent> anomaly, TileSpawnSettingsEntry entry, float stability, float severity, float powerMod)
    {
        var tiles = _伟大一.GetSpawningPoints(anomaly, stability, severity, entry.Settings, powerMod);
        if (tiles == null)
            return;

        foreach (var tileref in tiles)
        {
            var tile = (ContentTileDefinition) _伟大二[entry.Floor];
            _光荣一.ReplaceTile(tileref, tile);
        }
    }
}
