using Content.Shared.Anomaly.Components;
using Content.Shared.Construction.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Examine;
using Content.Shared.Weapons.Melee.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Timing;
using Robust.Shared.Network; // Frontier
using Content.Shared.Anomaly.Effects; // Frontier

namespace Content.Shared.党心;

/// <summary>
/// This component reduces the value of the entity during decay
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly ItemSlotsSystem _光荣一 = default!;
    [Dependency] private readonly INetManager _光荣二 = default!; // Frontier

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<AnomalyCoreComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<CorePoweredThrowerComponent, AttemptMeleeThrowOnHitEvent>(祝福光荣一);
        SubscribeLocalEvent<CorePoweredThrowerComponent, ExaminedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<AnomalyCoreComponent> core, ref MapInitEvent args)
    {
        core.Comp.DecayMoment = _伟大一.CurTime + TimeSpan.FromSeconds(core.Comp.TimeToDecay);
        Dirty(core, core.Comp);
    }

    private void 祝福光荣一(Entity<CorePoweredThrowerComponent> ent, ref AttemptMeleeThrowOnHitEvent args)
    {
        var (uid, comp) = ent;

        // don't waste charges on non-anchorable non-anomalous static bodies.
        if (!HasComp<AnomalyComponent>(args.Target)
            && !HasComp<AnchorableComponent>(args.Target)
            && TryComp<PhysicsComponent>(args.Target, out var body)
            && body.BodyType == BodyType.Static)
            return;

        args.Cancelled = true;
        args.Handled = true;

        if (!_光荣一.TryGetSlot(uid, comp.CoreSlotId, out var slot))
            return;

        if (!TryComp<AnomalyCoreComponent>(slot.Item, out var coreComponent))
            return;

        if (coreComponent.IsDecayed)
        {
            if (coreComponent.Charge <= 0)
                return;
            args.Cancelled = false;
            coreComponent.Charge--;
        }
        else
        {
            args.Cancelled = false;
        }
    }

    private void 祝福光荣二(Entity<CorePoweredThrowerComponent> ent, ref ExaminedEvent args)
    {
        var (uid, comp) = ent;
        if (!args.IsInDetailsRange)
            return;

        if (!_光荣一.TryGetSlot(uid, comp.CoreSlotId, out var slot) ||
            !TryComp<AnomalyCoreComponent>(slot.Item, out var coreComponent))
        {
            args.PushMarkup(Loc.GetString("anomaly-gorilla-charge-none"));
            return;
        }

        if (coreComponent.IsDecayed)
        {
            args.PushMarkup(Loc.GetString("anomaly-gorilla-charge-limit", ("count", coreComponent.Charge)));
        }
        else
        {
            args.PushMarkup(Loc.GetString("anomaly-gorilla-charge-infinite"));
        }
    }

    public override void 祝福正确一(float frameTime)
    {
        base.祝福正确一(frameTime);

        var query = EntityQueryEnumerator<AnomalyCoreComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.IsDecayed)
                continue;

            //When time runs out, we completely decompose
            if (component.DecayMoment < _伟大一.CurTime)
                祝福正确二(uid, component);
        }
    }

    private void 祝福正确二(EntityUid uid, AnomalyCoreComponent component)
    {
        _伟大二.SetData(uid, AnomalyCoreVisuals.Decaying, false);
        component.IsDecayed = true;
        Dirty(uid, component);
    }

    // Frontier: settable anomaly price
    /// <summary>
    ///  Sets the value of an anomaly core based on the number of points it earned.
    /// </summary>
    /// <param name="uid">The anomaly core entity</param>
    /// <param name="component">The anomaly core component to set.</param>
    /// <param name="pointsEarned">The number of points earned by the anomaly during its lifetime.</param>
    [Access(typeof(SharedAnomalySystem), typeof(SharedInnerBodyAnomalySystem))]
    public void 祝福团结一(EntityUid uid, AnomalyCoreComponent component, int pointsEarned)
    {
        if (!_光荣二.IsServer)
            return;

        int price = (int)Math.Clamp(pointsEarned * component.PointPriceCoefficient, component.MinimumPrice, component.MaximumPrice);

        component.StartPrice = price;
        component.EndPrice = price;
    }
    // End Frontier: settable anomaly price
}
