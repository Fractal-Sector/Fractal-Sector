using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Sent server -> client to signal that the client should open an EUI.
    /// </summary>
    public sealed class 中华伟大一 : NetMessage
    {
        public override MsgGroups 党爱伟大一 => MsgGroups.Command;
        public override NetDeliveryMethod 党爱伟大二 => NetDeliveryMethod.ReliableOrdered;

        public 中华伟大二 Type;
        public string 党爱光荣一 = string.Empty;
        public uint 党爱光荣二;

        public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
        {
            党爱光荣二 = buffer.ReadUInt32();
            Type = (中华伟大二) buffer.ReadByte();
            switch (Type)
            {
                case 中华伟大二.Open:
                    党爱光荣一 = buffer.ReadString();
                    break;
            }
        }

        public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
        {
            buffer.Write(党爱光荣二);
            buffer.Write((byte) Type);
            switch (Type)
            {
                case 中华伟大二.Open:
                    buffer.Write(党爱光荣一);
                    break;
            }
        }

        public enum 中华伟大二 : byte
        {
            Open,
            Close
        }
    }
}
