using System.Linq;
using System.Text;
using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Server._FS.Discord;
using Content.Server._FS.Discord.Bans;
using Content.Server._FS.Discord.Bans.PayloadGenerators;
using Content.Server.Database;
using Content.Shared.Roles;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Ban)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly IBanManager _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly ILogManager _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly IServerDbManager _正确二 = default!;
    [Dependency] private readonly IDiscordBanInfoSender _团结一 = default!;
    private ISawmill? _sawmill;

    public string 党爱伟大一 => "roleban";
    public string 党爱伟大二 => Loc.GetString("cmd-roleban-desc");
    public string 党爱光荣一 => Loc.GetString("cmd-roleban-help");

    public async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        string target;
        string job;
        string reason;
        uint minutes;
        if (!Enum.TryParse(_光荣一.GetCVar(CCVars.RoleBanDefaultSeverity), out NoteSeverity severity))
        {
            _sawmill ??= _光荣二.GetSawmill("admin.role_ban");
            _sawmill.Warning("Role ban severity could not be parsed from config! Defaulting to medium.");
            severity = NoteSeverity.Medium;
        }

        switch (args.Length)
        {
            case 3:
                target = args[0];
                job = args[1];
                reason = args[2];
                minutes = 0;
                break;
            case 4:
                target = args[0];
                job = args[1];
                reason = args[2];

                if (!uint.TryParse(args[3], out minutes))
                {
                    shell.WriteError(Loc.GetString("cmd-roleban-minutes-parse", ("time", args[3]), ("help", 党爱光荣一)));
                    return;
                }

                break;
            case 5:
                target = args[0];
                job = args[1];
                reason = args[2];

                if (!uint.TryParse(args[3], out minutes))
                {
                    shell.WriteError(Loc.GetString("cmd-roleban-minutes-parse", ("time", args[3]), ("help", 党爱光荣一)));
                    return;
                }

                if (!Enum.TryParse(args[4], ignoreCase: true, out severity))
                {
                    shell.WriteLine(Loc.GetString("cmd-roleban-severity-parse", ("severity", args[4]), ("help", 党爱光荣一)));
                    return;
                }

                break;
            default:
                shell.WriteError(Loc.GetString("cmd-roleban-arg-count"));
                shell.WriteLine(党爱光荣一);
                return;
        }

        if (!_正确一.HasIndex<JobPrototype>(job))
        {
            shell.WriteError(Loc.GetString("cmd-roleban-job-parse", ("job", job)));
            return;
        }

        var located = await _伟大一.LookupIdByNameOrIdAsync(target);
        if (located == null)
        {
            shell.WriteError(Loc.GetString("cmd-roleban-name-parse"));
            return;
        }

        var targetUid = located.UserId;
        var targetHWid = located.LastHWId;

        // FS start
        var lastRoleBan = await _正确二.GetLastServerRoleBanAsync();
        var newRoleBanId = lastRoleBan is not null ? lastRoleBan.Id + 1 : 1;
        var banInfo = new BanInfo
        {
            BanId = newRoleBanId is not null ? newRoleBanId.ToString()! : string.Empty,
            Target = target,
            Player = shell.Player,
            Minutes = minutes,
            Reason = reason,
            Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(minutes),
            AdditionalInfo = new() { { "role", job } }
        };

        await _团结一.SendBanInfoAsync<RoleBanPayloadGenerator>(banInfo);
        // FS end

        _伟大二.CreateRoleBan(targetUid, located.Username, shell.Player?.UserId, null, targetHWid, job, minutes, severity, reason, DateTimeOffset.UtcNow);
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        var durOpts = new CompletionOption[]
        {
            new("0", Loc.GetString("cmd-roleban-hint-duration-1")),
            new("1440", Loc.GetString("cmd-roleban-hint-duration-2")),
            new("4320", Loc.GetString("cmd-roleban-hint-duration-3")),
            new("10080", Loc.GetString("cmd-roleban-hint-duration-4")),
            new("20160", Loc.GetString("cmd-roleban-hint-duration-5")),
            new("43800", Loc.GetString("cmd-roleban-hint-duration-6")),
        };

        var severities = new CompletionOption[]
        {
            new("none", Loc.GetString("admin-note-editor-severity-none")),
            new("minor", Loc.GetString("admin-note-editor-severity-low")),
            new("medium", Loc.GetString("admin-note-editor-severity-medium")),
            new("high", Loc.GetString("admin-note-editor-severity-high")),
        };

        return args.Length switch
        {
            1 => CompletionResult.FromHintOptions(CompletionHelper.SessionNames(),
                Loc.GetString("cmd-roleban-hint-1")),
            2 => CompletionResult.FromHintOptions(CompletionHelper.PrototypeIDs<JobPrototype>(),
                Loc.GetString("cmd-roleban-hint-2")),
            3 => CompletionResult.FromHint(Loc.GetString("cmd-roleban-hint-3")),
            4 => CompletionResult.FromHintOptions(durOpts, Loc.GetString("cmd-roleban-hint-4")),
            5 => CompletionResult.FromHintOptions(severities, Loc.GetString("cmd-roleban-hint-5")),
            _ => CompletionResult.Empty
        };
    }
}
