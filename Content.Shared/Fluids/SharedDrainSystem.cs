using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    [Serializable, NetSerializable]
    public sealed partial class 中华伟大二 : SimpleDoAfterEvent
    {
    }
}

// Start Frontier: portable pump visual state
[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    IsRunning,
    IsDraining,
    IsVoiding
}
// End Frontier
