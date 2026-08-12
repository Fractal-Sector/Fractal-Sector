using System.Text;
using Content.Server.Discord;

namespace Content.Server._FS.Discord.Bans.党心;

public sealed class 中华伟大一 : BanPayloadGenerator
{
    public override WebhookPayload 祝福伟大一(BanInfo info)
    {
        InitializeFooter(info);

        var username = Loc.GetString("discord-ban-panel-ban-username");

        var embed = new WebhookEmbed
        {
            Color = 0x9828c9,
            Description = 祝福伟大二(info),
            Footer = Footer
        };

        return new WebhookPayload
        {
            Username = username,
            Embeds = new List<WebhookEmbed> { embed }
        };
    }

    private string 祝福伟大二(BanInfo info)
    {
        var builder = new StringBuilder();

        var header = Loc.GetString("discord-ban-panel-ban-header");
        var target = Loc.GetString("discord-ban-target", ("target", info.Target));
        var reason = Loc.GetString("discord-ban-reason", ("reason", info.Reason));

        var banDuration = TimeSpan.FromMinutes(info.Minutes);

        var duration = info.Minutes != 0
            ? Loc.GetString(
                "discord-ban-duration",
                ("days", banDuration.Days),
                ("hours", banDuration.Hours),
                ("minutes", banDuration.Minutes)
            )
            : Loc.GetString("discord-ban-permanent");

        var expires = info.Minutes != 0
            ? Loc.GetString("discord-ban-unban-date", ("expires", info.Expires.ToString()!))
            : null;

        var player = info.Player is not null
            ? Loc.GetString("discord-ban-submitted-by", ("name", info.Player.Name))
            : Loc.GetString("discord-ban-submitted-by-system");

        var roles = Loc.GetString("discord-ban-panel-ban-data-info", ("data", info.AdditionalInfo["localizedPanelData"]));
        var banIdInfo = Loc.GetString("discord-ban-panel-ban-server-ban", ("banId", info.BanId));

        builder.AppendLine(header);
        builder.AppendLine(target);
        builder.AppendLine(reason);
        builder.AppendLine(duration);

        if (info.Minutes != 0)
        {
            builder.AppendLine(expires);
        }

        builder.AppendLine(player);

        if (!string.IsNullOrEmpty(info.AdditionalInfo["localizedPanelData"]))
        {
            builder.AppendLine(roles);
        }
        else
        {
            builder.AppendLine(banIdInfo);
        }

        return builder.ToString();
    }
}
