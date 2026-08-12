using Content.Shared.Radio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.Radio.党心;

/// <summary>
///     This component is required to receive radio message events.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The channels that this radio is listening on.
    /// </summary>
    [DataField("channels", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<RadioChannelPrototype>))]
    public HashSet<string> 党爱伟大一 = new();

    /// <summary>
    /// A toggle for globally receiving all radio channels.
    /// Overrides <see cref="党爱伟大一"/>
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二;

    /// <summary>
    ///     If this radio can hear all messages on all maps
    /// </summary>
    [DataField("globalReceive")]
    public bool 党爱光荣一 = false;
}
