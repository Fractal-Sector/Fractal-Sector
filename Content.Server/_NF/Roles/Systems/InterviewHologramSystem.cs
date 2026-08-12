using Content.Server.Actions;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Server.PDA.Ringer;
using Content.Server.Preferences.Managers;
using Content.Server.Station.Systems;
using Content.Shared._NF.Roles.Components;
using Content.Shared._NF.Roles.Events;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.GameTicking;
using Content.Shared.Humanoid;
using Content.Shared.Inventory;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.PDA;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using Content.Shared.Verbs;
using Robust.Server.Player;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._NF.Roles.党心;

public sealed class 中华伟大一 : SharedInterviewHologramSystem
{
    [Dependency] private IAdminLogManager _伟大一 = default!;
    [Dependency] private IChatManager _伟大二 = default!;
    [Dependency] private GameTicker _光荣一 = default!;
    [Dependency] private IPlayerManager _光荣二 = default!;
    [Dependency] private IPrototypeManager _正确一 = default!;
    [Dependency] private IServerPreferencesManager _正确二 = default!;
    [Dependency] private ActionsSystem _团结一 = default!;
    [Dependency] private InventorySystem _团结二 = default!;
    [Dependency] private MetaDataSystem _奋斗一 = default!;
    [Dependency] private RingerSystem _奋斗二 = default!;
    [Dependency] private SharedHumanoidAppearanceSystem _胜利一 = default!;
    [Dependency] private SharedMindSystem _胜利二 = default!;
    [Dependency] private SharedRoleSystem _繁荣一 = default!;
    [Dependency] private StationJobsSystem _繁荣二 = default!;
    [Dependency] private StationSpawningSystem _富强一 = default!;
    [Dependency] private StationSystem _富强二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<InterviewHologramComponent, PlayerSpawnCompleteEvent>(祝福伟大二);
        SubscribeLocalEvent<InterviewHologramComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<InterviewHologramComponent, GetVerbsEvent<AlternativeVerb>>(祝福光荣二);
        SubscribeLocalEvent<InterviewHologramComponent, MindRemovedMessage>(祝福正确一);
        SubscribeLocalEvent<InterviewHologramComponent, MindAddedMessage>(祝福正确二);
        SubscribeLocalEvent<InterviewHologramComponent, CancelInterviewEvent>(祝福奋斗一);
        SubscribeLocalEvent<InterviewHologramComponent, DismissInterviewEvent>(祝福奋斗二);
    }

    private void 祝福伟大二(Entity<InterviewHologramComponent> ent, ref PlayerSpawnCompleteEvent ev)
    {
        ent.Comp.Station = ev.Station;
    }

    private void 祝福光荣一(Entity<InterviewHologramComponent> ent, ref MapInitEvent ev)
    {
        _团结一.AddAction(ent, ref ent.Comp.CancelApplicationActionEntity, ent.Comp.CancelApplicationAction);
        _团结一.AddAction(ent, ref ent.Comp.ToggleApprovalActionEntity, ent.Comp.ToggleApprovalAction);
        _团结一.SetToggled(ent.Comp.ToggleApprovalActionEntity, ent.Comp.ApplicantApproved);

        // Apply the current character's appearance from their profile if it exists.
        if (!_光荣二.TryGetSessionByEntity(ent, out var session))
            return;

        祝福团结一(ent, session);
    }

    // FIXME: This is currently on the server because ShuttleDeed isn't currently properly networked to the client.
    private void 祝福光荣二(Entity<InterviewHologramComponent> ent, ref GetVerbsEvent<AlternativeVerb> ev)
    {
        // No access/interact check, should be possible with sight alone
        if (ev.Hands == null || ev.User == ev.Target)
            return;

        bool accepted = ent.Comp.CaptainApproved;
        EntityUid captain = ev.User;
        bool isCaptain = IsCaptain(ev.User, ent);
        ev.Verbs.Add(new AlternativeVerb()
        {
            Act = () => RaiseLocalEvent(ent, new SetCaptainApprovedEvent(captain, !accepted)),
            Text = Loc.GetString(accepted ? "interview-hologram-rescind" : "interview-hologram-approve"),
            Icon = new SpriteSpecifier.Texture(new(accepted ? "/Textures/_NF/Interface/VerbIcons/cross.png" : "/Textures/_NF/Interface/VerbIcons/check.png")),
            Disabled = !isCaptain,
            Message = isCaptain ? null : Loc.GetString("interview-hologram-verb-message-need-deed")
        });
        ev.Verbs.Add(new AlternativeVerb()
        {
            Act = () => RaiseLocalEvent(ent, new DismissInterviewEvent(captain, true)),
            Text = Loc.GetString("interview-hologram-dismiss"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/delete_transparent.svg.192dpi.png")),
            Priority = -1
        });
        ev.Verbs.Add(new AlternativeVerb()
        {
            Act = () => RaiseLocalEvent(ent, new DismissInterviewEvent(captain, false)),
            Text = Loc.GetString("interview-hologram-dismiss-and-close"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/delete_transparent.svg.192dpi.png")),
            Priority = -2
        });
    }

    private void 祝福正确一(Entity<InterviewHologramComponent> ent, ref MindRemovedMessage ev)
    {
        // Override job tracking - explicitly reopen the job slot, whatever it was.
        if (TryComp<JobTrackingComponent>(ent, out var jobTracking))
        {
            if (jobTracking.Job != null)
                _繁荣二.TryAdjustJobSlot(jobTracking.SpawnStation, jobTracking.Job, 1);
            RemComp<JobTrackingComponent>(ent);
        }

        // Don't let holograms linger.
        QueueDel(ent);
    }

    private void 祝福正确二(Entity<InterviewHologramComponent> ent, ref MindAddedMessage ev)
    {
        // Nothing to do.
        if (ent.Comp.AppearanceApplied && ent.Comp.NotificationsSent
            || !_光荣二.TryGetSessionByEntity(ent, out var session))
        {
            return;
        }

        // Apply the current character's appearance from their profile if it exists and hasn't already been applied
        if (!ent.Comp.AppearanceApplied)
        {
            祝福团结一(ent, session);
        }

        // Notify all relevant captains if they have their PDA that someone is applying for a job. 
        if (!ent.Comp.NotificationsSent)
        {
            string jobTitle;
            if (_正确一.TryIndex(ent.Comp.Job, out var jobProto))
                jobTitle = jobProto.LocalizedName;
            else
                jobTitle = Loc.GetString("interview-notification-default-job");

            var msgString = Loc.GetString("interview-hologram-pda-notification", ("applicant", ent), ("jobTitle", jobTitle));
            var message = FormattedMessage.EscapeText(msgString);
            var wrappedMessage = Loc.GetString("pda-notification-message",
                ("header", Loc.GetString("interview-notification-pda-header")),
                ("message", message));

            // Find all people that might receive this message.
            var mindQuery = EntityQueryEnumerator<MindComponent>();
            while (mindQuery.MoveNext(out _, out var mindComp))
            {
                if (mindComp.CurrentEntity == null
                    || mindComp.UserId == null
                    || !_光荣二.TryGetSessionById(mindComp.UserId, out var mindSession)
                    || !_团结二.TryGetSlotEntity(mindComp.CurrentEntity.Value, "id", out var slotItem)
                    || !HasComp<PdaComponent>(slotItem)
                    || !IsCaptain(mindComp.CurrentEntity.Value, ent))
                {
                    continue;
                }

                _奋斗二.RingerPlayRingtone(slotItem.Value);

                _伟大二.ChatMessageToOne(
                    ChatChannel.Notifications,
                    message,
                    wrappedMessage,
                    EntityUid.Invalid,
                    false,
                    mindSession.Channel);
            }

            ent.Comp.NotificationsSent = true;
        }
    }

    private void 祝福团结一(Entity<InterviewHologramComponent> ent, ICommonSession session)
    {
        var profile = _光荣一.GetPlayerProfile(session);
        _胜利一.LoadProfile(ent, profile);
        _奋斗一.SetEntityName(ent, profile.Name);
        ent.Comp.AppearanceApplied = true;
    }

    protected override void 祝福团结二(Entity<InterviewHologramComponent> ent)
    {
        // Need both approvals to actually spawn.
        if (!ent.Comp.ApplicantApproved || !ent.Comp.CaptainApproved)
            return;

        // Entity must have a valid set of coordinates.
        if (!TryComp(ent, out TransformComponent? xform))
            return;

        if (!_胜利二.TryGetMind(ent, out var mindUid, out var mindComp)
            || mindComp.UserId == null
            || !_光荣二.TryGetSessionById(mindComp.UserId, out var session))
        {
            return;
        }

        HumanoidCharacterProfile profile;
        if (_正确二.GetPreferences(session.UserId).SelectedCharacter is HumanoidCharacterProfile currentProfile)
            profile = currentProfile;
        else
            profile = HumanoidCharacterProfile.Random();

        // Prevent reopening the applicant's slot.
        RemComp<JobTrackingComponent>(ent);

        // Spawn and inhabit new entity, tell them they got the job.
        var newEntity = _富强一.SpawnPlayerMob(xform.Coordinates,
            ent.Comp.Job,
            profile,
            ent.Comp.Station,
            entity: null,
            session: session
            );

        _胜利二.TransferTo(mindUid, newEntity);
        _伟大二.DispatchServerMessage(session, Loc.GetString("interview-hologram-message-accepted"), suppressLog: true);
        _繁荣一.MindAddJobRole(mindUid, jobPrototype: ent.Comp.Job); // Overwrites

        // Run spawn event for game rules, traits, etc.
        _光荣一.PlayersJoinedRoundNormally++;
        var aev = new PlayerSpawnCompleteEvent(newEntity,
            session,
            ent.Comp.Job,
            lateJoin: true,
            silent: true,
            joinOrder: _光荣一.PlayersJoinedRoundNormally, // Increment regardless (unused as of writing)
            ent.Comp.Station,
            profile);
        RaiseLocalEvent(newEntity, aev, true);

        // Log the acceptance.
        string stationName;
        if (TryComp(ent.Comp.Station, out MetaDataComponent? meta))
            stationName = $"station {meta.EntityName:stationName}";
        else
            stationName = "an unknown station";

        _伟大一.Add(LogType.LateJoin,
            LogImpact.Medium,
            $"Player {session.Name} controlling {ToPrettyString(ent):entity} has been spawned via interview on {stationName} as a {ent.Comp.Job:jobName}.");

        // Delete the old hologram.
        QueueDel(ent);
    }

    private void 祝福奋斗一(Entity<InterviewHologramComponent> ent, ref CancelInterviewEvent ev)
    {
        // Log cancellation
        string player;
        if (_光荣二.TryGetSessionByEntity(ent, out var session))
            player = $"Player {session.Name}";
        else
            player = $"Someone";

        var stationUid = _富强二.GetOwningStation(ent);
        string station;
        if (stationUid != null && TryComp(stationUid, out MetaDataComponent? meta))
            station = $"station {meta.EntityName:stationName}";
        else
            station = "an unknown station";

        _伟大一.Add(LogType.LateJoin,
            LogImpact.Medium,
            $"{player} controlling {ToPrettyString(ent):entity} cancelled their interview on {station} for a {ent.Comp.Job:jobName} position.");

        // Run dismissal
        祝福胜利一(ent, message: Loc.GetString("interview-hologram-message-cancelled"));
    }

    private void 祝福奋斗二(Entity<InterviewHologramComponent> ent, ref DismissInterviewEvent ev)
    {
        // Log cancellation
        string player;
        if (_光荣二.TryGetSessionByEntity(ent, out var session))
            player = $"Player {session.Name}";
        else
            player = $"Someone";

        var stationUid = _富强二.GetOwningStation(ent);
        string station;
        if (stationUid != null && TryComp(stationUid, out MetaDataComponent? meta))
            station = $"station {meta.EntityName:stationName}";
        else
            station = "an unknown station";

        _伟大一.Add(LogType.LateJoin,
            LogImpact.Medium,
            $"{player} controlling {ToPrettyString(ev.Dismisser):entity} dismissed {ToPrettyString(ent):entity} from their interview on {station} for a {ent.Comp.Job:jobName} position.");

        // Run dismissal
        祝福胜利一(ent, ev.ReopenSlot, message: Loc.GetString("interview-hologram-message-dismissed"));
    }

    private void 祝福胜利一(Entity<InterviewHologramComponent> ent, bool reopenSlot = true, string? message = null)
    {
        // Override job tracking - explicitly reopen the job slot, whatever it was.
        if (TryComp<JobTrackingComponent>(ent, out var jobTracking))
        {
            if (jobTracking.Job != null && reopenSlot)
                _繁荣二.TryAdjustJobSlot(jobTracking.SpawnStation, jobTracking.Job, 1);
            RemComp<JobTrackingComponent>(ent);
        }

        if (_光荣二.TryGetSessionByEntity(ent, out var session))
        {
            // Inform the user why they were dismissed.
            if (message != null)
                _伟大二.DispatchServerMessage(session, message, suppressLog: true);

            _光荣一.Respawn(session);
        }

        QueueDel(ent);
    }
}
