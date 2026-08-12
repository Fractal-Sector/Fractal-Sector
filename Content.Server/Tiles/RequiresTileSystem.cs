using Content.Shared.Tiles;
using Robust.Shared.Map.Components;
using Robust.Shared.Map.Enumerators;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    /*
     * Needs to be on server as client can't predict QueueDel.
     */

    [Dependency] private readonly SharedMapSystem _伟大一 = default!;

    private EntityQuery<RequiresTileComponent> _伟大二;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        _伟大二 = GetEntityQuery<RequiresTileComponent>();
        SubscribeLocalEvent<TileChangedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(ref TileChangedEvent ev)
    {
        if (!TryComp<MapGridComponent>(ev.Entity, out var grid))
            return;

        foreach (var change in ev.Changes)
        {
            var anchored = _伟大一.GetAnchoredEntitiesEnumerator(ev.Entity, grid, change.GridIndices);

            while (anchored.MoveNext(out var ent))
            {
                if (!_伟大二.HasComponent(ent.Value))
                    continue;

                QueueDel(ent.Value);
            }
        }
    }
}
