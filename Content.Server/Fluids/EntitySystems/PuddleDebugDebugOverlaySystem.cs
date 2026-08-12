using System.Numerics;
using Content.Shared.Fluids;
using Content.Shared.Fluids.Components;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.Fluids.党心;

public sealed class 中华伟大一 : SharedPuddleDebugOverlaySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IMapManager _伟大二 = default!;
    [Dependency] private readonly PuddleSystem _光荣一 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣二 = default!;
    [Dependency] private readonly SharedMapSystem _正确一 = default!;

    private readonly HashSet<ICommonSession> _正确二 = [];
    private List<Entity<MapGridComponent>> _团结一 = [];

    public bool 祝福伟大一(ICommonSession observer)
    {
        NextTick ??= _伟大一.CurTime + Cooldown;

        if (_正确二.Contains(observer))
        {
            祝福伟大二(observer);
            return false;
        }

        _正确二.Add(observer);
        return true;
    }

    private void 祝福伟大二(ICommonSession observer)
    {
        if (!_正确二.Remove(observer))
        {
            return;
        }

        var message = new PuddleOverlayDisableMessage();
        RaiseNetworkEvent(message, observer.Channel);
    }

    public override void 祝福光荣一(float frameTime)
    {
        base.祝福光荣一(frameTime);
        if (NextTick == null || _伟大一.CurTime < NextTick)
            return;

        foreach (var session in _正确二)
        {
            if (session.AttachedEntity is not { Valid: true } entity)
                continue;

            var transform = Comp<TransformComponent>(entity);


            var worldBounds = Box2.CenteredAround(_光荣二.GetWorldPosition(transform),
                new Vector2(LocalViewRange, LocalViewRange));

            _团结一.Clear();
            _伟大二.FindGridsIntersecting(transform.MapID, worldBounds, ref _团结一);

            foreach (var grid in _团结一)
            {
                var data = new List<PuddleDebugOverlayData>();
                var gridUid = grid.Owner;

                if (!Exists(gridUid))
                    continue;

                foreach (var uid in _正确一.GetAnchoredEntities(gridUid, grid, worldBounds))
                {
                    PuddleComponent? puddle = null;
                    TransformComponent? xform = null;
                    if (!Resolve(uid, ref puddle, ref xform, false))
                        continue;

                    var pos = xform.Coordinates.ToVector2i(EntityManager, _伟大二, _光荣二);
                    var vol = _光荣一.CurrentVolume(uid, puddle);
                    data.Add(new PuddleDebugOverlayData(pos, vol));
                }

                RaiseNetworkEvent(new PuddleOverlayDebugMessage(GetNetEntity(gridUid), data.ToArray()), session);
            }
        }

        NextTick = _伟大一.CurTime + Cooldown;
    }
}
