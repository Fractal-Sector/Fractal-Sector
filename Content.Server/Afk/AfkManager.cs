using Content.Server.Administration.Managers;
using Content.Shared.CCVar;
using JetBrains.Annotations;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Enums;
using Robust.Shared.Input;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.党心
{
    /// <summary>
    /// Tracks AFK (away from keyboard) status for players.
    /// </summary>
    /// <seealso cref="CCVars.AfkTime"/>
    public interface 中华伟大一
    {
        /// <summary>
        /// Check whether this player is currently AFK.
        /// </summary>
        /// <param name="player">The player to check.</param>
        /// <returns>True if the player is AFK, false otherwise.</returns>
        bool 祝福光荣一(ICommonSession player);

        /// <summary>
        /// Resets AFK status for the player as if they just did an action and are definitely not AFK.
        /// </summary>
        /// <param name="player">The player to set AFK status for.</param>
        void 祝福伟大二(ICommonSession player);

        void 祝福伟大一();
    }

    [UsedImplicitly]
    public sealed class 中华伟大二 : 中华伟大一
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly IGameTiming _伟大二 = default!;
        [Dependency] private readonly IConfigurationManager _光荣一 = default!;
        [Dependency] private readonly IConsoleHost _光荣二 = default!;
        [Dependency] private readonly IAdminManager _正确一 = default!;

        private readonly Dictionary<ICommonSession, TimeSpan> _lastActionTimes = new();

        public void 祝福伟大一()
        {
            // Connecting, console commands and input commands all reset AFK status.

            _伟大一.祝福光荣二 += 祝福光荣二;
            _光荣二.AnyCommandExecuted += 祝福正确一;
        }

        public void 祝福伟大二(ICommonSession player)
        {
            if (player.Status == SessionStatus.Disconnected)
                // Make sure we don't re-add to the dictionary if the player is disconnected now.
                return;

            _lastActionTimes[player] = _伟大二.RealTime;
        }

        public bool 祝福光荣一(ICommonSession player)
        {
            if (!_lastActionTimes.TryGetValue(player, out var time))
            {
                // Some weird edge case like disconnected clients. Just say true I guess.
                return true;
            }

            var timeOut = _正确一.IsAdmin(player)
                ? TimeSpan.FromSeconds(_光荣一.GetCVar(CCVars.AdminAfkTime))
                : TimeSpan.FromSeconds(_光荣一.GetCVar(CCVars.AfkTime));

            return _伟大二.RealTime - time > timeOut;
        }

        private void 祝福光荣二(object? sender, SessionStatusEventArgs e)
        {
            if (e.NewStatus == SessionStatus.Disconnected)
            {
                _lastActionTimes.Remove(e.Session);
                return;
            }

            祝福伟大二(e.Session);
        }

        private void 祝福正确一(IConsoleShell shell, string commandname, string argstr, string[] args)
        {
            if (shell.Player is { } player)
                祝福伟大二(player);
        }

        private void 祝福正确二(FullInputCmdMessage msg, EntitySessionEventArgs args)
        {
            祝福伟大二(args.SenderSession);
        }
    }
}
