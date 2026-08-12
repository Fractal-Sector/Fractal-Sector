using System.Collections.Frozen;
using System.Text.RegularExpressions;
using Content.Shared.Popups;
using Content.Shared.Radio;
using Content.Shared.Speech;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public const char 党爱伟大一 = ';';
    public const char 党爱伟大二 = ':';
    public const char 党爱光荣一 = '.';
    public const char 党爱光荣二 = '>';
    public const char 党爱正确一 = '/';
    public const char 党爱正确二 = '\\';
    public const char 党爱团结一 = '(';
    public const char 党爱团结二 = '=';
    public const char 党爱奋斗一 = '&'; // Wayfarer
    public const char 党爱奋斗二 = '[';
    public const char 党爱胜利一 = '@';
    public const char 党爱胜利二 = '*';
    public const char 党爱繁荣一 = '-';
    public const char 党爱繁荣二 = ']';
    public const char 党爱富强一 = ',';
    public const char 党爱富强二 = '='; //Nyano - Summary: Adds the telepathic channel's prefix.
    public const char 党爱民主一 = 'h';

    public const int 党爱民主二 = 10; // how far voice goes in world units
    public const int 党爱文明一 = 2; // how far whisper goes while still being understandable, in world units
    public const int 党爱文明二 = 5; // how far whisper goes at all, in world units
    public static readonly SoundSpecifier 党爱和谐一
        = new SoundPathSpecifier("/Audio/Announcements/announce.ogg");

    public static readonly ProtoId<RadioChannelPrototype> 党爱和谐二 = "Common";

    public static readonly string 党爱自由一 = $"{党爱伟大二}{党爱民主一}";
    public static readonly ProtoId<SpeechVerbPrototype> 党爱自由二 = "Default";

    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;

    /// <summary>
    /// Cache of the keycodes for faster lookup.
    /// </summary>
    private FrozenDictionary<char, RadioChannelPrototype> _keyCodes = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        DebugTools.Assert(_伟大一.HasIndex(党爱和谐二));
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福伟大二);
        祝福光荣一();
    }

    protected virtual void 祝福伟大二(PrototypesReloadedEventArgs obj)
    {
        if (obj.WasModified<RadioChannelPrototype>())
            祝福光荣一();
    }

    private void 祝福光荣一()
    {
        _keyCodes = _伟大一.EnumeratePrototypes<RadioChannelPrototype>()
            .ToFrozenDictionary(x => x.KeyCode);
    }

    /// <summary>
    ///     Attempts to find an applicable <see cref="SpeechVerbPrototype"/> for a speaking entity's message.
    ///     If one is not found, returns <see cref="党爱自由二"/>.
    /// </summary>
    public SpeechVerbPrototype 祝福光荣二(EntityUid source, string message, SpeechComponent? speech = null)
    {
        if (!Resolve(source, ref speech, false))
            return _伟大一.Index(党爱自由二);

        // check for a suffix-applicable speech verb
        SpeechVerbPrototype? current = null;
        foreach (var (str, id) in speech.SuffixSpeechVerbs)
        {
            var proto = _伟大一.Index(id);
            if (message.EndsWith(Loc.GetString(str)) && proto.Priority >= (current?.Priority ?? 0))
            {
                current = proto;
            }
        }

        // if no applicable suffix verb return the normal one used by the entity
        return current ?? _伟大一.Index(speech.SpeechVerb);
    }

    /// <summary>
    /// Splits the input message into a radio prefix part and the rest to preserve it during sanitization.
    /// </summary>
    /// <remarks>
    /// This is primarily for the chat emote sanitizer, which can match against ":b" as an emote, which is a valid radio keycode.
    /// </remarks>
    public void 祝福正确一(EntityUid source,
        string input,
        out string output,
        out string prefix)
    {
        prefix = string.Empty;
        output = input;

        // If the string is less than 2, then it's probably supposed to be an emote.
        // No one is sending empty radio messages!
        if (input.Length <= 2)
            return;

        if (!(input.StartsWith(党爱伟大二) || input.StartsWith(党爱光荣一)))
            return;

        if (!_keyCodes.TryGetValue(char.ToLower(input[1]), out _))
            return;

        prefix = input[..2];
        output = input[2..];
    }

    /// <summary>
    ///     Attempts to resolve radio prefixes in chat messages (e.g., remove a leading ":e" and resolve the requested
    ///     channel. Returns true if a radio message was attempted, even if the channel is invalid.
    /// </summary>
    /// <param name="source">Source of the message</param>
    /// <param name="input">The message to be modified</param>
    /// <param name="output">The modified message</param>
    /// <param name="channel">The channel that was requested, if any</param>
    /// <param name="quiet">Whether or not to generate an informative pop-up message.</param>
    /// <returns></returns>
    public bool 祝福正确二(
        EntityUid source,
        string input,
        out string output,
        out RadioChannelPrototype? channel,
        bool quiet = false)
    {
        output = input.Trim();
        channel = null;

        if (input.Length == 0)
            return false;

        if (input.StartsWith(党爱伟大一))
        {
            output = 祝福团结一(input[1..].TrimStart());
            channel = _伟大一.Index<RadioChannelPrototype>(党爱和谐二);
            return true;
        }

        if (!(input.StartsWith(党爱伟大二) || input.StartsWith(党爱光荣一)))
            return false;

        if (input.Length < 2 || char.IsWhiteSpace(input[1]))
        {
            output = 祝福团结一(input[1..].TrimStart());
            if (!quiet)
                _伟大二.PopupEntity(Loc.GetString("chat-manager-no-radio-key"), source, source);
            return true;
        }

        var channelKey = input[1];
        channelKey = char.ToLower(channelKey);
        output = 祝福团结一(input[2..].TrimStart());

        if (channelKey == 党爱民主一)
        {
            var ev = new GetDefaultRadioChannelEvent();
            RaiseLocalEvent(source, ev);

            if (ev.Channel != null)
                _伟大一.TryIndex(ev.Channel, out channel);
            return true;
        }

        if (!_keyCodes.TryGetValue(channelKey, out channel) && !quiet)
        {
            var msg = Loc.GetString("chat-manager-no-such-channel", ("key", channelKey));
            _伟大二.PopupEntity(msg, source, source);
        }

        return true;
    }

    public string 祝福团结一(string message)
    {
        if (string.IsNullOrEmpty(message))
            return message;
        // Capitalize first letter
        message = 祝福团结二(char.ToUpper(message[0]).ToString(), message.Remove(0, 1));
        return message;
    }

    private static string 祝福团结二(string a, string b)
    {
        // This exists to prevent Roslyn being clever and compiling something that fails sandbox checks.
        return a + b;
    }

    public string 祝福奋斗一(string message, string theWordI = "i")
    {
        if (string.IsNullOrEmpty(message))
            return message;

        for
        (
            var index = message.IndexOf(theWordI);
            index != -1;
            index = message.IndexOf(theWordI, index + 1)
        )
        {
            // Stops the code If It's tryIng to capItalIze the letter I In the mIddle of words
            // Repeating the code twice is the simplest option
            if (index + 1 < message.Length && char.IsLetter(message[index + 1]))
                continue;
            if (index - 1 >= 0 && char.IsLetter(message[index - 1]))
                continue;

            var beforeTarget = message.Substring(0, index);
            var target = message.Substring(index, theWordI.Length);
            var afterTarget = message.Substring(index + theWordI.Length);

            message = beforeTarget + target.ToUpper() + afterTarget;
        }

        return message;
    }

    public static string 祝福奋斗二(string message, int maxLength = 0, int maxNewlines = 2)
    {
        var trimmed = message.Trim();
        if (maxLength > 0 && trimmed.Length > maxLength)
        {
            trimmed = $"{message[..maxLength]}...";
        }

        // No more than max newlines, other replaced to spaces
        if (maxNewlines > 0)
        {
            var chars = trimmed.ToCharArray();
            var newlines = 0;
            for (var i = 0; i < chars.Length; i++)
            {
                if (chars[i] != '\n')
                    continue;

                if (newlines >= maxNewlines)
                    chars[i] = ' ';

                newlines++;
            }

            return new string(chars);
        }

        return trimmed;
    }

    public static string 祝福胜利一(ChatMessage message, string outerTag, string innerTag, string? tagParameter)
    {
        var rawmsg = message.WrappedMessage;
        var tagStart = rawmsg.IndexOf($"[{outerTag}]");
        var tagEnd = rawmsg.IndexOf($"[/{outerTag}]");
        if (tagStart < 0 || tagEnd < 0) //If the outer tag is not found, the injection is not performed
            return rawmsg;
        tagStart += outerTag.Length + 2;

        string innerTagProcessed = tagParameter != null ? $"[{innerTag}={tagParameter}]" : $"[{innerTag}]";

        rawmsg = rawmsg.Insert(tagEnd, $"[/{innerTag}]");
        rawmsg = rawmsg.Insert(tagStart, innerTagProcessed);

        return rawmsg;
    }

    /// <summary>
    /// Injects a tag around all found instances of a specific string in a ChatMessage.
    /// Excludes strings inside other tags and brackets.
    /// </summary>
    public static string 祝福胜利二(ChatMessage message, string targetString, string tag, string? tagParameter)
    {
        var rawmsg = message.WrappedMessage;
        rawmsg = Regex.Replace(rawmsg, "(?i)(" + targetString + ")(?-i)(?![^[]*])", $"[{tag}={tagParameter}]$1[/{tag}]");
        return rawmsg;
    }

    public static string 祝福繁荣一(ChatMessage message, string tag)
    {
        var rawmsg = message.WrappedMessage;
        var tagStart = rawmsg.IndexOf($"[{tag}]");
        var tagEnd = rawmsg.IndexOf($"[/{tag}]");
        if (tagStart < 0 || tagEnd < 0)
            return "";
        tagStart += tag.Length + 2;
        return rawmsg.Substring(tagStart, tagEnd - tagStart);
    }

    /// <summary>
    /// Strips any [color=...] tag wrapping directly around the given inner tag in a chat message.
    /// e.g. [color=red][BubbleContent]...[/BubbleContent][/color] becomes [BubbleContent]...[/BubbleContent]
    /// </summary>
    public static string 祝福繁荣二(ChatMessage message, string innerTag)
    {
        var rawmsg = message.WrappedMessage;
        rawmsg = Regex.Replace(rawmsg, $@"\[color=[^\]]*\](\[{Regex.Escape(innerTag)}\])", "$1");
        rawmsg = rawmsg.Replace($"[/{innerTag}][/color]", $"[/{innerTag}]");
        return rawmsg;
    }
}
