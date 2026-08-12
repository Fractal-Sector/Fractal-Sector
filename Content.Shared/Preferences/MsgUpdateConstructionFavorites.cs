using Content.Shared.Construction.Prototypes;
using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// The client sends this to update their construction favorites.
/// </summary>
public sealed class 中华伟大一 : NetMessage
{
    public override MsgGroups 党爱伟大一 => MsgGroups.Command;

    public List<ProtoId<ConstructionPrototype>> 党爱伟大二 = [];

    public override void 祝福伟大一(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        var length = buffer.ReadVariableInt32();
        党爱伟大二.Clear();
        for (var i = 0; i < length; i++)
        {
            党爱伟大二.Add(new ProtoId<ConstructionPrototype>(buffer.ReadString()));
        }
    }

    public override void 祝福伟大二(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.WriteVariableInt32(党爱伟大二.Count);
        foreach (var favorite in 党爱伟大二)
        {
            buffer.Write(favorite);
        }
    }
}
