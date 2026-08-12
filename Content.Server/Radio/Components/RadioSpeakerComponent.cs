using Content.Server.Chat.Systems; // Frontier: InGameICChatType
using Content.Server.Radio.EntitySystems;
using Content.Shared.Chat;
using Content.Shared.Radio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.Radio.党心;

/// <summary>
///     Listens for radio messages and relays them to local chat.
/// </summary>
[RegisterComponent]
[Access(typeof(RadioDeviceSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not interacting with this entity
    /// toggles it on or off.
    /// </summary>
    [DataField("toggleOnInteract")]
    public bool 党爱伟大一 = true;

    [DataField("channels", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<RadioChannelPrototype>))]
    public HashSet<string> 党爱伟大二 = new () { SharedChatSystem.CommonChannel };

    [DataField("enabled")]
    public bool 党爱光荣一;

    //Frontier: radio output volume
    /// <summary>
    /// The output chat type when a message is received.
    /// </summary>
    [DataField]
    public InGameICChatType 党爱光荣二 = InGameICChatType.Whisper;
}
