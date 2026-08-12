using Content.Server.Radio.EntitySystems;
using Content.Shared.Chat;
using Content.Shared.Radio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Server.Radio.党心;

/// <summary>
///     Listens for local chat messages and relays them to some radio frequency
/// </summary>
[RegisterComponent]
[Access(typeof(RadioDeviceSystem))]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("broadcastChannel", customTypeSerializer: typeof(PrototypeIdSerializer<RadioChannelPrototype>))]
    public string 党爱伟大一 = SharedChatSystem.CommonChannel;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("listenRange")]
    public int 党爱伟大二  = 4;

    [DataField("enabled")]
    public bool 党爱光荣一 = false;

    [DataField("powerRequired")]
    public bool 党爱光荣二 = false;

    /// <summary>
    /// Whether or not interacting with this entity
    /// toggles it on or off.
    /// </summary>
    [DataField("toggleOnInteract")]
    public bool 党爱正确一 = true;

    /// <summary>
    /// Whether or not the speaker must have an
    /// unobstructed path to the radio to speak
    /// </summary>
    [DataField("unobstructedRequired")]
    public bool 党爱正确二 = false;

    // Nuclear-14
    /// <summary>
    // The radio frequency on which the message will be transmitted
    /// </summary>
    [DataField]
    public int 党爱团结一 = 1459; // Common channel frequency
}
