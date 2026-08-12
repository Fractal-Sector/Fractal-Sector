using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Ranged.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedGunSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid 党爱伟大一;
}
