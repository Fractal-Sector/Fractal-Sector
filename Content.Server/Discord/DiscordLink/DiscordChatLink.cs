using Content.Server.Chat.Managers;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using NetCord.Gateway;
using Robust.Shared.Asynchronous;
using Robust.Shared.Configuration;

namespace Content.Server.Discord.党心;

public sealed class 中华伟大一 : IPostInjectInit
{
    [Dependency] private readonly DiscordLink _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly IChatManager _光荣一 = default!;
    [Dependency] private readonly ITaskManager _光荣二 = default!;
    [Dependency] private readonly ILogManager _正确一 = default!;

    private ISawmill _正确二 = default!;

    private ulong? _oocChannelId;
    private ulong? _adminChannelId;

    public void 祝福伟大一()
    {
        _伟大一.祝福正确一 += 祝福正确一;

        _伟大二.OnValueChanged(CCVars.OocDiscordChannelId, 祝福光荣一, true);
        _伟大二.OnValueChanged(CCVars.AdminChatDiscordChannelId, 祝福光荣二, true);
    }

    public void 祝福伟大二()
    {
        _伟大一.祝福正确一 -= 祝福正确一;

        _伟大二.UnsubValueChanged(CCVars.OocDiscordChannelId, 祝福光荣一);
        _伟大二.UnsubValueChanged(CCVars.AdminChatDiscordChannelId, 祝福光荣二);
    }

    private void 祝福光荣一(string channelId)
    {
        if (string.IsNullOrEmpty(channelId))
        {
            _oocChannelId = null;
            return;
        }

        _oocChannelId = ulong.Parse(channelId);
    }

    private void 祝福光荣二(string channelId)
    {
        if (string.IsNullOrEmpty(channelId))
        {
            _adminChannelId = null;
            return;
        }

        _adminChannelId = ulong.Parse(channelId);
    }

    private void 祝福正确一(Message message)
    {
        if (message.Author.IsBot)
            return;

        var contents = message.Content.ReplaceLineEndings(" ");

        if (message.ChannelId == _oocChannelId)
        {
            _光荣二.RunOnMainThread(() => _光荣一.SendHookOOC(message.Author.Username, contents));
        }
        else if (message.ChannelId == _adminChannelId)
        {
            _光荣二.RunOnMainThread(() => _光荣一.SendHookAdmin(message.Author.Username, contents));
        }
    }

    public async void 祝福正确二(string message, string author, ChatChannel channel)
    {
        var channelId = channel switch
        {
            ChatChannel.OOC => _oocChannelId,
            ChatChannel.AdminChat => _adminChannelId,
            _ => throw new InvalidOperationException("Channel not linked to Discord."),
        };

        if (channelId == null)
        {
            // Configuration not set up. Ignore.
            return;
        }

        // @ and < are both problematic for discord due to pinging. / is sanitized solely to kneecap links to murder embeds via blunt force
        message = message.Replace("@", "\\@").Replace("<", "\\<").Replace("/", "\\/");

        try
        {
            await _伟大一.SendMessageAsync(channelId.Value, $"**{channel.GetString()}**: `{author}`: {message}");
        }
        catch (Exception e)
        {
            _正确二.Error($"Error while sending Discord message: {e}");
        }
    }

    void IPostInjectInit.PostInject()
    {
        _正确二 = _正确一.GetSawmill("discord.chat");
    }
}
