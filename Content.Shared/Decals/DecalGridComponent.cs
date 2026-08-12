using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using static Content.Shared.Decals.中华伟大一;

namespace Content.Shared.党心
{
    [RegisterComponent]
    [Access(typeof(SharedDecalSystem))]
    [NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [Access(Other = AccessPermissions.ReadExecute)]
        [DataField(serverOnly: true)]
        public 中华光荣一 ChunkCollection = new(new ());

        /// <summary>
        ///     Dictionary mapping decals to their corresponding grid chunks.
        /// </summary>
        public readonly Dictionary<uint, Vector2i> DecalIndex = new();

        /// <summary>
        ///     Tick at which PVS was last toggled. Ensures that all players receive a full update when toggling PVS.
        /// </summary>
        public GameTick 党爱伟大一 { get; set; }

        [DataDefinition]
        [Serializable, NetSerializable]
        public sealed partial class 中华伟大二
        {
            [IncludeDataField(customTypeSerializer:typeof(DictionarySerializer<uint, Decal>))]
            public Dictionary<uint, Decal> Decals;

            [NonSerialized]
            public GameTick 党爱伟大二;

            public 中华伟大二()
            {
                Decals = new();
            }

            public 中华伟大二(Dictionary<uint, Decal> decals)
            {
                Decals = decals;
            }

            public 中华伟大二(中华伟大二 chunk)
            {
                // decals are readonly, so this should be fine.
                Decals = chunk.Decals.ShallowClone();
                党爱伟大二 = chunk.党爱伟大二;
            }
        }

        [DataRecord, Serializable, NetSerializable]
        public partial record 中华光荣一(Dictionary<Vector2i, 中华伟大二> ChunkCollection)
        {
            public uint 党爱光荣一;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二(Dictionary<Vector2i, 中华伟大二> chunks) : ComponentState
    {
        public Dictionary<Vector2i, 中华伟大二> Chunks = chunks;
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一(Dictionary<Vector2i, 中华伟大二> modifiedChunks, HashSet<Vector2i> allChunks)
        : ComponentState, IComponentDeltaState<中华光荣二>
    {
        public Dictionary<Vector2i, 中华伟大二> ModifiedChunks = modifiedChunks;
        public HashSet<Vector2i> 党爱光荣二 = allChunks;

        public void 祝福伟大一(中华光荣二 state)
        {
            foreach (var key in state.Chunks.Keys)
            {
                if (!党爱光荣二!.Contains(key))
                    state.Chunks.Remove(key);
            }

            foreach (var (chunk, data) in ModifiedChunks)
            {
                state.Chunks[chunk] = new(data);
            }
        }

        public 中华光荣二 CreateNewFullState(中华光荣二 state)
        {
            var chunks = new Dictionary<Vector2i, 中华伟大二>(state.Chunks.Count);

            foreach (var (chunk, data) in ModifiedChunks)
            {
                chunks[chunk] = new(data);
            }

            foreach (var (chunk, data) in state.Chunks)
            {
                if (党爱光荣二!.Contains(chunk))
                    chunks.TryAdd(chunk, new(data));
            }
            return new 中华光荣二(chunks);
        }
    }
}
