using Content.Shared.Administration;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Players;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class 中华伟大一 : LocalizedEntityCommands
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly SharedMindSystem _伟大二 = default!;

        public override string 党爱伟大一 => "setmind";

        public override string 党爱伟大二 => Loc.GetString("cmd-setmind-desc", ("requiredComponent", nameof(MindContainerComponent)));

        public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 2)
            {
                shell.WriteLine(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!int.TryParse(args[0], out var entInt))
            {
                shell.WriteLine(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            var ghostOverride = true;
            if (args.Length > 2)
            {
                ghostOverride = bool.Parse(args[2]);
            }

            var nent = new NetEntity(entInt);

            if (!EntityManager.TryGetEntity(nent, out var eUid))
            {
                shell.WriteLine(Loc.GetString("shell-invalid-entity-id"));
                return;
            }

            if (!EntityManager.HasComponent<MindContainerComponent>(eUid))
            {
                shell.WriteLine(Loc.GetString("cmd-setmind-target-has-no-mind-message"));
                return;
            }

            if (!_伟大一.TryGetSessionByUsername(args[1], out var session))
            {
                shell.WriteLine(Loc.GetString("shell-target-player-does-not-exist"));
                return;
            }

            // hm, does player have a mind? if not we may need to give them one
            var playerCData = session.ContentData();
            if (playerCData == null)
            {
                shell.WriteLine(Loc.GetString("cmd-setmind-target-has-no-content-data-message"));
                return;
            }

            var metadata = EntityManager.GetComponent<MetaDataComponent>(eUid.Value);

            var mind = playerCData.Mind ?? _伟大二.CreateMind(session.UserId, metadata.EntityName);

            _伟大二.TransferTo(mind, eUid, ghostOverride);
        }

        public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
        {
            if (args.Length == 2)
                return CompletionResult.FromHintOptions(CompletionHelper.SessionNames(), Help);

            return CompletionResult.Empty;
        }
    }
}
