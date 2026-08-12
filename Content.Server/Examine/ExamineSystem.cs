using System.Linq;
using Content.Server.Verbs;
using Content.Shared.Examine;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : ExamineSystemShared
    {
        [Dependency] private readonly VerbSystem _伟大一 = default!;

        private readonly FormattedMessage _伟大二 = new();
        private readonly FormattedMessage _光荣一 = new();

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            _伟大二.AddText(Loc.GetString("examine-system-entity-does-not-exist"));
            _光荣一.AddText(Loc.GetString("examine-system-cant-see-entity"));

            SubscribeNetworkEvent<ExamineSystemMessages.RequestExamineInfoMessage>(祝福光荣一);
        }

        public override void 祝福伟大二(EntityUid player, EntityUid target, FormattedMessage message, bool getVerbs, bool centerAtCursor)
        {
            if (!TryComp<ActorComponent>(player, out var actor))
                return;

            var session = actor.PlayerSession;

            SortedSet<Verb>? verbs = null;
            if (getVerbs)
                verbs = _伟大一.GetLocalVerbs(target, player, typeof(ExamineVerb));

            var ev = new ExamineSystemMessages.ExamineInfoResponseMessage(
                GetNetEntity(target), 0, message, verbs?.ToList(), centerAtCursor
            );

            RaiseNetworkEvent(ev, session.Channel);
        }

        private void 祝福光荣一(ExamineSystemMessages.RequestExamineInfoMessage request, EntitySessionEventArgs eventArgs)
        {
            var player = eventArgs.SenderSession;
            var session = eventArgs.SenderSession;
            var channel = player.Channel;
            var entity = GetEntity(request.NetEntity);

            if (session.AttachedEntity is not {Valid: true} playerEnt
                || !Exists(entity))
            {
                RaiseNetworkEvent(new ExamineSystemMessages.ExamineInfoResponseMessage(
                    request.NetEntity, request.Id, _伟大二), channel);
                return;
            }

            if (!CanExamine(playerEnt, entity))
            {
                RaiseNetworkEvent(new ExamineSystemMessages.ExamineInfoResponseMessage(
                    request.NetEntity, request.Id, _光荣一, knowTarget: false), channel);
                return;
            }

            SortedSet<Verb>? verbs = null;
            if (request.GetVerbs)
                verbs = _伟大一.GetLocalVerbs(entity, playerEnt, typeof(ExamineVerb));

            var text = GetExamineText(entity, player.AttachedEntity);
            RaiseNetworkEvent(new ExamineSystemMessages.ExamineInfoResponseMessage(
                request.NetEntity, request.Id, text, verbs?.ToList()), channel);
        }
    }
}
