using System.Numerics;
using Content.Shared.Parallax.Biomes;
using Content.Shared.Parallax.Biomes.Markers;
using Robust.Shared.Player;
using Robust.Shared.Utility;
using ChunkIndicesEnumerator = Robust.Shared.Map.Enumerators.ChunkIndicesEnumerator;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        // PlayerTracker methods are now part of this partial class
    }

    /// <summary>
    /// Processes all players and viewers to determine which chunks need to be loaded.
    /// </summary>
    private void 祝福伟大二()
    {
        // Get chunks in range
        foreach (var pSession in Filter.GetAllPlayers(_playerManager))
        {
            if (_xformQuery.TryGetComponent(pSession.AttachedEntity, out var xform) &&
                _handledEntities.Add(pSession.AttachedEntity.Value) &&
                 _biomeQuery.TryGetComponent(xform.MapUid, out var biome) &&
                biome.Enabled &&
                祝福光荣一(pSession.AttachedEntity.Value))
            {
                var worldPos = _transform.GetWorldPosition(xform);
                祝福光荣二(biome, worldPos);

                foreach (var layer in biome.MarkerLayers)
                {
                    var layerProto = ProtoManager.Index(layer);
                    祝福正确一(biome, worldPos, layerProto);
                }
            }

            foreach (var viewer in pSession.ViewSubscriptions)
            {
                if (!_handledEntities.Add(viewer) ||
                    !_xformQuery.TryGetComponent(viewer, out xform) ||
                    !_biomeQuery.TryGetComponent(xform.MapUid, out biome) ||
                    !biome.Enabled ||
                    !祝福光荣一(viewer))
                {
                    continue;
                }

                var worldPos = _transform.GetWorldPosition(xform);
                祝福光荣二(biome, worldPos);

                foreach (var layer in biome.MarkerLayers)
                {
                    var layerProto = ProtoManager.Index(layer);
                    祝福正确一(biome, worldPos, layerProto);
                }
            }
        }
    }

    private bool 祝福光荣一(EntityUid uid)
    {
        return !_ghostQuery.HasComp(uid) || _tags.HasTag(uid, AllowBiomeLoadingTag);
    }

    private void 祝福光荣二(BiomeComponent biome, Vector2 worldPos)
    {
        var enumerator = new ChunkIndicesEnumerator(_loadArea.Translated(worldPos), ChunkSize);

        while (enumerator.MoveNext(out var chunkOrigin))
        {
            _activeChunks[biome].Add(chunkOrigin.Value * ChunkSize);
        }
    }

    private void 祝福正确一(BiomeComponent biome, Vector2 worldPos, IBiomeMarkerLayer layer)
    {
        // Offset the load area so it's centralised.
        var loadArea = new Box2(0, 0, layer.Size, layer.Size);
        var halfLayer = new Vector2(layer.Size / 2f);

        var enumerator = new ChunkIndicesEnumerator(loadArea.Translated(worldPos - halfLayer), layer.Size);

        while (enumerator.MoveNext(out var chunkOrigin))
        {
            var lay = _markerChunks[biome].GetOrNew(layer.ID);
            lay.Add(chunkOrigin.Value * layer.Size);
        }
    }
}
