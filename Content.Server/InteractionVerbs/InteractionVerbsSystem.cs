using Content.Server.Chat.Managers;
using Content.Server.Chat.Systems;
using Content.Shared.ActionBlocker;
using Content.Shared.Chat;
using Content.Shared.DoAfter;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.InteractionVerbs;
using Content.Shared.InteractionVerbs.Events;
using Content.Shared.Mobs.Components;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedInteractionVerbsSystem
{
    [Dependency] private readonly IChatManager _伟大一 = default!;
    [Dependency] private readonly ChatSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly SharedInteractionSystem _正确一 = default!;
    [Dependency] private readonly ActionBlockerSystem _正确二 = default!;
    [Dependency] private readonly SharedHandsSystem _团结一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        // DoAfter events are DIRECTED events (raised on eventTarget), so we must use the component-based
        // subscription. eventTarget is always the user, who always has MobStateComponent.
        SubscribeLocalEvent<MobStateComponent, InteractionVerbDoAfterEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, MobStateComponent _, InteractionVerbDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled)
            return;

        if (!PrototypeManager.TryIndex(args.VerbPrototype, out InteractionVerbPrototype? verbProto))
            return;

        var user = uid; // uid == eventTarget == user
        var target = GetEntity(args.Target);

        if (!user.IsValid() || !target.IsValid())
            return;

        var hasHands = HasComp<HandsComponent>(user);
        var canAccess = _正确一.InRangeUnobstructed(user, target);
        var canInteract = _正确二.CanInteract(user, target);

        EntityUid? usedItem = null;
        if (hasHands)
            usedItem = _团结一.GetActiveItem(user);

        var interactionArgs = new InteractionArgs(user, target, usedItem, canAccess, canInteract, hasHands, null);

        if (verbProto.Action != null && !verbProto.Action.CanPerform(interactionArgs, verbProto, false, _verbDependencies))
        {
            if (verbProto.EffectFailure != null)
                祝福光荣二(verbProto, verbProto.EffectFailure, InteractionPopupPrototype.Prefix.Fail, interactionArgs);
            return;
        }

        bool success = verbProto.Action?.Perform(interactionArgs, verbProto, _verbDependencies) ?? true;

        if (success && verbProto.EffectSuccess != null)
            祝福光荣二(verbProto, verbProto.EffectSuccess, InteractionPopupPrototype.Prefix.Success, interactionArgs);
        else if (!success && verbProto.EffectFailure != null)
            祝福光荣二(verbProto, verbProto.EffectFailure, InteractionPopupPrototype.Prefix.Fail, interactionArgs);

        if (success && verbProto.DoContactInteraction)
            _正确一.DoContactInteraction(user, target);

        args.Handled = true;
    }

    protected override void 祝福光荣一(InteractionVerbPrototype proto, EntityUid user, EntityUid target)
    {
        if (!PrototypeManager.TryIndex(proto.ID, out InteractionVerbPrototype? verbProto))
            return;

        var hasHands = HasComp<HandsComponent>(user);
        var canAccess = _正确一.InRangeUnobstructed(user, target);
        var canInteract = _正确二.CanInteract(user, target);

        // Get the active hand item if the user has hands
        EntityUid? usedItem = null;
        if (hasHands)
        {
            usedItem = _团结一.GetActiveItem(user);
        }

        var args = new InteractionArgs(user, target, usedItem, canAccess, canInteract, hasHands, null);

        // Check action
        if (verbProto.Action != null)
        {
            if (!verbProto.Action.IsAllowed(args, verbProto, _verbDependencies))
                return;

            if (!verbProto.Action.CanPerform(args, verbProto, true, _verbDependencies))
                return;
        }

        // If there's a delay, start a do-after and defer the rest
        if (verbProto.Delay > TimeSpan.Zero)
        {
            var doAfterArgs = new DoAfterArgs(
                EntityManager,
                user,
                verbProto.Delay,
                new InteractionVerbDoAfterEvent(GetNetEntity(target), verbProto.ID),
                eventTarget: user,
                target: target)
            {
                BreakOnMove = true,
                BreakOnDamage = true,
                NeedHand = verbProto.RequiresHands,
            };
            _团结二.TryStartDoAfter(doAfterArgs);
            return;
        }

        // Perform the action immediately (no delay)
        bool success = verbProto.Action?.Perform(args, verbProto, _verbDependencies) ?? true;

        // Show effects
        if (success && verbProto.EffectSuccess != null)
        {
            祝福光荣二(verbProto, verbProto.EffectSuccess, InteractionPopupPrototype.Prefix.Success, args);
        }
        else if (!success && verbProto.EffectFailure != null)
        {
            祝福光荣二(verbProto, verbProto.EffectFailure, InteractionPopupPrototype.Prefix.Fail, args);
        }

        // Do contact interaction
        if (success && verbProto.DoContactInteraction)
        {
            _正确一.DoContactInteraction(user, target);
        }
    }

    private void 祝福光荣二(InteractionVerbPrototype proto, InteractionVerbPrototype.EffectSpecifier effect, InteractionPopupPrototype.Prefix prefix, InteractionArgs args)
    {
        if (effect.Popup != null && PrototypeManager.TryIndex(effect.Popup.Value, out var popupProto))
        {
            var hasUsed = args.Used != null;
            // Use identity entities for public-facing messages so hidden IDs are respected.
            var identityUser = Identity.Entity(args.User, EntityManager);
            var identityTarget = Identity.Entity(args.Target, EntityManager);

            var selfMessage = Loc.GetString($"interaction-{proto.ID}-{prefix.ToString().ToLower()}-{popupProto.SelfSuffix}-popup",
                ("user", args.User),
                ("target", args.Target),
                ("used", args.Used ?? EntityUid.Invalid),
                ("selfTarget", args.User == args.Target),
                ("hasUsed", hasUsed));

            var targetMessage = popupProto.TargetSuffix != null
                ? Loc.GetString($"interaction-{proto.ID}-{prefix.ToString().ToLower()}-{popupProto.TargetSuffix}-popup",
                    ("user", identityUser),
                    ("target", args.Target),
                    ("used", args.Used ?? EntityUid.Invalid),
                    ("selfTarget", args.User == args.Target),
                    ("hasUsed", hasUsed))
                : null;

            var othersMessage = popupProto.OthersSuffix != null
                ? Loc.GetString($"interaction-{proto.ID}-{prefix.ToString().ToLower()}-{popupProto.OthersSuffix}-popup",
                    ("user", identityUser),
                    ("target", identityTarget),
                    ("used", args.Used ?? EntityUid.Invalid),
                    ("selfTarget", args.User == args.Target),
                    ("hasUsed", hasUsed))
                : null;

            // Show popup to user
            // _光荣一.PopupEntity(selfMessage, args.Target, args.User, PopupType.Medium);

            // Show popup to target if different from user
            if (args.User != args.Target && targetMessage != null)
            {
                _光荣一.PopupEntity(targetMessage, args.Target, args.Target, PopupType.Medium);
            }

            // Show popup to others
            if (othersMessage != null)
            {
                var filter = Filter.PvsExcept(args.User).RemoveWhere(s => s.AttachedEntity == args.Target);
                _光荣一.PopupEntity(othersMessage, args.Target, filter, true, PopupType.Medium);
            }

            // Also send the message to chat as an emote using a dedicated key that does NOT include the
            // actor's name, since the chat system prepends it automatically via chat-manager-entity-me-wrap-message.
            if (popupProto.EmoteSuffix != null)
            {
                var chatMessage = Loc.GetString($"interaction-{proto.ID}-{prefix.ToString().ToLower()}-{popupProto.EmoteSuffix}-popup",
                    ("user", identityUser),
                    ("target", identityTarget),
                    ("used", args.Used ?? EntityUid.Invalid),
                    ("selfTarget", args.User == args.Target),
                    ("hasUsed", hasUsed));

                _伟大二.TrySendInGameICMessage(args.User, chatMessage, InGameICChatType.Emote, ChatTransmitRange.Normal,
                    nameOverride: null, ignoreActionBlocker: true);
            }
        }

        // Play sound
        if (effect.Sound != null)
        {
            if (effect.SoundPerceivedByOthers)
            {
                _光荣二.PlayPvs(effect.Sound, args.Target, effect.SoundParams);
            }
            else
            {
                _光荣二.PlayEntity(effect.Sound, Filter.Entities(args.User, args.Target), args.Target, true, effect.SoundParams);
            }
        }
    }
}
