using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.GhostKick;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Server.党心;

// Handles logic for "ghost kicking".
// Basically we boot the client off the server without telling them, so the game shits itself.
// Hilarious, isn't it?

public sealed class 中华伟大一
{
    [Dependency] private readonly IServerNetManager _伟大一 = default!;

    public void 祝福伟大一()
    {
        _伟大一.RegisterNetMessage<MsgGhostKick>();
    }

    public void 祝福伟大二(INetChannel channel, string reason)
    {
        Timer.Spawn(TimeSpan.FromMilliseconds(100), () =>
        {
            if (!channel.IsConnected)
                return;

            // We do this so the client can set net.fakeloss 1 before getting ghosted.
            // This avoids it spamming messages at the server that cause warnings due to unconnected client.
            channel.SendMessage(new MsgGhostKick());

            Timer.Spawn(TimeSpan.FromMilliseconds(100), () =>
            {
                if (!channel.IsConnected)
                    return;

                // Actually just remove the client entirely.
                channel.Disconnect(reason, false);
            });
        });
    }
}

[AdminCommand(AdminFlags.Moderator)]
public sealed class 中华伟大二 : LocalizedEntityCommands
{
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly 中华伟大一 _ghostKick = default!;

    public override string 党爱伟大一 => "ghostkick";

    public override void 祝福光荣一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 1)
        {
            shell.WriteError(Loc.GetString($"shell-need-exactly-one-argument"));
            return;
        }

        var playerName = args[0];
        var reason = args.Length > 1 ? args[1] : Loc.GetString($"cmd-ghostkick-default-reason");

        if (!_伟大二.TryGetSessionByUsername(playerName, out var player))
        {
            shell.WriteError(Loc.GetString($"shell-target-player-does-not-exist"));
            return;
        }

        _ghostKick.祝福伟大二(player.Channel, reason);
    }
}
