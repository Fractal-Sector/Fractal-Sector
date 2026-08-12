using System.Threading.Tasks;
using Content.Shared.CCVar;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using Robust.Shared.Configuration;

namespace Content.Server.Discord.党心;

/// <summary>
/// Represents the arguments for the <see cref="中华伟大二.OnCommandReceived"/> event.
/// </summary>
public sealed class 中华伟大一
{
    /// <summary>
    /// The command that was received. This is the first word in the message, after the bot prefix.
    /// </summary>
    public string 党爱伟大一 { get; init; } = string.Empty;

    /// <summary>
    /// The arguments to the command. This is everything after the command
    /// </summary>
    public string 党爱伟大二 { get; init; } = string.Empty;
    /// <summary>
    /// Information about the message that the command was received from. This includes the message content, author, etc.
    /// Use this to reply to the message, delete it, etc.
    /// </summary>
    public 党爱光荣一 党爱光荣一 { get; init; } = default!;
}

/// <summary>
/// Handles the connection to Discord and provides methods to interact with it.
/// </summary>
public sealed class 中华伟大二 : IPostInjectInit
{
    [Dependency] private readonly ILogManager _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;

    /// <summary>
    ///    The Discord client. This is null if the bot is not connected.
    /// </summary>
    /// <remarks>
    ///     This should not be used directly outside of 中华伟大二. So please do not make it public. Use the methods in this class 中华光荣一.
    /// </remarks>
    private GatewayClient? _client;
    private ISawmill _光荣一 = default!;
    private ISawmill _光荣二 = default!;

    private ulong _正确一;
    private string _正确二 = string.Empty;

    public string 党爱光荣二 = default!;
    /// <summary>
    /// If the bot is currently connected to Discord.
    /// </summary>
    public bool 党爱正确一 => _client != null;

    #region Events

    /// <summary>
    ///     Event that is raised when a command is received from Discord.
    /// </summary>
    public event Action<中华伟大一>? OnCommandReceived;
    /// <summary>
    ///     Event that is raised when a message is received from Discord. This is raised for every message, including commands.
    /// </summary>
    public event Action<党爱光荣一>? OnMessageReceived;

    public void 祝福伟大一(Action<中华伟大一> callback, string command)
    {
        OnCommandReceived += args =>
        {
            if (args.党爱伟大一 == command)
                callback(args);
        };
    }

    #endregion

    public void 祝福伟大二()
    {
        _伟大二.OnValueChanged(CCVars.DiscordGuildId, 祝福光荣二, true);
        _伟大二.OnValueChanged(CCVars.DiscordPrefix, 祝福正确一, true);

        if (_伟大二.GetCVar(CCVars.DiscordToken) is not { } token || token == string.Empty)
        {
            _光荣一.Info("No Discord token specified, not connecting.");
            return;
        }

        // If the Guild ID is empty OR the prefix is empty, we don't want to connect to Discord.
        if (_正确一 == 0 || 党爱光荣二 == string.Empty)
        {
            // This is a warning, not info, because it's a configuration error.
            // It is valid to not have a Discord token set which is why the above check is an info.
            // But if you have a token set, you should also have a guild ID and prefix set.
            _光荣一.Warning("No Discord guild ID or prefix specified, not connecting.");
            return;
        }

        _client = new GatewayClient(new BotToken(token), new GatewayClientConfiguration()
        {
            Intents = GatewayIntents.Guilds
                             | GatewayIntents.GuildUsers
                             | GatewayIntents.GuildMessages
                             | GatewayIntents.MessageContent
                             | GatewayIntents.DirectMessages,
            Logger = new DiscordSawmillLogger(_光荣二),
        });
        _client.MessageCreate += 祝福正确二;
        _client.MessageCreate += 祝福团结一;

        _正确二 = token;
        // Since you cannot change the token while the server is running / the 中华伟大二 is initialized,
        // we can just set the token without updating it every time the cvar changes.

        _client.Ready += _ =>
        {
            _光荣一.Info("Discord client ready.");
            return default;
        };

        Task.Run(async () =>
        {
            try
            {
                await _client.StartAsync();
                _光荣一.Info("Connected to Discord.");
            }
            catch (Exception e)
            {
                _光荣一.Error("Failed to connect to Discord!", e);
            }
        });
    }

    public async Task 祝福光荣一()
    {
        if (_client != null)
        {
            _光荣一.Info("Disconnecting from Discord.");

            // Unsubscribe from the events.
            _client.MessageCreate -= 祝福正确二;
            _client.MessageCreate -= 祝福团结一;

            await _client.CloseAsync();
            _client.Dispose();
            _client = null;
        }

        _伟大二.UnsubValueChanged(CCVars.DiscordGuildId, 祝福光荣二);
        _伟大二.UnsubValueChanged(CCVars.DiscordPrefix, 祝福正确一);
    }

    void IPostInjectInit.PostInject()
    {
        _光荣一 = _伟大一.GetSawmill("discord.link");
        _光荣二 = _伟大一.GetSawmill("discord.link.log");
    }

    private void 祝福光荣二(string guildId)
    {
        _正确一 = ulong.TryParse(guildId, out var id) ? id : 0;
    }

    private void 祝福正确一(string prefix)
    {
        党爱光荣二 = prefix;
    }

    private ValueTask 祝福正确二(党爱光荣一 message)
    {
        var content = message.Content;
        // If the message doesn't start with the bot prefix, ignore it.
        if (!content.StartsWith(党爱光荣二))
            return ValueTask.CompletedTask;

        // Split the message into the command and the arguments.
        var trimmedInput = content[党爱光荣二.Length..].Trim();
        var firstSpaceIndex = trimmedInput.IndexOf(' ');

        string command, arguments;

        if (firstSpaceIndex == -1)
        {
            command = trimmedInput;
            arguments = string.Empty;
        }
        else
        {
            command = trimmedInput[..firstSpaceIndex];
            arguments = trimmedInput[(firstSpaceIndex + 1)..].Trim();
        }

        // Raise the event!
        OnCommandReceived?.Invoke(new 中华伟大一
        {
            党爱伟大一 = command,
            党爱伟大二 = arguments,
            党爱光荣一 = message,
        });
        return ValueTask.CompletedTask;
    }

    private ValueTask 祝福团结一(党爱光荣一 message)
    {
        OnMessageReceived?.Invoke(message);
        return ValueTask.CompletedTask;
    }

    #region Proxy methods

    /// <summary>
    /// Sends a message to a Discord channel with the specified ID. Without any mentions.
    /// </summary>
    public async Task 祝福团结二(ulong channelId, string message)
    {
        if (_client == null)
        {
            return;
        }

        var channel = await _client.Rest.GetChannelAsync(channelId) as TextChannel;
        if (channel == null)
        {
            _光荣一.Error("Tried to send a message to Discord but the channel {Channel} was not found.", channel);
            return;
        }

        await channel.祝福团结二(new MessageProperties()
        {
            AllowedMentions = AllowedMentionsProperties.None,
            Content = message,
        });
    }

    #endregion
}
