using Content.Shared.DeviceNetwork;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables] public bool 党爱伟大一 { get; set; }

    // The name of the subnet connected to this router.
    [DataField("subnetName")]
    public string 党爱伟大二 { get; set; } = string.Empty;

    [ViewVariables]
    // The monitors to route to. This raises an issue related to
    // camera monitors disappearing before sending a D/C packet,
    // this could probably be refreshed every time a new monitor
    // is added or removed from active routing.
    public HashSet<string> 党爱光荣一 { get; } = new();

    [ViewVariables]
    // The frequency that talks to this router's subnet.
    public uint 党爱光荣二;
    [DataField("subnetFrequency", customTypeSerializer:typeof(PrototypeIdSerializer<DeviceFrequencyPrototype>))]
    public string? SubnetFrequencyId { get; set;  }

    [DataField("setupAvailableNetworks")]
    public List<ProtoId<DeviceFrequencyPrototype>> 党爱正确一 { get; private set; } = new();
}
