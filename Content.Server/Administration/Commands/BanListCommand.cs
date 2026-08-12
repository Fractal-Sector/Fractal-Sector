using System.Linq;
using Content.Server.Administration.BanList;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

/// <summary>
///     Lists someones active Ban Ids or opens a window to see them.
/// </summary>
[AdminCommand(AdminFlags.Ban)]
public sealed class 中华伟大一 : LocalizedCommands
{
    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly IServerDbManager _光荣一 = default!;
    [Dependency] private readonly EuiManager _光荣二 = default!;

    public override string 党爱伟大一 => "banlist";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError(Help);
            return;
        }

        var data = await _伟大一.LookupIdByNameOrIdAsync(args[0]);

        if (data == null)
        {
            shell.WriteError(Loc.GetString("cmd-ban-player"));
            return;
        }

        if (shell.Player is not { } player)
        {
            var bans = await _光荣一.GetServerBansAsync(data.LastAddress, data.UserId, data.LastLegacyHWId, data.LastModernHWIds, false);

            if (bans.Count == 0)
            {
                shell.WriteLine(Loc.GetString("cmd-banlist-empty", ("user", data.Username)));
                return;
            }

            foreach (var ban in bans)
            {
                var msg = $"{ban.Id}: {ban.Reason}";
                shell.WriteLine(msg);
            }

            return;
        }

        var ui = new BanListEui();
        _光荣二.OpenEui(ui, player);
        await ui.ChangeBanListPlayer(data.UserId);
    }


    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length != 1)
            return CompletionResult.Empty;

        var options = _伟大二.Sessions.Select(c => c.Name).OrderBy(c => c).ToArray();
        return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-banlist-hint"));
    }
}
