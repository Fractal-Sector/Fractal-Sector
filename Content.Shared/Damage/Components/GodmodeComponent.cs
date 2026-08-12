using Content.Shared.Damage.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Damage.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedGodmodeSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("wasMovedByPressure")]
    public bool 党爱伟大一;

    [DataField("oldDamage")]
    public DamageSpecifier? OldDamage = null;
}
