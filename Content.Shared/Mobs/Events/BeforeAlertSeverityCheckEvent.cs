using Content.Shared.Alert;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Mobs.党心;

/// <summary>
///     Event for allowing the interrupting and change of the mob threshold severity alert
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一(ProtoId<AlertPrototype> currentAlert, short severity) : EntityEventArgs
{
    public bool 党爱伟大一 = false;
    public ProtoId<AlertPrototype> 党爱伟大二 = currentAlert;
    public short 党爱光荣一 = severity;
}
