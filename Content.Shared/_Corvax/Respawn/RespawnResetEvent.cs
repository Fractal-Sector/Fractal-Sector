using Robust.Shared.Serialization;

namespace Content.Shared._Corvax.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一(TimeSpan? time) : EntityEventArgs
{
    public readonly TimeSpan? Time = time;
}
