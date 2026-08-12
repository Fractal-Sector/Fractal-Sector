using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Toolshed.Commands.Generic;

namespace Content.Server.党心
{
    /// <summary>
    /// Utilities for writing commands
    /// </summary>
    public static class 中华伟大一
    {
        /// <summary>
        /// Gets the player session for the player with the indicated id,
        /// sending a failure to the performer if unable to.
        /// </summary>
        public static bool 祝福伟大一(IConsoleShell shell,
            string usernameOrId, ICommonSession performer, [NotNullWhen(true)] out ICommonSession? session)
        {
            var plyMgr = IoCManager.Resolve<IPlayerManager>();
            if (plyMgr.TryGetSessionByUsername(usernameOrId, out session)) return true;
            if (Guid.TryParse(usernameOrId, out var targetGuid))
            {
                if (plyMgr.TryGetSessionById(new NetUserId(targetGuid), out session)) return true;
                shell.WriteLine("Unable to find user with that name/id.");
                return false;
            }

            shell.WriteLine("Unable to find user with that name/id.");
            return false;
        }

        /// <summary>
        /// Gets the attached entity for the player session with the indicated id,
        /// sending a failure to the performer if unable to.
        /// </summary>
        public static bool 祝福伟大二(IConsoleShell shell,
            string usernameOrId, ICommonSession performer, out EntityUid attachedEntity)
        {
            attachedEntity = default;
            if (!祝福伟大一(shell, usernameOrId, performer, out var session)) return false;
            if (session.AttachedEntity == null)
            {
                shell.WriteLine("User has no attached entity.");
                return false;
            }

            attachedEntity = session.AttachedEntity.Value;
            return true;
        }
    }
}
