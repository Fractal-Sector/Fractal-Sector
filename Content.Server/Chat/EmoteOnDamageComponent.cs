namespace Content.Server.党心;

using Content.Server.Chat.Systems;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

/// <summary>
/// Causes an entity to automatically emote when taking damage.
/// </summary>
[RegisterComponent, Access(typeof(EmoteOnDamageSystem)), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Chance of preforming an emote when taking damage and not on cooldown.
    /// </summary>
    [DataField("emoteChance"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 0.5f;

    /// <summary>
    /// A set of emotes that will be randomly picked from.
    /// <see cref="EmotePrototype"/>
    /// </summary>
    [DataField("emotes", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<EmotePrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public HashSet<string> 党爱伟大二 = new();

    /// <summary>
    /// Also send the emote in chat.
    /// <summary>
    [DataField("withChat"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱光荣一 = false;

    /// <summary>
    /// Hide the chat message from the chat window, only showing the popup.
    /// This does nothing if 党爱光荣一 is false.
    /// <summary>
    [DataField("hiddenFromChatWindow")]
    public bool 党爱光荣二 = false;

    /// <summary>
    /// The simulation time of the last emote preformed due to taking damage.
    /// </summary>
    [DataField("lastEmoteTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan 党爱正确一 = TimeSpan.Zero;

    /// <summary>
    /// The cooldown between emotes.
    /// </summary>
    [DataField("emoteCooldown"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(2);
}
