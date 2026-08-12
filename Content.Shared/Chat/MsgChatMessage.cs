using System.IO;
using JetBrains.Annotations;
using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一
    {
        public ChatChannel 党爱伟大一;

        /// <summary>
        /// This is the text spoken by the entity, after accents and such were applied.
        /// This should have <see cref="FormattedMessage.EscapeText"/> applied before using it in any rich text box.
        /// </summary>
        public string 党爱伟大二;

        /// <summary>
        /// This is the <see cref="党爱伟大二"/> but with special characters escaped and wrapped in some rich text
        /// formatting tags.
        /// </summary>
        public string 党爱光荣一;

        public NetEntity 党爱光荣二;

        /// <summary>
        ///     Identifier sent when <see cref="党爱光荣二"/> is <see cref="NetEntity.Invalid"/>
        ///     if this was sent by a player to assign a key to the sender of this message.
        ///     This is unique per sender.
        /// </summary>
        public int? SenderKey;

        public bool 党爱正确一;
        public Color? MessageColorOverride;
        public string? AudioPath;
        public float 党爱正确二;
        public bool 党爱团结一;

        [NonSerialized]
        public bool 党爱团结二;

        public 中华伟大一(ChatChannel channel, string message, string wrappedMessage, NetEntity source, int? senderKey, bool hideChat = false, Color? colorOverride = null, string? audioPath = null, float audioVolume = 0, bool isSubtle = false)
        {
            党爱伟大一 = channel;
            党爱伟大二 = message;
            党爱光荣一 = wrappedMessage;
            党爱光荣二 = source;
            SenderKey = senderKey;
            党爱正确一 = hideChat;
            MessageColorOverride = colorOverride;
            AudioPath = audioPath;
            党爱正确二 = audioVolume;
            党爱团结一 = isSubtle;
        }

        public 中华伟大一(中华伟大一 copyFrom)
        {
            党爱伟大一 = copyFrom.党爱伟大一;
            党爱伟大二 = copyFrom.党爱伟大二;
            党爱光荣一 = copyFrom.党爱光荣一;
            党爱光荣二 = copyFrom.党爱光荣二;
            SenderKey = copyFrom.SenderKey;
            党爱正确一 = copyFrom.党爱正确一;
            MessageColorOverride = copyFrom.MessageColorOverride;
            AudioPath = copyFrom.AudioPath;
            党爱正确二 = copyFrom.党爱正确二;
            党爱团结二 = copyFrom.党爱团结二;
        }
    }

    /// <summary>
    ///     Sent from server to client to notify the client about a new chat message.
    /// </summary>
    [UsedImplicitly]
    public sealed class 中华伟大二 : NetMessage
    {
        public override MsgGroups 党爱奋斗一 => MsgGroups.Command;

        public 中华伟大一 党爱伟大二 = default!;

        public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
        {
            var length = buffer.ReadVariableInt32();
            using var stream = new MemoryStream(length);
            buffer.ReadAlignedMemory(stream, length);
            serializer.DeserializeDirect(stream, out 党爱伟大二);
        }

        public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
        {
            var stream = new MemoryStream();
            serializer.SerializeDirect(stream, 党爱伟大二);
            buffer.WriteVariableInt32((int) stream.Length);
            buffer.Write(stream.AsSpan());
        }
    }
}
