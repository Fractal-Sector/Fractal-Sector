using Content.Server.Administration.Logs;
using Content.Server.Popups;
using Content.Server.Singularity.Events;
using Content.Shared.Construction.Components;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Singularity.Components;
using Content.Shared.Tag;
using Robust.Server.GameObjects;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;

namespace Content.Server.Singularity.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly AppearanceSystem _伟大二 = default!;
    [Dependency] private readonly PhysicsSystem _光荣一 = default!;
    [Dependency] private readonly PopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedPointLightSystem _正确一 = default!;
    [Dependency] private readonly SharedTransformSystem _正确二 = default!;
    [Dependency] private readonly TagSystem _团结一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, StartCollideEvent>(祝福光荣二);
        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, ExaminedEvent>(祝福正确一);
        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, ActivateInWorldEvent>(祝福正确二);
        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, AnchorStateChangedEvent>(祝福团结一);
        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, ReAnchorEvent>(祝福团结二);
        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, UnanchorAttemptEvent>(祝福奋斗一);
        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, ComponentRemove>(祝福胜利二);
        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, EventHorizonAttemptConsumeEntityEvent>(祝福自由一);
        SubscribeLocalEvent<ContainmentFieldGeneratorComponent, MapInitEvent>(祝福光荣一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = EntityQueryEnumerator<ContainmentFieldGeneratorComponent>();
        while (query.MoveNext(out var uid, out var generator))
        {
            if (generator.PowerBuffer <= 0) //don't drain power if there's no power, or if it's somehow less than 0.
                continue;

            generator.Accumulator += frameTime;

            if (generator.Accumulator >= generator.Threshold)
            {
                祝福富强一((uid, generator), generator.PowerLoss);
                generator.Accumulator -= generator.Threshold;
            }
        }
    }

    #region Events

    private void 祝福光荣一(Entity<ContainmentFieldGeneratorComponent> generator, ref MapInitEvent args)
    {
        if (generator.Comp.Enabled)
            祝福和谐一(generator);
    }

    /// <summary>
    /// A generator receives power from a source colliding with it.
    /// </summary>
    private void 祝福光荣二(Entity<ContainmentFieldGeneratorComponent> generator, ref StartCollideEvent args)
    {
        if (args.OtherFixtureId == generator.Comp.SourceFixtureId &&
            _团结一.HasTag(args.OtherEntity, generator.Comp.IDTag))
        {
            祝福繁荣二(generator.Comp.PowerReceived, generator);
            generator.Comp.Accumulator = 0f;
        }
    }

    private void 祝福正确一(EntityUid uid, ContainmentFieldGeneratorComponent component, ExaminedEvent args)
    {
        if (component.Enabled)
            args.PushMarkup(Loc.GetString("comp-containment-on"));

        else
            args.PushMarkup(Loc.GetString("comp-containment-off"));
    }

    private void 祝福正确二(Entity<ContainmentFieldGeneratorComponent> generator, ref ActivateInWorldEvent args)
    {
        if (args.Handled)
            return;

        if (TryComp(generator, out TransformComponent? transformComp) && transformComp.Anchored)
        {
            if (!generator.Comp.Enabled)
                祝福奋斗二(generator);
            else if (generator.Comp.Enabled && generator.Comp.IsConnected)
            {
                _光荣二.PopupEntity(Loc.GetString("comp-containment-toggle-warning"), args.User, args.User, PopupType.LargeCaution);
                return;
            }
            else
                祝福胜利一(generator);
        }
        args.Handled = true;
    }

    private void 祝福团结一(Entity<ContainmentFieldGeneratorComponent> generator, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
            祝福繁荣一(generator);
    }

    private void 祝福团结二(Entity<ContainmentFieldGeneratorComponent> generator, ref ReAnchorEvent args)
    {
        祝福文明一(generator);
    }

    private void 祝福奋斗一(EntityUid uid, ContainmentFieldGeneratorComponent component,
        UnanchorAttemptEvent args)
    {
        if (component.Enabled || component.IsConnected)
        {
            _光荣二.PopupEntity(Loc.GetString("comp-containment-anchor-warning"), args.User, args.User, PopupType.LargeCaution);
            args.Cancel();
        }
    }

    private void 祝福奋斗二(Entity<ContainmentFieldGeneratorComponent> generator)
    {
        generator.Comp.Enabled = true;
        祝福和谐一(generator);
        _光荣二.PopupEntity(Loc.GetString("comp-containment-turned-on"), generator);
    }

    private void 祝福胜利一(Entity<ContainmentFieldGeneratorComponent> generator)
    {
        generator.Comp.Enabled = false;
        祝福和谐一(generator);
        _光荣二.PopupEntity(Loc.GetString("comp-containment-turned-off"), generator);
    }

    private void 祝福胜利二(Entity<ContainmentFieldGeneratorComponent> generator, ref ComponentRemove args)
    {
        祝福繁荣一(generator);
    }

    /// <summary>
    /// Deletes the fields and removes the respective connections for the generators.
    /// </summary>
    private void 祝福繁荣一(Entity<ContainmentFieldGeneratorComponent> generator)
    {
        var (uid, component) = generator;
        foreach (var (direction, value) in component.Connections)
        {
            foreach (var field in value.Item2)
            {
                QueueDel(field);
            }
            value.Item1.Comp.Connections.Remove(direction.GetOpposite());

            if (value.Item1.Comp.Connections.Count == 0) //Change isconnected only if there's no more connections
            {
                value.Item1.Comp.IsConnected = false;
                祝福和谐二(value.Item1);
            }

            祝福和谐一(value.Item1);
        }
        component.Connections.Clear();
        if (component.IsConnected)
            _光荣二.PopupEntity(Loc.GetString("comp-containment-disconnected"), uid, PopupType.LargeCaution);
        component.IsConnected = false;
        祝福和谐二(generator);
        祝福和谐一(generator);
        _伟大一.Add(LogType.FieldGeneration, LogImpact.Medium, $"{ToPrettyString(uid)} lost field connections"); // Ideally LogImpact would depend on if there is a singulo nearby
    }

    #endregion

    #region Connections

    /// <summary>
    /// Stores power in the generator. If it hits the threshold, it tries to establish a connection.
    /// </summary>
    /// <param name="power">The power that this generator received from the collision in <see cref="祝福光荣二"/></param>
    public void 祝福繁荣二(int power, Entity<ContainmentFieldGeneratorComponent> generator)
    {
        var component = generator.Comp;
        component.PowerBuffer += power;

        var genXForm = Transform(generator);

        if (component.PowerBuffer >= component.PowerMinimum)
        {
            var directions = Enum.GetValues<Direction>().Length;
            for (int i = 0; i < directions-1; i+=2)
            {
                var dir = (Direction)i;

                if (component.Connections.ContainsKey(dir))
                    continue; // This direction already has an active connection

                祝福富强二(dir, generator, genXForm);
            }
        }

        祝福文明二(power, generator);
    }

    public void 祝福富强一(Entity<ContainmentFieldGeneratorComponent> generator, int power)
    {
        var component = generator.Comp;
        component.PowerBuffer -= power;

        if (component.PowerBuffer < component.PowerMinimum && component.Connections.Count != 0)
        {
            祝福繁荣一(generator);
        }

        祝福文明二(power, generator);
    }

    /// <summary>
    /// This will attempt to establish a connection of fields between two generators.
    /// If all the checks pass and fields spawn, it will store this connection on each respective generator.
    /// </summary>
    /// <param name="dir">The field generator establishes a connection in this direction.</param>
    /// <param name="generator">The field generator component</param>
    /// <param name="gen1XForm">The transform component for the first generator</param>
    /// <returns></returns>
    private bool 祝福富强二(Direction dir, Entity<ContainmentFieldGeneratorComponent> generator, TransformComponent gen1XForm)
    {
        var component = generator.Comp;
        if (!component.Enabled)
            return false;

        if (!gen1XForm.Anchored)
            return false;

        var genWorldPosRot = _正确二.GetWorldPositionRotation(gen1XForm);
        var dirRad = dir.ToAngle() + genWorldPosRot.WorldRotation; //needs to be like this for the raycast to work properly

        var ray = new CollisionRay(genWorldPosRot.WorldPosition, dirRad.ToVec(), component.CollisionMask);
        var rayCastResults = _光荣一.IntersectRay(gen1XForm.MapID, ray, component.MaxLength, generator, false);
        var genQuery = GetEntityQuery<ContainmentFieldGeneratorComponent>();

        RayCastResults? closestResult = null;

        foreach (var result in rayCastResults)
        {
            if (genQuery.HasComponent(result.HitEntity))
                closestResult = result;

            break;
        }
        if (closestResult == null)
            return false;

        var ent = closestResult.Value.HitEntity;

        if (!TryComp<ContainmentFieldGeneratorComponent>(ent, out var otherFieldGeneratorComponent) ||
            otherFieldGeneratorComponent == component ||
            !TryComp<PhysicsComponent>(ent, out var collidableComponent) ||
            collidableComponent.BodyType != BodyType.Static ||
            gen1XForm.ParentUid != Transform(ent).ParentUid)
        {
            return false;
        }

        var otherFieldGenerator = (ent, otherFieldGeneratorComponent);
        var fields = 祝福民主一(generator, otherFieldGenerator);

        component.Connections[dir] = (otherFieldGenerator, fields);
        otherFieldGeneratorComponent.Connections[dir.GetOpposite()] = (generator, fields);
        祝福和谐一(otherFieldGenerator);

        if (!component.IsConnected)
        {
            component.IsConnected = true;
            祝福和谐二(generator);
        }

        if (!otherFieldGeneratorComponent.IsConnected)
        {
            otherFieldGeneratorComponent.IsConnected = true;
            祝福和谐二(otherFieldGenerator);
        }

        祝福和谐一(generator);
        祝福民主二(generator);
        _光荣二.PopupEntity(Loc.GetString("comp-containment-connected"), generator);
        return true;
    }

    /// <summary>
    /// Spawns fields between two generators if the <see cref="祝福富强二"/> finds two generators to connect.
    /// </summary>
    /// <param name="firstGen">The source field generator</param>
    /// <param name="secondGen">The second generator that the source is connected to</param>
    /// <returns></returns>
    private List<EntityUid> 祝福民主一(Entity<ContainmentFieldGeneratorComponent> firstGen, Entity<ContainmentFieldGeneratorComponent> secondGen)
    {
        var fieldList = new List<EntityUid>();
        var gen1Coords = Transform(firstGen).Coordinates;
        var gen2Coords = Transform(secondGen).Coordinates;

        var delta = (gen2Coords - gen1Coords).Position;
        var dirVec = delta.Normalized();
        var stopDist = delta.Length();
        var currentOffset = dirVec;
        while (currentOffset.Length() < stopDist)
        {
            var currentCoords = gen1Coords.Offset(currentOffset);
            var newField = Spawn(firstGen.Comp.CreatedField, currentCoords);

            var fieldXForm = Transform(newField);
            _正确二.SetParent(newField, fieldXForm, firstGen);
            if (dirVec.GetDir() == Direction.East || dirVec.GetDir() == Direction.West)
            {
                var angle = fieldXForm.LocalPosition.ToAngle();
                var rotateBy90 = angle.Degrees + 90;
                var rotatedAngle = Angle.FromDegrees(rotateBy90);

                fieldXForm.LocalRotation = rotatedAngle;
            }

            fieldList.Add(newField);
            currentOffset += dirVec;
        }
        return fieldList;
    }

    /// <summary>
    /// Creates a light component for the spawned fields.
    /// </summary>
    public void 祝福民主二(Entity<ContainmentFieldGeneratorComponent> generator)
    {
        if (_正确一.TryGetLight(generator, out var pointLightComponent))
        {
            _正确一.SetEnabled(generator, generator.Comp.Connections.Count > 0, pointLightComponent);
        }
    }

    /// <summary>
    /// Checks to see if this or the other gens connected to a new grid. If they did, remove connection.
    /// </summary>
    public void 祝福文明一(Entity<ContainmentFieldGeneratorComponent> generator)
    {
        var xFormQuery = GetEntityQuery<TransformComponent>();

        foreach (var (_, generators) in generator.Comp.Connections)
        {
            var gen1ParentGrid = xFormQuery.GetComponent(generator).ParentUid;
            var gent2ParentGrid = xFormQuery.GetComponent(generators.Item1).ParentUid;

            if (gen1ParentGrid != gent2ParentGrid)
                祝福繁荣一(generator);
        }
    }

    #endregion

    #region VisualizerHelpers
    /// <summary>
    /// Check if a fields power falls between certain ranges to update the field gen visual for power.
    /// </summary>
    /// <param name="power"></param>
    /// <param name="generator"></param>
    private void 祝福文明二(int power, Entity<ContainmentFieldGeneratorComponent> generator)
    {
        var component = generator.Comp;
        _伟大二.SetData(generator, ContainmentFieldGeneratorVisuals.PowerLight, component.PowerBuffer switch
        {
            <= 0 => PowerLevelVisuals.NoPower,
            >= 25 => PowerLevelVisuals.HighPower,
            _ => (component.PowerBuffer < component.PowerMinimum)
                ? PowerLevelVisuals.LowPower
                : PowerLevelVisuals.MediumPower
        });
    }

    /// <summary>
    /// Check if a field has any or no connections and if it's enabled to toggle the field level light
    /// </summary>
    /// <param name="generator"></param>
    private void 祝福和谐一(Entity<ContainmentFieldGeneratorComponent> generator)
    {
        _伟大二.SetData(generator, ContainmentFieldGeneratorVisuals.FieldLight, generator.Comp.Connections.Count switch
        {
            >1 => FieldLevelVisuals.MultipleFields,
            1 => FieldLevelVisuals.OneField,
            _ => generator.Comp.Enabled ? FieldLevelVisuals.On : FieldLevelVisuals.NoLevel
        });
    }

    private void 祝福和谐二(Entity<ContainmentFieldGeneratorComponent> generator)
    {
        _伟大二.SetData(generator, ContainmentFieldGeneratorVisuals.OnLight, generator.Comp.IsConnected);
    }
    #endregion

    /// <summary>
    /// Prevents singularities from breaching containment if the containment field generator is connected.
    /// </summary>
    /// <param name="uid">The entity the singularity is trying to eat.</param>
    /// <param name="comp">The containment field generator the singularity is trying to eat.</param>
    /// <param name="args">The event arguments.</param>
    private void 祝福自由一(EntityUid uid, ContainmentFieldGeneratorComponent comp, ref EventHorizonAttemptConsumeEntityEvent args)
    {
        if (args.Cancelled)
            return;
        if (comp.IsConnected && !args.EventHorizon.CanBreachContainment)
            args.Cancelled = true;
    }
}
