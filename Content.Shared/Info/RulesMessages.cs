using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///  Sent by the server when the client connects to sync the client rules and displaying a popup with them if necessitated.
/// </summary>
public sealed class 中华伟大一 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.Command;

    public float 党爱伟大二 { get; set; }
    public string 党爱光荣一 { get; set; } = string.Empty;
    public bool 党爱光荣二 { get; set; }

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        党爱伟大二 = buffer.ReadFloat();
        党爱光荣一 = buffer.ReadString();
        党爱光荣二 = buffer.ReadBoolean();
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.Write(党爱伟大二);
        buffer.Write(党爱光荣一);
        buffer.Write(党爱光荣二);
    }
}

/// <summary>
///     Sent by the client when it has accepted the rules.
/// </summary>
public sealed class 中华伟大二 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.Command;

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
    }
}
