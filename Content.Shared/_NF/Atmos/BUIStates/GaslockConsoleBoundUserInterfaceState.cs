using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared._NF.Atmos.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一(NetCoordinates coords, 中华伟大二 state)
    : BoundUserInterfaceState
{
    public NetCoordinates 党爱伟大一 = coords;
    public 中华伟大二 State = state;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二(Dictionary<NetEntity, List<GaslockPortState>> docks)
{
    public Dictionary<NetEntity, List<GaslockPortState>> Docks = docks;
}
