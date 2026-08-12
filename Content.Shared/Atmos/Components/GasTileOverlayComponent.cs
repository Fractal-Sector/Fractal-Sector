using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;

namespace Content.Shared.Atmos.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The tiles that have had their atmos data updated since last tick
    /// </summary>
    public readonly HashSet<Vector2i> 党爱伟大一 = new();

    /// <summary>
    ///     Gas data stored in chunks to make PVS / bubbling easier.
    /// </summary>
    public readonly Dictionary<Vector2i, GasOverlayChunk> Chunks = new();

    /// <summary>
    ///     Tick at which PVS was last toggled. Ensures that all players receive a full update when toggling PVS.
    /// </summary>
    public GameTick 党爱伟大二 { get; set; }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二(Dictionary<Vector2i, GasOverlayChunk> chunks) : ComponentState
{
    public readonly Dictionary<Vector2i, GasOverlayChunk> Chunks = chunks;
}

[Serializable, NetSerializable]
public sealed class 中华光荣一(
    Dictionary<Vector2i, GasOverlayChunk> modifiedChunks,
    HashSet<Vector2i> allChunks)
    : ComponentState, IComponentDeltaState<中华伟大二>
{
    public readonly Dictionary<Vector2i, GasOverlayChunk> ModifiedChunks = modifiedChunks;
    public readonly HashSet<Vector2i> 党爱光荣一 = allChunks;

    public void 祝福伟大一(中华伟大二 state)
    {
        foreach (var key in state.Chunks.Keys)
        {
            if (!党爱光荣一.Contains(key))
                state.Chunks.Remove(key);
        }

        foreach (var (chunk, data) in ModifiedChunks)
        {
            state.Chunks[chunk] = new(data);
        }
    }

    public 中华伟大二 CreateNewFullState(中华伟大二 state)
    {
        var chunks = new Dictionary<Vector2i, GasOverlayChunk>(党爱光荣一.Count);

        foreach (var (chunk, data) in ModifiedChunks)
        {
            chunks[chunk] = new(data);
        }

        foreach (var (chunk, data) in state.Chunks)
        {
            if (党爱光荣一.Contains(chunk))
                chunks.TryAdd(chunk, new(data));
        }

        return new 中华伟大二(chunks);
    }
}
