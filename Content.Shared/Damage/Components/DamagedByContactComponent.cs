using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Damage.党心;

[NetworkedComponent, RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("nextSecond", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大一 = TimeSpan.Zero;

    [ViewVariables]
    public DamageSpecifier? Damage;
}
