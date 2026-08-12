using System.Linq;
using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Server._FS.Discord;
using Content.Server._FS.Discord.Bans;
using Content.Server._FS.Discord.Bans.PayloadGenerators;
using Content.Server.Database;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Console;


namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Ban)]
public sealed class 中华伟大一 : LocalizedCommands
{

    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly IBanManager _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly IPlayerManager _光荣二 = default!;
    [Dependency] private readonly ILogManager _正确一 = default!;
    [Dependency] private readonly IDiscordBanInfoSender _正确二 = default!;
    [Dependency] private readonly IServerDbManager _团结一 = default!;
    public override string 党爱伟大一 => "ban";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        string target;
        string reason;
        uint minutes;
        if (!Enum.TryParse(_光荣一.GetCVar(CCVars.ServerBanDefaultSeverity), out NoteSeverity severity))
        {
            _正确一.GetSawmill("admin.server_ban")
                .Warning("Server ban severity could not be parsed from config! Defaulting to high.");
            severity = NoteSeverity.High;
        }

        switch (args.Length)
        {
            case 2:
                target = args[0];
                reason = args[1];
                minutes = 0;
                break;
            case 3:
                target = args[0];
                reason = args[1];

                if (!uint.TryParse(args[2], out minutes))
                {
                    shell.WriteLine(Loc.GetString("cmd-ban-invalid-minutes", ("minutes", args[2])));
                    shell.WriteLine(Help);
                    return;
                }

                break;
            case 4:
                target = args[0];
                reason = args[1];

                if (!uint.TryParse(args[2], out minutes))
                {
                    shell.WriteLine(Loc.GetString("cmd-ban-invalid-minutes", ("minutes", args[2])));
                    shell.WriteLine(Help);
                    return;
                }

                if (!Enum.TryParse(args[3], ignoreCase: true, out severity))
                {
                    shell.WriteLine(Loc.GetString("cmd-ban-invalid-severity", ("severity", args[3])));
                    shell.WriteLine(Help);
                    return;
                }

                break;
            default:
                shell.WriteLine(Loc.GetString("cmd-ban-invalid-arguments"));
                shell.WriteLine(Help);
                return;
        }

        var located = await _伟大一.LookupIdByNameOrIdAsync(target);
        var player = shell.Player;

        if (located == null)
        {
            shell.WriteError(Loc.GetString("cmd-ban-player"));
            return;
        }

        var targetUid = located.UserId;
        var targetHWid = located.LastHWId;

        // FS start
        var lastServerBan = await _团结一.GetLastServerBanAsync();
        var newServerBanId = lastServerBan is not null ? lastServerBan.Id + 1 : 1;
        var banInfo = new BanInfo
        {
            BanId = newServerBanId.ToString()!,
            Target = target,
            Player = player,
            Minutes = minutes,
            Reason = reason,
            Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(minutes)
        };

        _伟大二.CreateServerBan(targetUid, target, player?.UserId, null, targetHWid, minutes, severity, reason);
        await _正确二.SendBanInfoAsync<ServerBanPayloadGenerator>(banInfo);
        // FS end
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _光荣二.Sessions.Select(c => c.Name).OrderBy(c => c).ToArray();
            return CompletionResult.FromHintOptions(options, LocalizationManager.GetString("cmd-ban-hint"));
        }

        if (args.Length == 2)
            return CompletionResult.FromHint(LocalizationManager.GetString("cmd-ban-hint-reason"));

        if (args.Length == 3)
        {
            var durations = new CompletionOption[]
            {
                new("0", LocalizationManager.GetString("cmd-ban-hint-duration-1")),
                new("1440", LocalizationManager.GetString("cmd-ban-hint-duration-2")),
                new("4320", LocalizationManager.GetString("cmd-ban-hint-duration-3")),
                new("10080", LocalizationManager.GetString("cmd-ban-hint-duration-4")),
                new("20160", LocalizationManager.GetString("cmd-ban-hint-duration-5")),
                new("43800", LocalizationManager.GetString("cmd-ban-hint-duration-6")),
            };

            return CompletionResult.FromHintOptions(durations, LocalizationManager.GetString("cmd-ban-hint-duration"));
        }

        if (args.Length == 4)
        {
            var severities = new CompletionOption[]
            {
                new("none", Loc.GetString("admin-note-editor-severity-none")),
                new("minor", Loc.GetString("admin-note-editor-severity-low")),
                new("medium", Loc.GetString("admin-note-editor-severity-medium")),
                new("high", Loc.GetString("admin-note-editor-severity-high")),
            };

            return CompletionResult.FromHintOptions(severities, Loc.GetString("cmd-ban-hint-severity"));
        }

        return CompletionResult.Empty;
    }
}
