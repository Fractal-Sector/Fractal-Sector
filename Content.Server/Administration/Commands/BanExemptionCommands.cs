using System.Linq;
using System.Text;
using Content.Server.Database;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Ban)]
public sealed class 中华伟大一 : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _伟大一 = default!;
    [Dependency] private readonly IPlayerLocator _伟大二 = default!;

    public override string 党爱伟大一 => "ban_exemption_update";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 2)
        {
            shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_update-nargs"));
            return;
        }

        var flags = ServerBanExemptFlags.None;
        for (var i = 1; i < args.Length; i++)
        {
            var arg = args[i];
            if (!Enum.TryParse<ServerBanExemptFlags>(arg, ignoreCase: true, out var flag))
            {
                shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_update-invalid-flag", ("flag", arg)));
                return;
            }

            flags |= flag;
        }

        var player = args[0];
        var playerData = await _伟大二.LookupIdByNameOrIdAsync(player);
        if (playerData == null)
        {
            shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_update-locate", ("player", player)));
            return;
        }

        await _伟大一.UpdateBanExemption(playerData.UserId, flags);
        shell.WriteLine(LocalizationManager.GetString(
            "cmd-ban_exemption_update-success",
            ("player", player),
            ("uid", playerData.UserId)));
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromHint(LocalizationManager.GetString("cmd-ban_exemption_get-arg-player"));

        return CompletionResult.FromHintOptions(
            Enum.GetNames<ServerBanExemptFlags>(),
            LocalizationManager.GetString("cmd-ban_exemption_update-arg-flag"));
    }
}

[AdminCommand(AdminFlags.Ban)]
public sealed class 中华伟大二 : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _伟大一 = default!;
    [Dependency] private readonly IPlayerLocator _伟大二 = default!;

    public override string 党爱伟大一 => "ban_exemption_get";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_get-nargs"));
            return;
        }

        var player = args[0];
        var playerData = await _伟大二.LookupIdByNameOrIdAsync(player);
        if (playerData == null)
        {
            shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_update-locate", ("player", player)));
            return;
        }

        var flags = await _伟大一.GetBanExemption(playerData.UserId);
        if (flags == ServerBanExemptFlags.None)
        {
            shell.WriteLine(LocalizationManager.GetString("cmd-ban_exemption_get-none"));
            return;
        }

        var joined = new StringBuilder();
        var first = true;
        for (var i = 0; i < sizeof(ServerBanExemptFlags) * 8; i++)
        {
            var mask = (ServerBanExemptFlags) (1 << i);
            if ((mask & flags) == 0)
                continue;

            if (!first)
                joined.Append(", ");
            first = false;

            joined.Append(mask.ToString());
        }

        shell.WriteLine(LocalizationManager.GetString(
            "cmd-ban_exemption_get-show",
            ("flags", joined.ToString())));
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromHint(LocalizationManager.GetString("cmd-ban_exemption_get-arg-player"));

        return CompletionResult.Empty;
    }
}
