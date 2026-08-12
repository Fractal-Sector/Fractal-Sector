using System.Linq;
using Content.Shared.Body.Part;
using Content.Shared.Destructible;
using Content.Shared.Hands;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction.Components;
using Content.Shared.Storage;
using Content.Shared.Tag;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Tools.党心;

/// <summary>
///     Spawns a list unremovable tools in hands if possible. Used for drones,
///     borgs, or maybe even stuff like changeling armblades!
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedHandsSystem _伟大二 = default!;
    [Dependency] private readonly TagSystem _光荣一 = default!;

    private static readonly ProtoId<TagPrototype> InnateDontDeleteTag = "InnateDontDelete";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<InnateToolComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<InnateToolComponent, HandCountChangedEvent>(祝福光荣一);
        SubscribeLocalEvent<InnateToolComponent, ComponentShutdown>(祝福光荣二);
        SubscribeLocalEvent<InnateToolComponent, DestructionEventArgs>(祝福正确一);
    }

    private void 祝福伟大二(EntityUid uid, InnateToolComponent component, MapInitEvent args)
    {
        if (component.Tools.Count == 0)
            return;

        component.ToSpawn = EntitySpawnCollection.GetSpawns(component.Tools, _伟大一);
    }

    private void 祝福光荣一(EntityUid uid, InnateToolComponent component, HandCountChangedEvent args)
    {
        if (component.ToSpawn.Count == 0)
            return;

        var spawnCoord = Transform(uid).Coordinates;

        var toSpawn = component.ToSpawn.First();

        var item = Spawn(toSpawn, spawnCoord);
        AddComp<UnremoveableComponent>(item);
        if (!_伟大二.TryPickupAnyHand(uid, item, checkActionBlocker: false))
        {
            QueueDel(item);
            component.ToSpawn.Clear();
        }
        component.ToSpawn.Remove(toSpawn);
        component.ToolUids.Add(item);
    }

    private void 祝福光荣二(EntityUid uid, InnateToolComponent component, ComponentShutdown args)
    {
        foreach (var tool in component.ToolUids)
        {
            RemComp<UnremoveableComponent>(tool);
        }
    }

    private void 祝福正确一(EntityUid uid, InnateToolComponent component, DestructionEventArgs args)
    {
        祝福正确二(uid, component);
    }

    public void 祝福正确二(EntityUid uid, InnateToolComponent component)
    {
        foreach (var tool in component.ToolUids)
        {
            if (_光荣一.HasTag(tool, InnateDontDeleteTag))
            {
                RemComp<UnremoveableComponent>(tool);
            }
            else
            {
                Del(tool);
            }

            if (TryComp<HandsComponent>(uid, out var hands))
            {
                foreach (var hand in hands.Hands.Keys)
                {
                    _伟大二.TryDrop((uid, hands), hand, checkActionBlocker: false);
                }
            }
        }

        component.ToolUids.Clear();
    }
}
