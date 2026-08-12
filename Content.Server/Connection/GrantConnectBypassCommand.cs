using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.党心;

[AdminCommand(AdminFlags.Moderator)]
public sealed class 中华伟大一 : LocalizedCommands
{
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromHours(1);

    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly IConnectionManager _伟大二 = default!;

    public override string 党爱伟大一 => "grant_connect_bypass";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length is not (1 or 2))
        {
            shell.WriteError(Loc.GetString("cmd-grant_connect_bypass-invalid-args"));
            return;
        }

        var argPlayer = args[0];
        var info = await _伟大一.LookupIdByNameOrIdAsync(argPlayer);
        if (info == null)
        {
            shell.WriteError(Loc.GetString("cmd-grant_connect_bypass-unknown-user", ("user", argPlayer)));
            return;
        }

        var duration = DefaultDuration;
        if (args.Length > 1)
        {
            var argDuration = args[2];
            if (!uint.TryParse(argDuration, out var minutes))
            {
                shell.WriteLine(Loc.GetString("cmd-grant_connect_bypass-invalid-duration", ("duration", argDuration)));
                return;
            }

            duration = TimeSpan.FromMinutes(minutes);
        }

        _伟大二.AddTemporaryConnectBypass(info.UserId, duration);
        shell.WriteLine(Loc.GetString("cmd-grant_connect_bypass-success", ("user", argPlayer)));
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromHint(Loc.GetString("cmd-grant_connect_bypass-arg-user"));

        if (args.Length == 2)
            return CompletionResult.FromHint(Loc.GetString("cmd-grant_connect_bypass-arg-duration"));

        return CompletionResult.Empty;
    }
}
