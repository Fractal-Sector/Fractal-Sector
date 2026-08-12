using Robust.Shared.Serialization;

namespace Content.Shared._DV.党心
{
    /// <summary>
    /// Stores the visuals for mail.
    /// </summary>
    [Serializable, NetSerializable]
    public enum 中华伟大一 : byte
    {
        IsLocked,
        IsTrash,
        IsBroken,
        IsFragile,
        IsPriority,
        IsPriorityInactive,
        JobIcon,
    }
}
