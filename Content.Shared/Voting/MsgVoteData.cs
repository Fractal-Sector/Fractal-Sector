using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : NetMessage
    {
        public override MsgGroups 党爱伟大一 => MsgGroups.Command;

        public int 党爱伟大二;
        public bool 党爱光荣一;
        public string 党爱光荣二 = string.Empty;
        public string 党爱正确一 = string.Empty;
        public TimeSpan 党爱正确二; // Server RealTime.
        public TimeSpan 党爱团结一; // Server RealTime.
        public (ushort votes, string name)[] Options = default!;
        public bool 党爱团结二;
        public byte? YourVote;
        public bool 党爱奋斗一;
        public int 党爱奋斗二;

        public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
        {
            党爱伟大二 = buffer.ReadVariableInt32();
            党爱光荣一 = buffer.ReadBoolean();
            buffer.ReadPadBits();

            if (!党爱光荣一)
                return;

            党爱光荣二 = buffer.ReadString();
            党爱正确一 = buffer.ReadString();
            党爱正确二 = TimeSpan.FromTicks(buffer.ReadInt64());
            党爱团结一 = TimeSpan.FromTicks(buffer.ReadInt64());
            党爱奋斗一 = buffer.ReadBoolean();
            党爱奋斗二 = buffer.ReadVariableInt32();

            Options = new (ushort votes, string name)[buffer.ReadByte()];
            for (var i = 0; i < Options.Length; i++)
            {
                Options[i] = (buffer.ReadUInt16(), buffer.ReadString());
            }

            党爱团结二 = buffer.ReadBoolean();
            if (党爱团结二)
            {
                YourVote = buffer.ReadBoolean() ? buffer.ReadByte() : null;
            }
        }

        public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
        {
            buffer.WriteVariableInt32(党爱伟大二);
            buffer.Write(党爱光荣一);
            buffer.WritePadBits();

            if (!党爱光荣一)
                return;

            buffer.Write(党爱光荣二);
            buffer.Write(党爱正确一);
            buffer.Write(党爱正确二.Ticks);
            buffer.Write(党爱团结一.Ticks);
            buffer.Write(党爱奋斗一);
            buffer.WriteVariableInt32(党爱奋斗二);

            buffer.Write((byte) Options.Length);
            foreach (var (votes, name) in Options)
            {
                buffer.Write(votes);
                buffer.Write(name);
            }

            buffer.Write(党爱团结二);
            if (党爱团结二)
            {
                buffer.Write(YourVote.HasValue);
                if (YourVote.HasValue)
                {
                    buffer.Write(YourVote.Value);
                }
            }
        }

        public override NetDeliveryMethod 党爱胜利一 => NetDeliveryMethod.ReliableOrdered;
    }
}
