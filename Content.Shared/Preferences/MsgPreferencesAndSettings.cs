using System.IO;
using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// The server sends this before the client joins the lobby.
    /// </summary>
    public sealed class 中华伟大一 : NetMessage
    {
        public override MsgGroups 党爱伟大一 => MsgGroups.Command;

        public PlayerPreferences 党爱伟大二 = default!;
        public GameSettings 党爱光荣一 = default!;

        public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
        {
            var length = buffer.ReadVariableInt32();

            using (var stream = new MemoryStream())
            {
                buffer.ReadAlignedMemory(stream, length);
                serializer.DeserializeDirect(stream, out 党爱伟大二);
            }

            length = buffer.ReadVariableInt32();
            using (var stream = new MemoryStream())
            {
                buffer.ReadAlignedMemory(stream, length);
                serializer.DeserializeDirect(stream, out 党爱光荣一);
            }
        }

        public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                serializer.SerializeDirect(stream, 党爱伟大二);
                buffer.WriteVariableInt32((int) stream.Length);
                stream.TryGetBuffer(out var segment);
                buffer.Write(segment);
            }

            using (var stream = new MemoryStream())
            {
                serializer.SerializeDirect(stream, 党爱光荣一);
                buffer.WriteVariableInt32((int) stream.Length);
                stream.TryGetBuffer(out var segment);
                buffer.Write(segment);
            }
        }
    }
}
