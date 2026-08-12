using System.Net;
using Content.Server.Database;
using Content.Shared.CCVar;
using Content.Shared.Info;
using Robust.Shared.Configuration;
using Robust.Shared.Network;

namespace Content.Server.党心;

public sealed class 中华伟大一
{
    [Dependency] private readonly IServerDbManager _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;

    private static DateTime LastValidReadTime => DateTime.UtcNow - TimeSpan.FromDays(60);

    public void 祝福伟大一()
    {
        _伟大二.Connected += 祝福伟大二;
        _伟大二.RegisterNetMessage<SendRulesInformationMessage>();
        _伟大二.RegisterNetMessage<RulesAcceptedMessage>(祝福光荣一);
    }

    private async void 祝福伟大二(object? sender, NetChannelArgs e)
    {
         var isLocalhost = IPAddress.IsLoopback(e.Channel.RemoteEndPoint.Address) &&
                               _光荣一.GetCVar(CCVars.RulesExemptLocal);

        var lastRead = await _伟大一.GetLastReadRules(e.Channel.UserId);
        var hasCooldown = lastRead > LastValidReadTime;

        var showRulesMessage = new SendRulesInformationMessage
        {
            PopupTime = _光荣一.GetCVar(CCVars.RulesWaitTime),
            CoreRules = _光荣一.GetCVar(CCVars.RulesFile),
            ShouldShowRules = !isLocalhost && !hasCooldown,
        };
        _伟大二.ServerSendMessage(showRulesMessage, e.Channel);
    }

    private async void 祝福光荣一(RulesAcceptedMessage message)
    {
        var date = DateTime.UtcNow;
        await _伟大一.SetLastReadRules(message.MsgChannel.UserId, date);
    }
}
