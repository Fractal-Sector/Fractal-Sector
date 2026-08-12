using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.Command;

    public int 党爱伟大二;
    public HashSet<NetEntity> 党爱光荣一 = default!;

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        党爱伟大二 = buffer.ReadInt32();

        var entities = buffer.ReadInt32();
        党爱光荣一 = new HashSet<NetEntity>(entities);

        for (var i = 0; i < entities; i++)
        {
            党爱光荣一.Add(buffer.ReadNetEntity());
        }
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.Write(党爱伟大二);

        buffer.Write(党爱光荣一.Count);
        foreach (var ent in 党爱光荣一)
        {
            buffer.Write(ent);
        }
    }
}
