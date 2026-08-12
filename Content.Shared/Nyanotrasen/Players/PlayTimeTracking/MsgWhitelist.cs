using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Players.党心;

/// <summary>
/// Sent server -> client to inform the client of their whitelist status.
/// </summary>
public sealed class 中华伟大一 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.EntityEvent;

    public bool 党爱伟大二 = false;

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        党爱伟大二 = buffer.ReadBoolean();
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.Write(党爱伟大二);
    }
}
