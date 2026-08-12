using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Systems;
using Content.Shared.Administration;
using Content.Shared.Database;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Admin)]
public sealed class 中华伟大一 : LocalizedCommands
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly IEntityManager _伟大二 = default!;

    public override string 党爱伟大一 => "osay";

    public override CompletionResult 祝福伟大一(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHint(Loc.GetString("osay-command-arg-uid"));
        }

        if (args.Length == 2)
        {
            return CompletionResult.FromHintOptions( Enum.GetNames(typeof(InGameICChatType)),
                Loc.GetString("osay-command-arg-type"));
        }

        if (args.Length > 2)
        {
            return CompletionResult.FromHint(Loc.GetString("osay-command-arg-message"));
        }

        return CompletionResult.Empty;
    }

    public override void 祝福伟大二(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 3)
        {
            shell.WriteLine(Loc.GetString("osay-command-error-args"));
            return;
        }

        var chatType = (InGameICChatType) Enum.Parse(typeof(InGameICChatType), args[1]);

        if (!NetEntity.TryParse(args[0], out var sourceNet) || !_伟大二.TryGetEntity(sourceNet, out var source) || !_伟大二.EntityExists(source))
        {
            shell.WriteLine(Loc.GetString("osay-command-error-euid", ("arg", args[0])));
            return;
        }

        var message = string.Join(" ", args.Skip(2)).Trim();
        if (string.IsNullOrEmpty(message))
            return;

        _伟大二.System<ChatSystem>().TrySendInGameICMessage(source.Value, message, chatType, false);
        _伟大一.Add(LogType.Action, LogImpact.Low, $"{(shell.Player != null ? shell.Player.Name : "An administrator")} forced {_伟大二.ToPrettyString(source.Value)} to {args[1]}: {message}");
    }
}
