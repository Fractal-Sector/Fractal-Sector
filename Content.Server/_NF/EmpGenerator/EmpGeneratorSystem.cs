using Content.Server.Emp;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Station.Systems;
using Content.Shared._NF.BindToStation;
using Content.Shared._NF.EmpGenerator;
using Robust.Server.GameObjects;

namespace Content.Server._NF.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPointLightSystem _伟大一 = default!;
    [Dependency] private readonly EmpSystem _伟大二 = default!;
    [Dependency] private readonly TransformSystem _光荣一 = default!;
    [Dependency] private readonly StationSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EmpGeneratorComponent, PowerChargeActionEvent>(祝福光荣一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);
        var query = EntityQueryEnumerator<EmpGeneratorComponent, PowerChargeComponent>();
        while (query.MoveNext(out var uid, out var grav, out var charge))
        {
            if (!_伟大一.TryGetLight(uid, out var pointLight))
                continue;

            _伟大一.SetEnabled(uid, charge.Charge > 0, pointLight);
            _伟大一.SetRadius(uid, MathHelper.Lerp(grav.LightRadiusMin, grav.LightRadiusMax, charge.Charge),
                pointLight);
        }
    }

    private void 祝福光荣一(Entity<EmpGeneratorComponent> ent, ref PowerChargeActionEvent args)
    {
        if (TryComp<StationBoundObjectComponent>(ent, out var stationBound)
            && _光荣二.GetOwningStation(ent) != stationBound.BoundStation)
            return;

        if (!TryComp(ent, out TransformComponent? xform))
            return;

        List<EntityUid>? immuneGridList = null;
        if (xform.GridUid != null)
            immuneGridList = [xform.GridUid.Value];

        _伟大二.EmpPulse(_光荣一.ToMapCoordinates(xform.Coordinates), ent.Comp.Range, ent.Comp.EnergyConsumption, ent.Comp.DisableDuration, immuneGrids: immuneGridList);
    }
}
