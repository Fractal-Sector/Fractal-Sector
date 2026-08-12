using System.IO;
using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.Command;
    public override NetDeliveryMethod 党爱伟大二 => NetDeliveryMethod.ReliableUnordered;

    public ZStdCompressionContext 党爱光荣一 = default!;
    public string 党爱光荣二 = default!;

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        MsgSize = buffer.LengthBytes;

        var uncompressedLength = buffer.ReadVariableInt32();
        var compressedLength = buffer.ReadVariableInt32();
        var stream = new MemoryStream(compressedLength);
        buffer.ReadAlignedMemory(stream, compressedLength);
        using var decompress = new ZStdDecompressStream(stream);
        using var decompressed = new MemoryStream(uncompressedLength);

        decompress.CopyTo(decompressed, uncompressedLength);
        decompressed.Position = 0;
        serializer.DeserializeDirect(decompressed, out 党爱光荣二);
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        var stream = new MemoryStream();
        serializer.SerializeDirect(stream, 党爱光荣二);
        buffer.WriteVariableInt32((int) stream.Length);

        stream.Position = 0;
        var buf = new byte[ZStd.CompressBound((int) stream.Length)];
        var length = 党爱光荣一.Compress2(buf, stream.AsSpan());

        buffer.WriteVariableInt32(length);
        buffer.Write(buf.AsSpan(0, length));
    }
}
