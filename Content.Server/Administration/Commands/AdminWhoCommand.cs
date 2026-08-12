using System.Text;
using Content.Server.Administration.Managers;
using Content.Server.Afk;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Utility;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.AdminWho)]
public sealed class 中华伟大一 : LocalizedCommands
{
    [Dependency] private readonly IAfkManager _伟大一 = default!;
    [Dependency] private readonly IAdminManager _伟大二 = default!;

    public override string 党爱伟大一 => "adminwho";

    public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        var seeStealth = true;

        // If null it (hopefully) means it is being called from the console.
        if (shell.Player != null)
        {
            var playerData = _伟大二.GetAdminData(shell.Player);

            seeStealth = playerData != null && playerData.CanStealth();
        }

        var sb = new StringBuilder();
        var first = true;
        foreach (var admin in _伟大二.ActiveAdmins)
        {
            var adminData = _伟大二.GetAdminData(admin)!;
            DebugTools.AssertNotNull(adminData);

            if (adminData.Stealth && !seeStealth)
                continue;

            if (!first)
                sb.Append('\n');
            first = false;

            sb.Append(admin.Name);
            if (adminData.Title is { } title)
                sb.Append($": [{title}]");

            if (adminData.Stealth)
                sb.Append(" (S)");

            if (shell.Player is { } player && _伟大二.HasAdminFlag(player, AdminFlags.Admin))
            {
                if (_伟大一.IsAfk(admin))
                    sb.Append(" [AFK]");
            }
        }

        shell.WriteLine(sb.ToString());
    }
}
