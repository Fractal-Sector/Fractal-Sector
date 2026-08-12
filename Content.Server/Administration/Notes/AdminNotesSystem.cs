using System.Linq;
using Content.Server.Administration.Commands;
using Content.Server.Chat.Managers;
using Content.Server.EUI;
using Content.Shared.Database;
using Content.Shared.Verbs;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.Administration.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IConsoleHost _伟大一 = default!;
    [Dependency] private readonly IAdminNotesManager _伟大二 = default!;
    [Dependency] private readonly IPlayerManager _光荣一 = default!;
    [Dependency] private readonly IChatManager _光荣二 = default!;
    [Dependency] private readonly EuiManager _正确一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<GetVerbsEvent<Verb>>(祝福伟大二);
        _光荣一.PlayerStatusChanged += 祝福光荣一;
    }

    private void 祝福伟大二(GetVerbsEvent<Verb> ev)
    {
        if (EntityManager.GetComponentOrNull<ActorComponent>(ev.User) is not {PlayerSession: var user} ||
            EntityManager.GetComponentOrNull<ActorComponent>(ev.Target) is not {PlayerSession: var target})
        {
            return;
        }

        if (!_伟大二.CanView(user))
        {
            return;
        }

        var verb = new Verb
        {
            Text = Loc.GetString("admin-notes-verb-text"),
            Category = VerbCategory.Admin,
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/examine.svg.192dpi.png")),
            Act = () => _伟大一.RemoteExecuteCommand(user, $"{OpenAdminNotesCommand.CommandName} \"{target.UserId}\""),
            Impact = LogImpact.Low
        };

        ev.Verbs.Add(verb);
    }

    private async void 祝福光荣一(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus != SessionStatus.InGame)
            return;

        var messages = await _伟大二.GetNewMessages(e.Session.UserId);
        var watchlists = await _伟大二.GetActiveWatchlists(e.Session.UserId);

        if (!_光荣一.TryGetPlayerData(e.Session.UserId, out var playerData))
        {
            Log.Error($"Could not get player data for ID {e.Session.UserId}");
        }

        var username = playerData?.UserName ?? e.Session.UserId.ToString();
        foreach (var watchlist in watchlists)
        {
            _光荣二.SendAdminAlert(Loc.GetString("admin-notes-watchlist", ("player", username), ("message", watchlist.Message)));
        }

        var messagesToShow = messages.OrderBy(x => x.CreatedAt).Where(x => !x.Dismissed).ToArray();
        if (messagesToShow.Length == 0)
            return;

        var ui = new AdminMessageEui(messagesToShow);
        _正确一.OpenEui(ui, e.Session);
    }
}
