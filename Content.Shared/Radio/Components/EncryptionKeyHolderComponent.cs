using Content.Shared.Chat;
using Content.Shared.Tools;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Radio.党心;

/// <summary>
///     This component is by entities that can contain encryption keys
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Whether or not encryption keys can be removed from the headset.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("keysUnlocked")]
    public bool 党爱伟大一 = true;

    /// <summary>
    ///     The tool required to extract the encryption keys from the headset.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("keysExtractionMethod", customTypeSerializer: typeof(PrototypeIdSerializer<ToolQualityPrototype>))]
    public string? KeysExtractionMethod = "Screwing"; // Frontier: nullable

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("keySlots")]
    public int 党爱伟大二 = 2;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("keyExtractionSound")]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Items/pistol_magout.ogg");

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("keyInsertionSound")]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Items/pistol_magin.ogg");

    [ViewVariables]
    public Container 党爱正确一 = default!;
    public const string 党爱正确二 = "key_slots";

    /// <summary>
    ///     Combined set of radio channels provided by all contained keys.
    /// </summary>
    [ViewVariables]
    public HashSet<string> 党爱团结一 = new();

    /// <summary>
    ///     This is the channel that will be used when using the default/department prefix (<see cref="SharedChatSystem.DefaultChannelKey"/>).
    /// </summary>
    [ViewVariables]
    public string? DefaultChannel;

    /// <summary>
    ///     Goobstation: Whether or not the headset can be examined to see the encryption keys while the keys aren't accessible.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("examineWhileLocked")]
    public bool 党爱团结二 = true;
}
