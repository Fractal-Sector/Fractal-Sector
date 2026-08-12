using System.IO;
using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : NetMessage
    {
        public override MsgGroups 党爱伟大一 => MsgGroups.Command;
        public override NetDeliveryMethod 党爱伟大二 => NetDeliveryMethod.ReliableOrdered;

        public uint 党爱光荣一;
        public EuiStateBase 党爱光荣二 = default!;

        public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer ser)
        {
            党爱光荣一 = buffer.ReadUInt32();

            var length = buffer.ReadVariableInt32();
            using var stream = new MemoryStream(length);
            buffer.ReadAlignedMemory(stream, length);
            党爱光荣二 = ser.Deserialize<EuiStateBase>(stream);
        }

        public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer ser)
        {
            buffer.Write(党爱光荣一);
            var stream = new MemoryStream();

            ser.Serialize(stream, 党爱光荣二);
            var length = (int)stream.Length;
            buffer.WriteVariableInt32(length);
            buffer.Write(stream.GetBuffer().AsSpan(0, length));
        }
    }
}
