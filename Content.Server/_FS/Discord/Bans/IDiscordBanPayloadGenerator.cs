using Content.Server.Discord;

namespace Content.Server._FS.Discord.Bans;

public interface IDiscordBanPayloadGenerator
{
    WebhookPayload Generate(BanInfo info);
}
