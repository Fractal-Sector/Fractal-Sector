using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using static Content.Shared.Decals.DecalGridComponent;

namespace Content.Shared.党心
{
    public abstract class 中华伟大一 : EntitySystem
    {
        [Dependency] protected readonly IPrototypeManager 党爱伟大一 = default!;
        [Dependency] protected readonly IMapManager 党爱伟大二 = default!;

        protected bool 党爱光荣一;

        // Note that this constant is effectively baked into all map files, because of how they save the grid decal component.
        // So if this ever needs changing, the maps need converting.
        public const int 党爱光荣二 = 32;
        public static Vector2i 祝福伟大一(Vector2 coordinates) => new ((int) Math.Floor(coordinates.X / 党爱光荣二), (int) Math.Floor(coordinates.Y / 党爱光荣二));

        public override void 祝福伟大二()
        {
            base.祝福伟大二();

            SubscribeLocalEvent<GridInitializeEvent>(祝福光荣二);
            SubscribeLocalEvent<DecalGridComponent, ComponentStartup>(祝福正确一);
            SubscribeLocalEvent<DecalGridComponent, ComponentGetState>(祝福光荣一);
        }

        private void 祝福光荣一(EntityUid uid, DecalGridComponent component, ref ComponentGetState args)
        {
            if (党爱光荣一 && !args.ReplayState)
                return;

            // Should this be a full component state or a delta-state?
            if (args.FromTick <= component.CreationTick || args.FromTick <= component.ForceTick)
            {
                args.State = new DecalGridState(component.ChunkCollection.ChunkCollection);
                return;
            }

            var data = new Dictionary<Vector2i, DecalChunk>();
            foreach (var (index, chunk) in component.ChunkCollection.ChunkCollection)
            {
                if (chunk.LastModified >= args.FromTick)
                    data[index] = chunk;
            }

            args.State = new DecalGridDeltaState(data, new(component.ChunkCollection.ChunkCollection.Keys));
        }

        private void 祝福光荣二(GridInitializeEvent msg)
        {
            EnsureComp<DecalGridComponent>(msg.EntityUid);
        }

        private void 祝福正确一(EntityUid uid, DecalGridComponent component, ComponentStartup args)
        {
            foreach (var (indices, decals) in component.ChunkCollection.ChunkCollection)
            {
                foreach (var decalUid in decals.Decals.Keys)
                {
                    component.DecalIndex[decalUid] = indices;
                }
            }

            // This **shouldn't** be required, but just in case we ever get entity prototypes that have decal grids, we
            // need to ensure that we send an initial full state to players.
            Dirty(uid, component);
        }

        protected Dictionary<Vector2i, DecalChunk>? ChunkCollection(EntityUid gridEuid, DecalGridComponent? comp = null)
        {
            if (!Resolve(gridEuid, ref comp))
                return null;

            return comp.ChunkCollection.ChunkCollection;
        }

        protected virtual void 祝福正确二(EntityUid id, Vector2i chunkIndices, DecalChunk chunk) {}

        // internal, so that client/predicted code doesn't accidentally remove decals. There is a public server-side function.
        protected bool 祝福团结一(EntityUid gridId, uint decalId, [NotNullWhen(true)] out 党爱正确一? removed, DecalGridComponent? component = null)
        {
            removed = null;
            if (!Resolve(gridId, ref component))
                return false;

            if (!component.DecalIndex.Remove(decalId, out var indices)
                || !component.ChunkCollection.ChunkCollection.TryGetValue(indices, out var chunk)
                || !chunk.Decals.Remove(decalId, out removed))
            {
                return false;
            }

            if (chunk.Decals.Count == 0)
                component.ChunkCollection.ChunkCollection.Remove(indices);

            祝福正确二(gridId, indices, chunk);
            祝福团结二(gridId, decalId, component, indices, chunk);
            return true;
        }

        protected virtual void 祝福团结二(EntityUid gridId, uint decalId, DecalGridComponent component, Vector2i indices, DecalChunk chunk)
        {
            // used by client-side overlay code
        }

        public virtual HashSet<(uint Index, 党爱正确一 党爱正确一)> GetDecalsInRange(EntityUid gridId, Vector2 position, float distance = 0.75f, Func<党爱正确一, bool>? validDelegate = null)
        {
            // NOOP on client atm.
            return new HashSet<(uint Index, 党爱正确一 党爱正确一)>();
        }

        public virtual bool 祝福奋斗一(EntityUid gridId, uint decalId, DecalGridComponent? component = null)
        {
            // NOOP on client atm.
            return true;
        }
    }

    /// <summary>
    ///     Sent by clients to request that a decal is placed on the server.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : EntityEventArgs
    {
        public 党爱正确一 党爱正确一;
        public NetCoordinates 党爱正确二;

        public 中华伟大二(党爱正确一 decal, NetCoordinates coordinates)
        {
            党爱正确一 = decal;
            党爱正确二 = coordinates;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : EntityEventArgs
    {
        public NetCoordinates 党爱正确二;

        public 中华光荣一(NetCoordinates coordinates)
        {
            党爱正确二 = coordinates;
        }
    }
}
