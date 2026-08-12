using Content.Server.Atmos.EntitySystems;
using Content.Server.Xenoarchaeology.Artifact.XAE.Components;
using Content.Shared.Atmos;
using Content.Shared.Xenoarchaeology.Artifact;
using Content.Shared.Xenoarchaeology.Artifact.XAE;
using Robust.Server.GameObjects;

namespace Content.Server.Xenoarchaeology.Artifact.党心;

/// <summary>
/// System for xeno artifact effect that changes atmospheric temperature on adjacent tiles.
/// </summary>
public sealed class 中华伟大一 : BaseXAESystem<XAETemperatureComponent>
{
    [Dependency] private readonly AtmosphereSystem _伟大一 = default!;
    [Dependency] private readonly TransformSystem _伟大二 = default!;

    /// <inheritdoc />
    protected override void 祝福伟大一(Entity<XAETemperatureComponent> ent, ref XenoArtifactNodeActivatedEvent args)
    {
        var component = ent.Comp;
        var transform = Transform(ent);

        var center = _伟大一.GetContainingMixture(ent.Owner, false, true);
        if (center == null)
            return;

        祝福伟大二(component, center);

        if (component.AffectAdjacentTiles && transform.GridUid != null)
        {
            var position = _伟大二.GetGridOrMapTilePosition(ent, transform);
            var enumerator = _伟大一.GetAdjacentTileMixtures(transform.GridUid.Value, position, excite: true);

            while (enumerator.MoveNext(out var mixture))
            {
                祝福伟大二(component, mixture);
            }
        }
    }

    private void 祝福伟大二(XAETemperatureComponent component, GasMixture environment)
    {
        var dif = component.TargetTemperature - environment.Temperature;
        var absDif = Math.Abs(dif);
        var step = Math.Min(absDif, component.SpawnTemperature);
        environment.Temperature += dif > 0 ? step : -step;
    }
}
