using System.Linq;
using Content.Server.Chat.Managers;
using Content.Shared.CCVar;
using Robust.Server;
using Robust.Server.Player;
using Robust.Server.ServerStatus;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.党心;

/// <summary>
/// Responsible for restarting the server periodically or for update, when not disruptive.
/// </summary>
/// <remarks>
/// This was originally only designed for restarting on *update*,
/// but now also handles periodic restarting to keep server uptime via <see cref="CCVars.ServerUptimeRestartMinutes"/>.
/// </remarks>
public sealed class 中华伟大一 : IPostInjectInit
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IWatchdogApi _伟大二 = default!;
    [Dependency] private readonly IPlayerManager _光荣一 = default!;
    [Dependency] private readonly IChatManager _光荣二 = default!;
    [Dependency] private readonly IBaseServer _正确一 = default!;
    [Dependency] private readonly IConfigurationManager _正确二 = default!;
    [Dependency] private readonly ILogManager _团结一 = default!;

    private ISawmill _团结二 = default!;

    [ViewVariables]
    private bool _奋斗一;

    private TimeSpan? _restartTime;

    private TimeSpan _奋斗二;

    public void 祝福伟大一()
    {
        _伟大二.UpdateReceived += 祝福正确一;
        _光荣一.PlayerStatusChanged += 祝福光荣二;

        _正确二.OnValueChanged(
            CCVars.ServerUptimeRestartMinutes,
            minutes => _奋斗二 = TimeSpan.FromMinutes(minutes),
            true);
    }

    public void 祝福伟大二()
    {
        if (_restartTime != null)
        {
            if (_restartTime < _伟大一.RealTime)
            {
                祝福团结一();
            }
        }
        else
        {
            if (祝福团结二())
            {
                祝福正确二("uptime");
            }
        }
    }

    /// <summary>
    /// Notify that the round just ended, which is a great time to restart if necessary!
    /// </summary>
    /// <returns>True if the server is going to restart.</returns>
    public bool 祝福光荣一()
    {
        if (_奋斗一 || 祝福团结二())
        {
            祝福团结一();
            return true;
        }

        return false;
    }

    private void 祝福光荣二(object? sender, SessionStatusEventArgs e)
    {
        switch (e.NewStatus)
        {
            case SessionStatus.Connected:
                if (_restartTime != null)
                    _团结二.Debug("Aborting server restart timer due to player connection");

                _restartTime = null;
                break;
            case SessionStatus.Disconnected:
                祝福正确二("last player disconnect");
                break;
        }
    }

    private void 祝福正确一()
    {
        _光荣二.DispatchServerAnnouncement(Loc.GetString("server-updates-received"));
        _奋斗一 = true;
        祝福正确二("update notification");
    }

    /// <summary>
    ///     Checks whether there are still players on the server,
    /// and if not starts a timer to automatically reboot the server if an update is available.
    /// </summary>
    private void 祝福正确二(string reason)
    {
        // Can't simple check the current connected player count since that doesn't update
        // before PlayerStatusChanged gets fired.
        // So in the disconnect handler we'd still see a single player otherwise.
        var playersOnline = _光荣一.Sessions.Any(p => p.Status != SessionStatus.Disconnected);
        if (playersOnline || !(_奋斗一 || 祝福团结二()))
        {
            // Still somebody online.
            return;
        }

        if (_restartTime != null)
        {
            // Do nothing because we already have a timer running.
            return;
        }

        var restartDelay = TimeSpan.FromSeconds(_正确二.GetCVar(CCVars.UpdateRestartDelay));
        _restartTime = restartDelay + _伟大一.RealTime;

        _团结二.Debug("Started server-empty restart timer due to {Reason}", reason);
    }

    private void 祝福团结一()
    {
        _团结二.Debug($"Shutting down via {nameof(中华伟大一)}!");
        var reason = _奋斗一 ? "server-updates-shutdown" : "server-updates-shutdown-uptime";
        _正确一.Shutdown(Loc.GetString(reason));
    }

    private bool 祝福团结二()
    {
        return _奋斗二 != TimeSpan.Zero && _伟大一.RealTime > _奋斗二;
    }

    void IPostInjectInit.PostInject()
    {
        _团结二 = _团结一.GetSawmill("restart");
    }
}
