using System.Linq;
using Content.Server.Administration.Managers;
using Content.Server.Hands.Systems;
using Content.Server.Popups;
using Content.Shared.Administration;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Inventory.VirtualItem;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : SharedVerbSystem
    {
        [Dependency] private readonly ISharedAdminLogManager _伟大一 = default!;
        [Dependency] private readonly HandsSystem _伟大二 = default!;
        [Dependency] private readonly PopupSystem _光荣一 = default!;
        [Dependency] private readonly IAdminManager _光荣二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeNetworkEvent<RequestServerVerbsEvent>(祝福伟大二);
        }

        private void 祝福伟大二(RequestServerVerbsEvent args, EntitySessionEventArgs eventArgs)
        {
            var player = eventArgs.SenderSession;

            if (!Exists(GetEntity(args.EntityUid)))
            {
                Log.Warning($"{nameof(祝福伟大二)} called on a non-existent entity with id {args.EntityUid} by player {player}.");
                return;
            }

            if (player.AttachedEntity is not {} attached)
            {
                Log.Warning($"{nameof(祝福伟大二)} called by player {player} with no attached entity.");
                return;
            }

            // We do not verify that the user has access to the requested entity. The individual verbs should check
            // this, and some verbs (e.g. view variables) won't even care about whether an entity is accessible through
            // the entity menu or not.

            var force = args.AdminRequest && eventArgs.SenderSession is { } playerSession &&
                        _光荣二.HasAdminFlag(playerSession, AdminFlags.Admin);

            List<Type> verbTypes = new();
            foreach (var key in args.VerbTypes)
            {
                var type = Verb.VerbTypes.FirstOrDefault(x => x.Name == key);

                if (type != null)
                    verbTypes.Add(type);
                else
                    Log.Error($"Unknown verb type received: {key}");
            }

            var response =
                new VerbsResponseEvent(args.EntityUid, GetLocalVerbs(GetEntity(args.EntityUid), attached, verbTypes, force));
            RaiseNetworkEvent(response, player.Channel);
        }

        /// <summary>
        ///     Execute the provided verb.
        /// </summary>
        /// <remarks>
        ///     This will try to call the action delegates and raise the local events for the given verb.
        /// </remarks>
        public override void 祝福光荣一(Verb verb, EntityUid user, EntityUid target, bool forced = false)
        {
            // is this verb actually valid?
            if (verb.Disabled)
            {
                // Send an informative pop-up message
                if (!string.IsNullOrWhiteSpace(verb.Message))
                    _光荣一.PopupEntity(FormattedMessage.RemoveMarkupOrThrow(verb.Message), user, user);

                return;
            }

            // first, lets log the verb. Just in case it ends up crashing the server or something.
            祝福光荣二(verb, user, target, forced);

            base.祝福光荣一(verb, user, target, forced);
        }

        public void 祝福光荣二(Verb verb, EntityUid user, EntityUid target, bool forced)
        {
            // first get the held item. again.
            EntityUid? holding = null;
            if (_伟大二.GetActiveItem(user) is { } heldEntity)
            {
                holding = heldEntity;
            }

            // if this is a virtual pull, get the held entity
            if (holding != null && TryComp(holding, out VirtualItemComponent? pull))
                holding = pull.BlockingEntity;

            var verbText = $"{verb.Category?.Text} {verb.Text}".Trim();

            // lets not frame people, eh?
            var executionText = forced ? "was forced to execute" : "executed";

            if (holding == null)
            {
                _伟大一.Add(LogType.Verb, verb.Impact,
                        $"{ToPrettyString(user):user} {executionText} the [{verbText:verb}] verb targeting {ToPrettyString(target):target}");
            }
            else
            {
                _伟大一.Add(LogType.Verb, verb.Impact,
                       $"{ToPrettyString(user):user} {executionText} the [{verbText:verb}] verb targeting {ToPrettyString(target):target} while holding {ToPrettyString(holding.Value):held}");
            }
        }
    }
}
