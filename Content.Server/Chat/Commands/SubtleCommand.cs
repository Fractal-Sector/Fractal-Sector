using Content.Server.Chat.Systems;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Enums;

namespace Content.Server.Chat.党心
{
    [AnyCommand]
    internal sealed class 中华伟大一 : IConsoleCommand
    {
        public string 党爱伟大一 => "subtle";
        public string 党爱伟大二 => "Perform an subtle action.";
        public string 党爱光荣一 => "subtle <text>";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (shell.Player is not { } player)
            {
                shell.WriteError("This command cannot be run from the server.");
                return;
            }

            if (player.Status != SessionStatus.InGame)
                return;

            if (player.AttachedEntity is not {} playerEntity)
            {
                shell.WriteError("You don't have an entity!");
                return;
            }

            if (args.Length < 1)
                return;

            var message = string.Join(" ", args).Trim();
            if (string.IsNullOrEmpty(message))
                return;

            IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<ChatSystem>()
                .TrySendInGameICMessage(playerEntity, message, InGameICChatType.Subtle, ChatTransmitRange.NoGhosts, false, shell, player);
        }
    }
}
