using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

/// <summary>
/// Sends message to request that the clicker teleports to the requested location
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二(NetEntity netEnt, string pointName) : BoundUserInterfaceMessage
{
    public NetEntity 党爱伟大一 = netEnt;
    public string 党爱伟大二 = pointName;
}
