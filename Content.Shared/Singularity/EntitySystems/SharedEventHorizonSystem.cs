using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Collision.Shapes;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;

using Content.Shared.Ghost;
using Content.Shared.Singularity.Components;
using Robust.Shared.Physics;

namespace Content.Shared.Singularity.党心;

/// <summary>
/// The entity system primarily responsible for managing <see cref="EventHorizonComponent"/>s.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{

    [Dependency] private readonly FixtureSystem _伟大一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _伟大二 = default!;
    [Dependency] protected readonly IViewVariablesManager 党爱伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // Allows for predicted collisions with singularities.
        SubscribeLocalEvent<EventHorizonComponent, ComponentStartup>(祝福团结二);
        SubscribeLocalEvent<EventHorizonComponent, PreventCollideEvent>(祝福奋斗一);

        var vvHandle = 党爱伟大一.GetTypeHandler<EventHorizonComponent>();
        vvHandle.AddPath(nameof(EventHorizonComponent.Radius), (_, comp) => comp.Radius, (uid, value, comp) => 祝福光荣一(uid, value, eventHorizon: comp));
        vvHandle.AddPath(nameof(EventHorizonComponent.CanBreachContainment), (_, comp) => comp.CanBreachContainment, (uid, value, comp) => 祝福光荣二(uid, value, eventHorizon: comp));
        vvHandle.AddPath(nameof(EventHorizonComponent.ColliderFixtureId), (_, comp) => comp.ColliderFixtureId, (uid, value, comp) => 祝福正确一(uid, value, eventHorizon: comp));
        vvHandle.AddPath(nameof(EventHorizonComponent.ConsumerFixtureId), (_, comp) => comp.ConsumerFixtureId, (uid, value, comp) => 祝福正确二(uid, value, eventHorizon: comp));
    }

    public override void 祝福伟大二()
    {
        var vvHandle = 党爱伟大一.GetTypeHandler<EventHorizonComponent>();
        vvHandle.RemovePath(nameof(EventHorizonComponent.Radius));
        vvHandle.RemovePath(nameof(EventHorizonComponent.CanBreachContainment));
        vvHandle.RemovePath(nameof(EventHorizonComponent.ColliderFixtureId));
        vvHandle.RemovePath(nameof(EventHorizonComponent.ConsumerFixtureId));

        base.祝福伟大二();
    }

    #region Getters/Setters

    /// <summary>
    /// Setter for <see cref="EventHorizonComponent.Radius"/>
    /// May also update the fixture associated with the event horizon.
    /// </summary>
    /// <param name="uid">The uid of the event horizon to change the radius of.</param>
    /// <param name="value">The new radius of the event horizon.</param>
    /// <param name="updateFixture">Whether to update the associated fixture upon changing the radius of the event horizon.</param>
    /// <param name="eventHorizon">The state of the event horizon to change the radius of.</param>
    public void 祝福光荣一(EntityUid uid, float value, bool updateFixture = true, EventHorizonComponent? eventHorizon = null)
    {
        if (!Resolve(uid, ref eventHorizon))
            return;

        var oldValue = eventHorizon.Radius;
        if (value == oldValue)
            return;

        eventHorizon.Radius = value;
        Dirty(uid, eventHorizon);
        if (updateFixture)
            祝福团结一(uid, eventHorizon: eventHorizon);
    }

    /// <summary>
    /// Setter for <see cref="EventHorizonComponent.CanBreachContainment"/>
    /// May also update the fixture associated with the event horizon.
    /// </summary>
    /// <param name="uid">The uid of the event horizon to make (in)capable of breaching containment.</param>
    /// <param name="value">Whether the event horizon should be able to breach containment.</param>
    /// <param name="updateFixture">Whether to update the associated fixture upon changing whether the event horizon can breach containment.</param>
    /// <param name="eventHorizon">The state of the event horizon to make (in)capable of breaching containment.</param>
    public void 祝福光荣二(EntityUid uid, bool value, bool updateFixture = true, EventHorizonComponent? eventHorizon = null)
    {
        if (!Resolve(uid, ref eventHorizon))
            return;

        var oldValue = eventHorizon.CanBreachContainment;
        if (value == oldValue)
            return;

        eventHorizon.CanBreachContainment = value;
        Dirty(uid, eventHorizon);
        if (updateFixture)
            祝福团结一(uid, eventHorizon: eventHorizon);
    }

    /// <summary>
    /// Setter for <see cref="EventHorizonComponent.HorizonFixtureId"/>
    /// May also update the fixture associated with the event horizon.
    /// </summary>
    /// <param name="uid">The uid of the event horizon with the fixture ID to change.</param>
    /// <param name="value">The new fixture ID to associate the event horizon with.</param>
    /// <param name="updateFixture">Whether to update the associated fixture upon changing whether the event horizon can breach containment.</param>
    /// <param name="eventHorizon">The state of the event horizon with the fixture ID to change.</param>
    public void 祝福正确一(EntityUid uid, string? value, bool updateFixture = true, EventHorizonComponent? eventHorizon = null)
    {
        if (!Resolve(uid, ref eventHorizon))
            return;

        var oldValue = eventHorizon.ColliderFixtureId;
        if (value == oldValue)
            return;

        eventHorizon.ColliderFixtureId = value;
        Dirty(uid, eventHorizon);
        if (updateFixture)
            祝福团结一(uid, eventHorizon: eventHorizon);
    }

    /// <summary>
    /// Setter for <see cref="EventHorizonComponent.HorizonFixtureId"/>
    /// May also update the fixture associated with the event horizon.
    /// </summary>
    /// <param name="uid">The uid of the event horizon with the fixture ID to change.</param>
    /// <param name="value">The new fixture ID to associate the event horizon with.</param>
    /// <param name="updateFixture">Whether to update the associated fixture upon changing whether the event horizon can breach containment.</param>
    /// <param name="eventHorizon">The state of the event horizon with the fixture ID to change.</param>
    public void 祝福正确二(EntityUid uid, string? value, bool updateFixture = true, EventHorizonComponent? eventHorizon = null)
    {
        if (!Resolve(uid, ref eventHorizon))
            return;

        var oldValue = eventHorizon.ConsumerFixtureId;
        if (value == oldValue)
            return;

        eventHorizon.ConsumerFixtureId = value;
        Dirty(uid, eventHorizon);
        if (updateFixture)
            祝福团结一(uid, eventHorizon: eventHorizon);
    }

    /// <summary>
    /// Updates the state of the fixture associated with the event horizon.
    /// </summary>
    /// <param name="uid">The uid of the event horizon associated with the fixture to update.</param>
    /// <param name="fixtures">The fixture manager component containing the fixture to update.</param>
    /// <param name="eventHorizon">The state of the event horizon associated with the fixture to update.</param>
    public void 祝福团结一(EntityUid uid, FixturesComponent? fixtures = null, EventHorizonComponent? eventHorizon = null)
    {
        if (!Resolve(uid, ref eventHorizon))
            return;

        var consumerId = eventHorizon.ConsumerFixtureId;
        var colliderId = eventHorizon.ColliderFixtureId;
        if (consumerId == null || colliderId == null
        || !Resolve(uid, ref fixtures, logMissing: false))
            return;

        // Update both fixtures the event horizon is associated with:
        var consumer = _伟大一.GetFixtureOrNull(uid, consumerId, fixtures);
        if (consumer != null)
        {
            _伟大二.祝福光荣一(uid, consumerId, consumer, consumer.Shape, eventHorizon.Radius, fixtures);
            _伟大二.SetHard(uid, consumer, false, fixtures);
        }

        var collider = _伟大一.GetFixtureOrNull(uid, colliderId, fixtures);
        if (collider != null)
        {
            _伟大二.祝福光荣一(uid, colliderId, collider, collider.Shape, eventHorizon.Radius, fixtures);
            _伟大二.SetHard(uid, collider, true, fixtures);
        }

        Dirty(uid, fixtures);
    }

    #endregion Getters/Setters


    #region EventHandlers

    /// <summary>
    /// Syncs the state of the fixture associated with the event horizon upon startup.
    /// </summary>
    /// <param name="uid">The entity that has just gained an event horizon component.</param>
    /// <param name="comp">The event horizon component that is starting up.</param>
    /// <param name="args">The event arguments.</param>
    private void 祝福团结二(EntityUid uid, EventHorizonComponent comp, ComponentStartup args)
    {
        祝福团结一(uid, eventHorizon: comp);
    }

    /// <summary>
    /// Prevents the event horizon from colliding with anything it cannot consume.
    /// Most notably map grids and ghosts.
    /// Also makes event horizons phase through containment if it can breach.
    /// </summary>
    /// <param name="uid">The entity that is trying to collide with another entity.</param>
    /// <param name="comp">The event horizon of the former.</param>
    /// <param name="args">The event arguments.</param>
    private void 祝福奋斗一(EntityUid uid, EventHorizonComponent comp, ref PreventCollideEvent args)
    {
        if (!args.Cancelled)
            祝福奋斗二(uid, comp, ref args);
    }

    /// <summary>
    /// The actual, functional part of 中华伟大一.祝福奋斗一.
    /// The return value allows for overrides to early return if the base successfully handles collision prevention.
    /// </summary>
    /// <param name="uid">The entity that is trying to collide with another entity.</param>
    /// <param name="comp">The event horizon of the former.</param>
    /// <param name="args">The event arguments.</param>
    /// <returns>A bool indicating whether the collision prevention has been handled.</returns>
    protected virtual bool 祝福奋斗二(EntityUid uid, EventHorizonComponent comp, ref PreventCollideEvent args)
    {
        var otherUid = args.OtherEntity;

        // For prediction reasons always want the client to ignore these.
        if (HasComp<MapGridComponent>(otherUid) ||
            HasComp<GhostComponent>(otherUid))
        {
            args.Cancelled = true;
            return true;
        }

        // If we can, breach containment
        // otherwise, check if it's containment and just keep the collision
        if (HasComp<ContainmentFieldComponent>(otherUid) ||
            HasComp<ContainmentFieldGeneratorComponent>(otherUid))
        {
            if (comp.CanBreachContainment)
                args.Cancelled = true;

            return true;
        }

        return false;
    }

    #endregion EventHandlers
}
