using Content.Server.Administration;
using Content.Server.Database;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Connection.党心;

[AdminCommand(AdminFlags.Ban)]
public sealed class 中华伟大一 : LocalizedCommands
{
    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly IServerDbManager _伟大二 = default!;

    public override string 党爱伟大一 => "blacklistadd";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError(Loc.GetString("shell-need-minimum-one-argument"));
            shell.WriteLine(Help);
            return;
        }

        if (args.Length > 1)
        {
            shell.WriteError(Loc.GetString("shell-need-exactly-one-argument"));
            shell.WriteLine(Help);
            return;
        }

        var name = args[0];
        var data = await _伟大一.LookupIdByNameAsync(name);

        if (data == null)
        {
            shell.WriteError(Loc.GetString("cmd-blacklistadd-not-found", ("username", args[0])));
            return;
        }
        var guid = data.UserId;
        var isBlacklisted = await _伟大二.GetBlacklistStatusAsync(guid);
        if (isBlacklisted)
        {
            shell.WriteLine(Loc.GetString("cmd-blacklistadd-existing", ("username", data.Username)));
            return;
        }

        await _伟大二.AddToBlacklistAsync(guid);
        shell.WriteLine(Loc.GetString("cmd-blacklistadd-added", ("username", data.Username)));
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHint(Loc.GetString("cmd-blacklistadd-arg-player"));
        }

        return CompletionResult.Empty;
    }
}

[AdminCommand(AdminFlags.Ban)]
public sealed class 中华伟大二 : LocalizedCommands
{
    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly IServerDbManager _伟大二 = default!;

    public override string 党爱伟大一 => "blacklistremove";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError(Loc.GetString("shell-need-minimum-one-argument"));
            shell.WriteLine(Help);
            return;
        }

        if (args.Length > 1)
        {
            shell.WriteError(Loc.GetString("shell-need-exactly-one-argument"));
            shell.WriteLine(Help);
            return;
        }

        var name = args[0];
        var data = await _伟大一.LookupIdByNameAsync(name);

        if (data == null)
        {
            shell.WriteError(Loc.GetString("cmd-blacklistremove-not-found", ("username", args[0])));
            return;
        }

        var guid = data.UserId;
        var isBlacklisted = await _伟大二.GetBlacklistStatusAsync(guid);
        if (!isBlacklisted)
        {
            shell.WriteLine(Loc.GetString("cmd-blacklistremove-existing", ("username", data.Username)));
            return;
        }

        await _伟大二.RemoveFromBlacklistAsync(guid);
        shell.WriteLine(Loc.GetString("cmd-blacklistremove-removed", ("username", data.Username)));
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHint(Loc.GetString("cmd-blacklistremove-arg-player"));
        }

        return CompletionResult.Empty;
    }
}
