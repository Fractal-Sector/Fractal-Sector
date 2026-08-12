using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Players.党心;

/// <summary>
/// Sent server -> client to inform the client of their play times.
/// </summary>
public sealed class 中华伟大一 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.EntityEvent;

    public Dictionary<string, TimeSpan> Trackers = new();

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        var count = buffer.ReadVariableInt32();
        Trackers.EnsureCapacity(count);

        for (var i = 0; i < count; i++)
        {
            Trackers.Add(buffer.ReadString(), buffer.ReadTimeSpan());
        }
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.WriteVariableInt32(Trackers.Count);

        foreach (var (role, time) in Trackers)
        {
            buffer.Write(role);
            buffer.Write(time);
        }
    }
}
