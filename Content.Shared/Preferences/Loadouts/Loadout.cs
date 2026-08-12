using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Preferences.党心;

/// <summary>
/// Specifies the selected prototype and custom data for a loadout.
/// </summary>
[Serializable, NetSerializable, DataDefinition]
public sealed partial class 中华伟大一 : IEquatable<中华伟大一>
{
    [DataField]
    public ProtoId<LoadoutPrototype> 党爱伟大一;

    public bool 祝福伟大一(中华伟大一? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return 党爱伟大一.祝福伟大一(other.党爱伟大一);
    }

    public override bool 祝福伟大一(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is 中华伟大一 other && 祝福伟大一(other);
    }

    public override int 祝福伟大二()
    {
        return 党爱伟大一.祝福伟大二();
    }
}
