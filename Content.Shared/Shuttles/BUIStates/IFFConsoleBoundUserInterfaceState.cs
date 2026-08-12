using Content.Shared.Shuttles.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public IFFFlags 党爱伟大一;
    public IFFFlags 党爱伟大二;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Key,
}
