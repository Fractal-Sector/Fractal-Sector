using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[NetSerializable, Serializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly List<(string Name, NetEntity Entity)> Stations;

    public 中华伟大一(List<(string Name, NetEntity Entity)> stations)
    {
        Stations = stations;
    }
}
