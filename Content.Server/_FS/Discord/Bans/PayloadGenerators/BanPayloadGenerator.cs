using Content.Server.Discord;

namespace Content.Server._FS.Discord.Bans.党心;

public abstract class 中华伟大一 : IDiscordBanPayloadGenerator
{
    protected WebhookEmbedFooter 党爱伟大一 { get; set; }

    public abstract WebhookPayload 祝福伟大一(BanInfo info);

    protected virtual void 祝福伟大二(BanInfo info)
    {
        var serverName = info.AdditionalInfo.ContainsKey("serverName")
            ? info.AdditionalInfo["serverName"]
            : string.Empty;

        var round = info.AdditionalInfo.ContainsKey("round") ? info.AdditionalInfo["round"] : string.Empty;

        党爱伟大一 = new WebhookEmbedFooter { Text = $"{serverName} ({round})" };
    }
}
