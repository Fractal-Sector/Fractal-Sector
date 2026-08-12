using Content.Shared.DeviceLinking;
using Content.Shared.DeviceNetwork.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.DeviceNetwork.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedNetworkConfiguratorSystem))]
public sealed partial class 中华伟大一 : Component
{
    // AAAAA ALL OF THESE FAA
    /// <summary>
    /// Determines whether the configurator is in linking mode or list mode
    /// </summary>
    [DataField, AutoNetworkedField]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// The entity containing a <see cref="DeviceListComponent"/> this configurator is currently interacting with
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? ActiveDeviceList { get; set; }

    /// <summary>
    /// The entity containing a <see cref="DeviceLinkSourceComponent"/> or <see cref="DeviceLinkSinkComponent"/> this configurator is currently interacting with.<br/>
    /// If this is set the configurator is in linking mode.
    /// </summary>
    // TODO handle device deletion
    public EntityUid? ActiveDeviceLink;

    /// <summary>
    /// The target device this configurator is currently linking with the <see cref="ActiveDeviceLink"/>
    /// </summary>
    // TODO handle device deletion
    public EntityUid? DeviceLinkTarget;

    /// <summary>
    /// The list of devices stored in the configurator
    /// </summary>
    [DataField]
    public Dictionary<string, EntityUid> Devices = new();

    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(0.5);

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱光荣一;

    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Machines/custom_deny.ogg");

    [DataField]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Machines/quickbeep.ogg");
}
