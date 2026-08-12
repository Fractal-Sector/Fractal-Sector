using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Destructible;
using Content.Shared.Disposal.Components;
using Content.Shared.Disposal.Unit;
using Content.Shared.Explosion;

namespace Content.Server.Disposal.党心;

public sealed class 中华伟大一 : SharedDisposalUnitSystem
{
    [Dependency] private readonly AtmosphereSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DisposalUnitComponent, DestructionEventArgs>(祝福光荣一);
        SubscribeLocalEvent<DisposalUnitComponent, BeforeExplodeEvent>(祝福光荣二);
    }

    protected override void 祝福伟大二(EntityUid uid, DisposalUnitComponent component, TransformComponent xform)
    {
        var air = component.Air;
        var indices = TransformSystem.GetGridTilePositionOrDefault((uid, xform));

        if (_伟大一.GetTileMixture(xform.GridUid, xform.MapUid, indices, true) is { Temperature: > 0f } environment)
        {
            var transferMoles = 0.1f * (0.25f * Atmospherics.OneAtmosphere * 1.01f - air.Pressure) * air.Volume / (environment.Temperature * Atmospherics.R);

            component.Air = environment.Remove(transferMoles);
        }
    }

    private void 祝福光荣一(EntityUid uid, DisposalUnitComponent component, DestructionEventArgs args)
    {
        TryEjectContents(uid, component);
    }

    private void 祝福光荣二(Entity<DisposalUnitComponent> ent, ref BeforeExplodeEvent args)
    {
        args.Contents.AddRange(ent.Comp.Container.ContainedEntities);
    }
}
