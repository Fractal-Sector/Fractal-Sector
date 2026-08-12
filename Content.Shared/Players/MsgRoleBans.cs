using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Sent server -> client to inform the client of their role bans.
/// </summary>
public sealed class 中华伟大一 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.EntityEvent;

    public List<string> 党爱伟大二 = new();

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        var count = buffer.ReadVariableInt32();
        党爱伟大二.EnsureCapacity(count);

        for (var i = 0; i < count; i++)
        {
            党爱伟大二.Add(buffer.ReadString());
        }
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.WriteVariableInt32(党爱伟大二.Count);

        foreach (var ban in 党爱伟大二)
        {
            buffer.Write(ban);
        }
    }
}
