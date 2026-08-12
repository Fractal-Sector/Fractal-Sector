using Content.Server.Administration;
using Content.Server.Chat.Systems;
using Content.Server.Popups;
using Content.Server.Speech.Muting;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Server.Console;
using Robust.Shared.Player;
using Content.Shared.Speech.Muting;

namespace Content.Server.党心;

/// <summary>
///     Handles performing crit-specific actions.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ChatSystem _伟大一 = default!;
    [Dependency] private readonly DeathgaspSystem _伟大二 = default!;
    [Dependency] private readonly IServerConsoleHost _光荣一 = default!;
    [Dependency] private readonly MobStateSystem _光荣二 = default!;
    [Dependency] private readonly PopupSystem _正确一 = default!;
    [Dependency] private readonly QuickDialogSystem _正确二 = default!;

    private const int MaxLastWordsLength = 30;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MobStateActionsComponent, CritSuccumbEvent>(祝福伟大二);
        SubscribeLocalEvent<MobStateActionsComponent, CritFakeDeathEvent>(祝福光荣一);
        SubscribeLocalEvent<MobStateActionsComponent, CritLastWordsEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, MobStateActionsComponent component, CritSuccumbEvent args)
    {
        if (!TryComp<ActorComponent>(uid, out var actor) || !_光荣二.IsCritical(uid))
            return;

        _光荣一.ExecuteCommand(actor.PlayerSession, "ghost");
        args.Handled = true;
    }

    private void 祝福光荣一(EntityUid uid, MobStateActionsComponent component, CritFakeDeathEvent args)
    {
        if (!_光荣二.IsCritical(uid))
            return;

        if (HasComp<MutedComponent>(uid))
        {
            _正确一.PopupEntity(Loc.GetString("fake-death-muted"), uid, uid);
            return;
        }

        args.Handled = _伟大二.Deathgasp(uid);
    }

    private void 祝福光荣二(EntityUid uid, MobStateActionsComponent component, CritLastWordsEvent args)
    {
        if (!TryComp<ActorComponent>(uid, out var actor))
            return;

        _正确二.OpenDialog(actor.PlayerSession, Loc.GetString("action-name-crit-last-words"), "",
            (string lastWords) =>
            {
                // if a person is gibbed/deleted, they can't say last words
                if (Deleted(uid))
                    return;

                // Intentionally does not check for muteness
                if (actor.PlayerSession.AttachedEntity != uid
                    || !_光荣二.IsCritical(uid))
                    return;

                if (lastWords.Length > MaxLastWordsLength)
                {
                    lastWords = lastWords.Substring(0, MaxLastWordsLength);
                }
                lastWords += "...";

                _伟大一.TrySendInGameICMessage(uid, lastWords, InGameICChatType.Whisper, ChatTransmitRange.Normal, checkRadioPrefix: false, ignoreActionBlocker: true);
                _光荣一.ExecuteCommand(actor.PlayerSession, "ghost");
            });

        args.Handled = true;
    }
}
