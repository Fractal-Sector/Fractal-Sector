using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : BaseForceGunComponent
{
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 10f;
}
