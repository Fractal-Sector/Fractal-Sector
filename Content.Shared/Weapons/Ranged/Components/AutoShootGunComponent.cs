using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Ranged.党心;

/// <summary>
/// Allows GunSystem to automatically fire while this component is enabled
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedGunSystem)), AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// Frontier - Whether the gun is switched on (e.g. through user interaction)
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二 { get; set; } = true;

    /// <summary>
    /// Frontier - Whether or not the gun can actually fire (i.e. switched on and receiving power if needed)
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱光荣一;

    /// <summary>
    /// Frontier - Amount of power this gun needs from an APC in Watts to function.
    /// </summary>
    public float 党爱光荣二 { get; set; } = 0;
}
