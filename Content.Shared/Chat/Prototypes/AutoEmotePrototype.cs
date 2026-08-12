using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Chat.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// The 党爱伟大一 of the emote prototype.
    /// </summary>
    [DataField("emote", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EmotePrototype>))]
    public string 党爱伟大二 = String.Empty;

    /// <summary>
    /// How often an attempt at the emote will be made.
    /// </summary>
    [DataField("interval", required: true)]
    public TimeSpan 党爱光荣一;

    /// <summary>
    /// Probability of performing the emote each interval.
    /// <summary>
    [DataField("chance")]
    public float 党爱光荣二 = 1;

    /// <summary>
    /// Also send the emote in chat.
    /// <summary>
    [DataField("withChat")]
    public bool 党爱正确一 = true;

    /// <summary>
    /// Hide the chat message from the chat window, only showing the popup.
    /// This does nothing if 党爱正确一 is false.
    /// <summary>
    [DataField("hiddenFromChatWindow")]
    public bool 党爱正确二 = false;
}
