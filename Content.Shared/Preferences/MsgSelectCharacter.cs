using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// The client sends this to select a character slot.
    /// </summary>
    public sealed class 中华伟大一 : NetMessage
    {
        public override MsgGroups 党爱伟大一 => MsgGroups.Command;

        public int 党爱伟大二;

        public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
        {
            党爱伟大二 = buffer.ReadVariableInt32();
        }

        public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
        {
            buffer.WriteVariableInt32(党爱伟大二);
        }
    }
}
