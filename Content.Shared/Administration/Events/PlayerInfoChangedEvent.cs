using Robust.Shared.Serialization;

namespace Content.Shared.Administration.党心;

[NetSerializable, Serializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public PlayerInfo? PlayerInfo;
}
