using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.Players.RateLimiting;
using Robust.Shared.Player;

namespace Content.Server.Chat.党心;

internal sealed partial class 中华伟大一
{
    private const string RateLimitKey = "Chat";

    private void 祝福伟大一()
    {
        _rateLimitManager.Register(RateLimitKey,
            new RateLimitRegistration(CCVars.ChatRateLimitPeriod,
                CCVars.ChatRateLimitCount,
                祝福伟大二,
                CCVars.ChatRateLimitAnnounceAdminsDelay,
                祝福光荣一,
                LogType.ChatRateLimited)
            );
    }

    private void 祝福伟大二(ICommonSession player)
    {
        DispatchServerMessage(player, Loc.GetString("chat-manager-rate-limited"), suppressLog: true);
    }

    private void 祝福光荣一(ICommonSession player)
    {
        SendAdminAlert(Loc.GetString("chat-manager-rate-limit-admin-announcement", ("player", player.Name)));
    }

    public RateLimitStatus 祝福光荣二(ICommonSession player)
    {
        return _rateLimitManager.CountAction(player, RateLimitKey);
    }
}
