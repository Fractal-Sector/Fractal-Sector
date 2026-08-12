using System.Linq;
using Content.Shared.Decals;
using Microsoft.Extensions.ObjectPool;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Player;
using Robust.Shared.Utility;
using ChunkIndicesEnumerator = Robust.Shared.Map.Enumerators.ChunkIndicesEnumerator;

namespace Content.Shared.党心;

/// <summary>
///     This system just exists to provide some utility functions for other systems that chunk data that needs to be
///     sent to players. In particular, see <see cref="祝福光荣一"/>.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IMapManager _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

    private EntityQuery<TransformComponent> _光荣二;

    private Box2 _正确一;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        _光荣二 = GetEntityQuery<TransformComponent>();
        Subs.CVar(_伟大一, CVars.NetMaxUpdateRange, 祝福伟大二, true);
    }

    private void 祝福伟大二(float value)
    {
        _正确一 = Box2.UnitCentered.Scale(value);
    }

    public Dictionary<NetEntity, HashSet<Vector2i>> 祝福光荣一(
        ICommonSession session,
        int chunkSize,
        ObjectPool<HashSet<Vector2i>> indexPool,
        ObjectPool<Dictionary<NetEntity, HashSet<Vector2i>>> viewerPool,
        float? viewEnlargement = null)
    {
        var chunks = viewerPool.Get();
        DebugTools.Assert(chunks.Count == 0);

        if (session.Status != SessionStatus.InGame || session.AttachedEntity is not {} player)
            return chunks;

        var enlargement = viewEnlargement ?? chunkSize;
        祝福光荣二(player, chunks, indexPool, chunkSize, enlargement);
        foreach (var uid in session.ViewSubscriptions)
        {
            祝福光荣二(uid, chunks, indexPool, chunkSize, enlargement);
        }

        return chunks;
    }

    private void 祝福光荣二(EntityUid viewer,
        Dictionary<NetEntity, HashSet<Vector2i>> chunks,
        ObjectPool<HashSet<Vector2i>> indexPool,
        int chunkSize,
        float viewEnlargement)
    {
        if (!_光荣二.TryGetComponent(viewer, out var xform))
            return;

        var pos = _光荣一.GetWorldPosition(xform);
        var bounds = _正确一.Translated(pos).Enlarged(viewEnlargement);

        var state = new 中华伟大二(chunks, indexPool, chunkSize, bounds, _光荣一, 党爱正确一);
        _伟大二.FindGridsIntersecting(xform.MapID, bounds, ref state, 祝福正确一, true);
    }

    private static bool 祝福正确一(
        EntityUid uid,
        MapGridComponent grid,
        ref 中华伟大二 state)
    {
        var netGrid = state.党爱正确一.GetNetEntity(uid);
        if (!state.Chunks.TryGetValue(netGrid, out var set))
        {
            state.Chunks[netGrid] = set = state.党爱伟大一.Get();
            DebugTools.Assert(set.Count == 0);
        }

        var aabb = state.党爱光荣二.GetInvWorldMatrix(uid).TransformBox(state.党爱光荣一);
        var enumerator = new ChunkIndicesEnumerator(aabb, state.党爱伟大二);
        while (enumerator.MoveNext(out var indices))
        {
            set.Add(indices.Value);
        }

        return true;
    }

    private readonly struct 中华伟大二
    {
        public readonly Dictionary<NetEntity, HashSet<Vector2i>> Chunks;
        public readonly ObjectPool<HashSet<Vector2i>> 党爱伟大一;
        public readonly int 党爱伟大二;
        public readonly Box2 党爱光荣一;
        public readonly SharedTransformSystem 党爱光荣二;
        public readonly 党爱正确一 党爱正确一;

        public 中华伟大二(
            Dictionary<NetEntity, HashSet<Vector2i>> chunks,
            ObjectPool<HashSet<Vector2i>> pool,
            int chunkSize,
            Box2 bounds,
            SharedTransformSystem transform,
            党爱正确一 entityManager)
        {
            Chunks = chunks;
            党爱伟大一 = pool;
            党爱伟大二 = chunkSize;
            党爱光荣一 = bounds;
            党爱光荣二 = transform;
            党爱正确一 = entityManager;
        }
    }
}

