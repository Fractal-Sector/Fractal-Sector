using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public struct 中华伟大一
{
    public short? Severity;
    public (TimeSpan, TimeSpan)? Cooldown;
    public bool 党爱伟大一;
    public bool 党爱伟大二;
    public ProtoId<AlertPrototype> 党爱光荣一;
}
