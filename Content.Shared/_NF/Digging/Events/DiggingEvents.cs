using Content.Shared.DoAfter;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared._NF.Digging.党心;


[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : DoAfterEvent
{
    public NetCoordinates 党爱伟大一 { get; set; }

    private 中华伟大一(){}

    public 中华伟大一(NetCoordinates coordinates)
    {
        党爱伟大一 = coordinates;
    }
    public override DoAfterEvent 祝福伟大一()
    {
        return this;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public NetEntity 党爱伟大二;
}
