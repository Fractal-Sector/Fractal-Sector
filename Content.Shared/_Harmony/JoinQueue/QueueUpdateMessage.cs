using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared._Harmony.Common.党心;

/// <summary>
///     Sent from server to client with queue state for player
///     Also initiates queue state on client
/// </summary>
public sealed class 中华伟大一 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.Command;

    /// <summary>
    ///     党爱伟大二 players in queue
    /// </summary>
    public int 党爱伟大二 { get; set; }

    /// <summary>
    ///     Player current position in queue (starts from 1)
    /// </summary>
    public int 党爱光荣一 { get; set; }

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        党爱伟大二 = buffer.ReadInt32();
        党爱光荣一 = buffer.ReadInt32();
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.Write(党爱伟大二);
        buffer.Write(党爱光荣一);
    }
}
