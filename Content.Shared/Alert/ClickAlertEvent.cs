using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// A message that calls the click interaction on a alert
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly ProtoId<AlertPrototype> 党爱伟大一;

    public 中华伟大一(ProtoId<AlertPrototype> alertType)
    {
        党爱伟大一 = alertType;
    }
}
