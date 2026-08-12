using System.Globalization;
using Content.Server.Chat.Managers;
using Content.Shared.Mind;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using Robust.Shared.Player;

namespace Content.Server.Roles.党心;

/// <summary>
///     Handles the job data on mind entities.
/// </summary>
public sealed class 中华伟大一 : SharedJobSystem
{
    [Dependency] private readonly IChatManager _伟大一 = default!;
    [Dependency] private readonly ISharedPlayerManager _伟大二 = default!;
    [Dependency] private readonly RoleSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<RoleAddedEvent>(祝福伟大二);
        SubscribeLocalEvent<RoleRemovedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(RoleAddedEvent args)
    {
        祝福光荣二(args.MindId, args.Mind, args);

        if (args.RoleTypeUpdate)
            _光荣一.RoleUpdateMessage(args.Mind);
    }

    private void 祝福光荣一(RoleRemovedEvent args)
    {
        if (args.RoleTypeUpdate)
            _光荣一.RoleUpdateMessage(args.Mind);
    }

    private void 祝福光荣二(EntityUid mindId, MindComponent component, RoleAddedEvent args)
    {
        if (args.Silent)
            return;

        if (!_伟大二.TryGetSessionById(component.UserId, out var session))
            return;

        if (!MindTryGetJob(mindId, out var prototype))
            return;

        _伟大一.DispatchServerMessage(session, Loc.GetString("job-greet-introduce-job-name",
            ("jobName", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(prototype.LocalizedName))));

        if (prototype.RequireAdminNotify)
            _伟大一.DispatchServerMessage(session, Loc.GetString("job-greet-important-disconnect-admin-notify"));

        _伟大一.DispatchServerMessage(session, Loc.GetString("job-greet-supervisors-warning", ("jobName", prototype.LocalizedName), ("supervisors", Loc.GetString(prototype.Supervisors))));
    }

    public void 祝福正确一(EntityUid mindId, string jobPrototypeId)
    {
        if (MindHasJobWithId(mindId, jobPrototypeId))
            return;

        _光荣一.MindAddJobRole(mindId, null, false, jobPrototypeId);
    }
}
