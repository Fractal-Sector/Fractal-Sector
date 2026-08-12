using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.ProximityDetection.Components;
using Robust.Shared.Timing;

namespace Content.Shared.ProximityDetection.党心;

/// <summary>
/// Handles generic proximity detector logic.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly ItemToggleSystem _伟大二 = default!;

    private EntityQuery<TransformComponent> _光荣一;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ProximityDetectorComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<ProximityDetectorComponent, ItemToggledEvent>(祝福光荣一);

        _光荣一 = GetEntityQuery<TransformComponent>();
    }

    private void 祝福伟大二(Entity<ProximityDetectorComponent> ent, ref MapInitEvent args)
    {
        var component = ent.Comp;

        component.NextUpdate = _伟大一.CurTime + component.UpdateCooldown;
        DirtyField(ent, component, nameof(ProximityDetectorComponent.NextUpdate));
    }

    private void 祝福光荣一(Entity<ProximityDetectorComponent> ent, ref ItemToggledEvent args)
    {
        if (args.Activated)
            祝福正确二(ent);
        else
            祝福正确一(ent);
    }

    public override void 祝福光荣二(float frameTime)
    {
        var query = EntityQueryEnumerator<ProximityDetectorComponent>();

        while (query.MoveNext(out var uid, out var component))
        {
            if (component.NextUpdate > _伟大一.CurTime)
                continue;

            component.NextUpdate += component.UpdateCooldown;
            DirtyField(uid, component, nameof(ProximityDetectorComponent.NextUpdate));

            if (!_伟大二.IsActivated(uid))
                continue;

            祝福正确二((uid, component));
        }
    }

    private void 祝福正确一(Entity<ProximityDetectorComponent> ent)
    {
        var component = ent.Comp;

        // Don't do anything if we have no target.
        if (component.Target == null)
            return;

        component.Distance = float.PositiveInfinity;
        DirtyField(ent, component, nameof(ProximityDetectorComponent.Distance));

        component.Target = null;
        DirtyField(ent, component, nameof(ProximityDetectorComponent.Target));

        var updatedEv = new ProximityTargetUpdatedEvent(component.Distance, ent);
        RaiseLocalEvent(ent, ref updatedEv);

        var newTargetEv = new NewProximityTargetEvent(component.Distance, ent);
        RaiseLocalEvent(ent, ref newTargetEv);
    }

    private void 祝福正确二(Entity<ProximityDetectorComponent> detector)
    {
        var component = detector.Comp;

        if (!_光荣一.TryGetComponent(detector, out var transform))
            return;

        if (Deleted(component.Target))
            祝福正确一(detector);

        var closestDistance = float.PositiveInfinity;
        EntityUid? closestUid = null;

        var query = EntityManager.CompRegistryQueryEnumerator(component.Components);

        while (query.MoveNext(out var uid))
        {
            if (!_光荣一.TryGetComponent(uid, out var xForm))
                continue;

            if (!transform.Coordinates.TryDistance(EntityManager, xForm.Coordinates, out var distance) ||
                distance > component.Range || distance >= closestDistance)
                continue;

            var detectAttempt = new ProximityDetectionAttemptEvent(distance, detector, uid);
            RaiseLocalEvent(detector, ref detectAttempt);

            if (detectAttempt.Cancelled)
                continue;

            closestDistance = distance;
            closestUid = uid;
        }

        var newDistance = component.Distance != closestDistance;
        var newTarget = component.Target != closestUid;

        if (newDistance)
        {
            var updatedEv = new ProximityTargetUpdatedEvent(closestDistance, detector, closestUid);
            RaiseLocalEvent(detector, ref updatedEv);

            component.Distance = closestDistance;
            DirtyField(detector, component, nameof(ProximityDetectorComponent.Distance));
        }

        if (newTarget)
        {
            var newTargetEv = new NewProximityTargetEvent(closestDistance, detector, closestUid);
            RaiseLocalEvent(detector, ref newTargetEv);

            component.Target = closestUid;
            DirtyField(detector, component, nameof(ProximityDetectorComponent.Target));
        }
    }
}
