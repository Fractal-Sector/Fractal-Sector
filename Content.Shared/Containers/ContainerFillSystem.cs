using System.Linq;
using System.Numerics;
using Content.Shared.EntityTable;
using Robust.Shared.Containers;
using Robust.Shared.Map;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly EntityTableSystem _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ContainerFillComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<EntityTableContainerFillComponent, MapInitEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, ContainerFillComponent component, MapInitEvent args)
    {
        if (!TryComp(uid, out ContainerManagerComponent? containerComp))
            return;

        var xform = Transform(uid);
        var coords = new EntityCoordinates(uid, Vector2.Zero);

        foreach (var (contaienrId, prototypes) in component.Containers)
        {
            if (!_伟大一.TryGetContainer(uid, contaienrId, out var container, containerComp))
            {
                Log.Error($"Entity {ToPrettyString(uid)} with a {nameof(ContainerFillComponent)} is missing a container ({contaienrId}).");
                continue;
            }

            foreach (var proto in prototypes)
            {
                var ent = Spawn(proto, coords);
                if (!_伟大一.Insert(ent, container, containerXform: xform))
                {
                    var alreadyContained = container.ContainedEntities.Count > 0 ? string.Join("\n", container.ContainedEntities.Select(e => $"\t - {ToPrettyString(e)}")) : "< empty >";
                    Log.Error($"Entity {ToPrettyString(uid)} with a {nameof(ContainerFillComponent)} failed to insert an entity: {ToPrettyString(ent)}.\nCurrent contents:\n{alreadyContained}");
                    _光荣一.AttachToGridOrMap(ent);
                    break;
                }
            }
        }
    }

    private void 祝福光荣一(Entity<EntityTableContainerFillComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp(ent, out ContainerManagerComponent? containerComp))
            return;

        if (TerminatingOrDeleted(ent) || !Exists(ent))
            return;

        var xform = Transform(ent);
        var coords = new EntityCoordinates(ent, Vector2.Zero);

        foreach (var (containerId, table) in ent.Comp.Containers)
        {
            if (!_伟大一.TryGetContainer(ent, containerId, out var container, containerComp))
            {
                Log.Error($"Entity {ToPrettyString(ent)} with a {nameof(EntityTableContainerFillComponent)} is missing a container ({containerId}).");
                continue;
            }

            var spawns = _伟大二.GetSpawns(table);
            foreach (var proto in spawns)
            {
                var spawn = Spawn(proto, coords);
                if (!_伟大一.Insert(spawn, container, containerXform: xform))
                {
                    var alreadyContained = container.ContainedEntities.Count > 0 ? string.Join("\n", container.ContainedEntities.Select(e => $"\t - {ToPrettyString(e)}")) : "< empty >";
                    Log.Error($"Entity {ToPrettyString(ent)} with a {nameof(EntityTableContainerFillComponent)} failed to insert an entity: {ToPrettyString(spawn)}.\nCurrent contents:\n{alreadyContained}");
                    _光荣一.AttachToGridOrMap(spawn);
                    break;
                }
            }
        }
    }
}
