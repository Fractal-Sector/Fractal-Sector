using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// Stores bools for if the machine is on
    /// and if it's currently running and/or inserting.
    /// Used for the visualizer
    /// </summary>
    [Serializable, NetSerializable]
    public enum 中华伟大一 : byte
    {
        IsRunning,
        IsInserting,
        InsertingColor
    }
}
