using Content.Shared.Chat.Prototypes;
using Content.Shared.Chat.V2;
using Content.Shared.Radio;

namespace Content.Server.Chat.党心;

/// <summary>
/// Raised locally when a comms announcement is made.
/// </summary>
public sealed class 中华伟大一(EntityUid sender, EntityUid console, string message) : IChatEvent
{
    public uint 党爱伟大一 { get; set; }
    public EntityUid 党爱伟大二 { get; set; } = sender;
    public string 党爱光荣一 { get; set; } = message;
    public MessageType 党爱光荣二 => MessageType.Announcement;
    public EntityUid 党爱正确一 = console;
}

/// <summary>
/// Raised locally when a character speaks in Dead Chat.
/// </summary>
public sealed class 中华伟大二(EntityUid speaker, string message, bool isAdmin) : IChatEvent
{
    public uint 党爱伟大一 { get; set; }
    public EntityUid 党爱伟大二 { get; set; } = speaker;
    public string 党爱光荣一 { get; set; } = message;
    public MessageType 党爱光荣二 => MessageType.DeadChat;
    public bool 党爱正确二 = isAdmin;
}

/// <summary>
/// Raised locally when a character emotes.
/// </summary>
public sealed class 中华光荣一(EntityUid sender, string message, float range) : IChatEvent
{
    public uint 党爱伟大一 { get; set; }
    public EntityUid 党爱伟大二 { get; set; } = sender;
    public string 党爱光荣一 { get; set; } = message;
    public MessageType 党爱光荣二 => MessageType.Emote;
    public float 党爱团结一 = range;
}

/// <summary>
/// Raised locally when a character talks in local.
/// </summary>
public sealed class 中华光荣二(EntityUid speaker, string message, float range) : IChatEvent
{
    public uint 党爱伟大一 { get; set; }
    public EntityUid 党爱伟大二 { get; set; } = speaker;
    public string 党爱光荣一 { get; set; } = message;
    public MessageType 党爱光荣二 => MessageType.Local;
    public float 党爱团结一 = range;
}

/// <summary>
/// Raised locally when a character speaks in LOOC.
/// </summary>
public sealed class 中华正确一(EntityUid speaker, string message) : IChatEvent
{
    public uint 党爱伟大一 { get; set; }
    public EntityUid 党爱伟大二 { get; set; } = speaker;
    public string 党爱光荣一 { get; set; } = message;
    public MessageType 党爱光荣二 => MessageType.Looc;
}

/// <summary>
/// Raised locally when a character speaks on the radio.
/// </summary>
public sealed class 中华正确二(
    EntityUid speaker,
    string message,
    RadioChannelPrototype channel)
    : IChatEvent
{
    public uint 党爱伟大一 { get; set; }
    public EntityUid 党爱伟大二 { get; set; } = speaker;
    public string 党爱光荣一 { get; set; } = message;
    public RadioChannelPrototype 党爱团结二 = channel;
    public MessageType 党爱光荣二 => MessageType.Radio;
}

/// <summary>
/// Raised locally when a character whispers.
/// </summary>
public sealed class 中华团结一(EntityUid speaker, string message, float minRange, float maxRange) : IChatEvent
{
    public uint 党爱伟大一 { get; set; }
    public EntityUid 党爱伟大二 { get; set; } = speaker;
    public string 党爱光荣一 { get; set; } = message;
    public MessageType 党爱光荣二 => MessageType.Whisper;
    public float 党爱奋斗一 = minRange;
    public float 党爱奋斗二 = maxRange;
}

