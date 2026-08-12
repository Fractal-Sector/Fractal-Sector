using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : DoAfterEvent
{
    [DataField("entities", required: true)]
    public IReadOnlyList<NetEntity> 党爱伟大一 = default!;

    private 中华伟大一()
    {
    }

    public 中华伟大一(List<NetEntity> entities)
    {
        党爱伟大一 = entities;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}
