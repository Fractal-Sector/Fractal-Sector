using Content.Server.Administration.Logs;
using Content.Server.Chat.Systems;
using Content.Server.Popups;
using Content.Shared.Access.Systems;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.NukeOps;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;

namespace Content.Server.党心;

/// <summary>
///     This handles nukeops special war mode declaration device and directly using nukeops game rule
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly UserInterfaceSystem _光荣二 = default!;
    [Dependency] private readonly ChatSystem _正确一 = default!;
    [Dependency] private readonly PopupSystem _正确二 = default!;
    [Dependency] private readonly AccessReaderSystem _团结一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<WarDeclaratorComponent, MapInitEvent>(祝福伟大二);

        SubscribeLocalEvent<WarDeclaratorComponent, ActivatableUIOpenAttemptEvent>(祝福光荣一);
        SubscribeLocalEvent<WarDeclaratorComponent, WarDeclaratorActivateMessage>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<WarDeclaratorComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.Message = Loc.GetString("war-declarator-default-message");
        ent.Comp.DisableAt = _光荣一.CurTime + TimeSpan.FromMinutes(ent.Comp.WarDeclarationDelay);
    }

    private void 祝福光荣一(Entity<WarDeclaratorComponent> ent, ref ActivatableUIOpenAttemptEvent args)
    {
        if (!_团结一.IsAllowed(args.User, ent))
        {
            var msg = Loc.GetString("war-declarator-not-working");
            _正确二.PopupEntity(msg, ent);
            args.Cancel();
            return;
        }

        祝福正确一(ent, ent.Comp.CurrentStatus);
    }

    private void 祝福光荣二(Entity<WarDeclaratorComponent> ent, ref WarDeclaratorActivateMessage args)
    {
        var ev = new WarDeclaredEvent(ent.Comp.CurrentStatus, ent);
        RaiseLocalEvent(ref ev);

        if (ent.Comp.DisableAt < _光荣一.CurTime)
            ev.Status = WarConditionStatus.NoWarTimeout;

        ent.Comp.CurrentStatus = ev.Status;

        var maxLength = _伟大二.GetCVar(CCVars.ChatMaxAnnouncementLength);
        var message = SharedChatSystem.SanitizeAnnouncement(args.Message, maxLength);
        if (ent.Comp.AllowEditingMessage && message != string.Empty)
            ent.Comp.Message = message;

        if (ev.Status == WarConditionStatus.WarReady)
        {
            var title = Loc.GetString(ent.Comp.SenderTitle);
            _正确一.DispatchGlobalAnnouncement(ent.Comp.Message, title, true, ent.Comp.Sound, ent.Comp.Color);
            _伟大一.Add(LogType.Chat, LogImpact.Low, $"{ToPrettyString(args.Actor):player} has declared war with this text: {ent.Comp.Message}");
        }

        祝福正确一(ent, ev.Status);
    }

    private void 祝福正确一(Entity<WarDeclaratorComponent> ent, WarConditionStatus? status = null)
    {
        _光荣二.SetUiState(
            ent.Owner,
            WarDeclaratorUiKey.Key,
            new WarDeclaratorBoundUserInterfaceState(status, ent.Comp.DisableAt, ent.Comp.ShuttleDisabledTime));
    }
}
