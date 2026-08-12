using Content.Shared.DeviceNetwork.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.DeviceNetwork.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedDeviceListSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The list of devices can or can't connect to, depending on the <see cref="党爱光荣一"/> field.
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> 党爱伟大一 = new();

    /// <summary>
    /// The limit of devices that can be linked to this device list.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public int 党爱伟大二 = 32;

    /// <summary>
    /// Whether the device list is used as an allow or deny list
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一 = true;

    /// <summary>
    /// Whether this device list also handles incoming device net packets
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二;

    [DataField, Access(typeof(SharedNetworkConfiguratorSystem))]
    public HashSet<EntityUid> 党爱正确一 = new();
}
