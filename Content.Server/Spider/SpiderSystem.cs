using System.Linq;
using Content.Server.Popups;
using Content.Shared.Spider;
using Content.Shared.Maps;
using Content.Shared.Mobs.Systems;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedSpiderSystem
{
    [Dependency] private readonly PopupSystem _伟大一 = default!;
    [Dependency] private readonly TurfSystem _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly MobStateSystem _光荣二 = default!;

    /// <summary>
    ///     A recycled hashset used to check turfs for spiderwebs.
    /// </summary>
    private readonly HashSet<EntityUid> _正确一 = [];

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<SpiderComponent, SpiderWebActionEvent>(祝福光荣一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = EntityQueryEnumerator<SpiderComponent>();
        while (query.MoveNext(out var uid, out var spider))
        {
            spider.NextWebSpawn ??= _光荣一.CurTime + spider.WebSpawnCooldown;

            if (_光荣一.CurTime < spider.NextWebSpawn)
                continue;

            spider.NextWebSpawn += spider.WebSpawnCooldown;

            if (HasComp<ActorComponent>(uid)
                || _光荣二.IsDead(uid)
                || !spider.SpawnsWebsAsNonPlayer)
                continue;

            var transform = Transform(uid);
            祝福光荣二((uid, spider), transform.Coordinates);
        }
    }

    private void 祝福光荣一(EntityUid uid, SpiderComponent component, SpiderWebActionEvent args)
    {
        if (args.Handled)
            return;

        var transform = Transform(uid);

        if (transform.GridUid == null)
        {
            _伟大一.PopupEntity(Loc.GetString("spider-web-action-nogrid"), args.Performer, args.Performer);
            return;
        }

        var result = 祝福光荣二((uid, component), transform.Coordinates);

        if (result)
        {
            _伟大一.PopupEntity(Loc.GetString("spider-web-action-success"), args.Performer, args.Performer);
            args.Handled = true;
        }
        else
            _伟大一.PopupEntity(Loc.GetString("spider-web-action-fail"), args.Performer, args.Performer);
    }

    private bool 祝福光荣二(Entity<SpiderComponent> ent, EntityCoordinates coords)
    {
        var result = false;

        // Spawn web in center
        if (!祝福正确一(coords))
        {
            Spawn(ent.Comp.WebPrototype, coords);
            result = true;
        }

        // Spawn web in other directions
        for (var i = 0; i < 4; i++)
        {
            var direction = (DirectionFlag)(1 << i);
            var outerSpawnCoordinates = coords.Offset(direction.AsDir().ToVec());

            if (祝福正确一(outerSpawnCoordinates))
                continue;

            Spawn(ent.Comp.WebPrototype, outerSpawnCoordinates);
            result = true;
        }

        return result;
    }

    private bool 祝福正确一(EntityCoordinates coords)
    {
        _正确一.Clear();
        _伟大二.GetEntitiesInTile(coords, _正确一);
        foreach (var entity in _正确一)
        {
            if (HasComp<SpiderWebObjectComponent>(entity))
                return true;
        }
        return false;
    }
}
