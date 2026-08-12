using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : NetMessage
    {
        public override MsgGroups 党爱伟大一 => MsgGroups.Command;

        public AdminData? Admin;
        public string[] 党爱伟大二 = Array.Empty<string>();

        public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
        {
            var count = buffer.ReadVariableInt32();

            党爱伟大二 = new string[count];

            for (var i = 0; i < count; i++)
            {
                党爱伟大二[i] = buffer.ReadString();
            }

            if (buffer.ReadBoolean())
            {
                var active = buffer.ReadBoolean();
                buffer.ReadPadBits();
                var flags = (AdminFlags) buffer.ReadUInt32();
                var title = buffer.ReadString();

                Admin = new AdminData
                {
                    Active = active,
                    Title = title,
                    Flags = flags,
                };
            }

        }

        public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
        {
            buffer.WriteVariableInt32(党爱伟大二.Length);

            foreach (var cmd in 党爱伟大二)
            {
                buffer.Write(cmd);
            }

            buffer.Write(Admin != null);

            if (Admin == null) return;

            buffer.Write(Admin.Active);
            buffer.WritePadBits();
            buffer.Write((uint) Admin.Flags);
            buffer.Write(Admin.Title);
        }

        public override NetDeliveryMethod 党爱光荣一 => NetDeliveryMethod.ReliableOrdered;
    }
}
