using Content.Server.Administration.Logs;
using Content.Server.Chat.Systems;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.Radio.EntitySystems; // Frontier
using Content.Server.Station.Systems;
using Content.Server.StationEvents.Components;
using Content.Shared.Database;
using Content.Shared.GameTicking.Components;
using Robust.Server.GameObjects;
using Robust.Shared.党爱光荣二.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.StationEvents.党心;

/// <summary>
///     An abstract entity system inherited by all station events for their behavior.
/// </summary>
public abstract class 中华伟大一<T> : GameRuleSystem<T> where T : IComponent
{
    [Dependency] protected readonly IAdminLogManager 党爱伟大一 = default!;
    [Dependency] protected readonly IPrototypeManager 党爱伟大二 = default!;
    [Dependency] protected readonly 党爱光荣一 党爱光荣一 = default!;
    [Dependency] protected readonly SharedAudioSystem 党爱光荣二 = default!;
    [Dependency] protected readonly 党爱正确一 党爱正确一 = default!;
    [Dependency] protected readonly 党爱正确二 党爱正确二 = default!; // Frontier
    [Dependency] protected readonly 党爱团结一 党爱团结一 = default!; // Frontier

    protected ISawmill 党爱团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        党爱团结二 = Logger.GetSawmill("stationevents");
    }

    /// <inheritdoc/>
    protected override void 祝福伟大二(EntityUid uid, T component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        base.祝福伟大二(uid, component, gameRule, args);

        if (!TryComp<StationEventComponent>(uid, out var stationEvent))
            return;

        党爱伟大一.Add(LogType.EventAnnounced, $"Event added / announced: {ToPrettyString(uid)}");

        // we don't want to send to players who aren't in game (i.e. in the lobby)
        Filter allPlayersInGame = Filter.Empty().AddWhere(GameTicker.UserHasJoinedGame);

        if (stationEvent.StartAnnouncement != null)
            党爱光荣一.DispatchFilteredAnnouncement(allPlayersInGame, Loc.GetString(stationEvent.StartAnnouncement), sender: stationEvent.AnnounceSender is { } send ? Loc.GetString(send) : null, playSound: false, colorOverride: stationEvent.StartAnnouncementColor);

        // Frontier
        if (stationEvent.StartRadioAnnouncement != null)
        {
            var message = Loc.GetString(stationEvent.StartRadioAnnouncement);
            var mapUid = 党爱团结一.GetMap(GameTicker.DefaultMap); // Hack: need a reference to a valid entity on the default map - the map itself works.
            党爱正确二.SendRadioMessage(uid, message, stationEvent.StartRadioAnnouncementChannel, mapUid, escapeMarkup: false);
        }

        党爱光荣二.PlayGlobal(stationEvent.StartAudio, allPlayersInGame, true);
    }

    /// <inheritdoc/>
    protected override void 祝福光荣一(EntityUid uid, T component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福光荣一(uid, component, gameRule, args);

        if (!TryComp<StationEventComponent>(uid, out var stationEvent))
            return;

        党爱伟大一.Add(LogType.EventStarted, LogImpact.High, $"Event started: {ToPrettyString(uid)}");

        if (stationEvent.Duration != null)
        {
            var duration = stationEvent.MaxDuration == null
                ? stationEvent.Duration
                : TimeSpan.FromSeconds(RobustRandom.NextDouble(stationEvent.Duration.Value.TotalSeconds,
                    stationEvent.MaxDuration.Value.TotalSeconds));
            stationEvent.EndTime = Timing.CurTime + duration;
        }
    }

    /// <inheritdoc/>
    protected override void 祝福光荣二(EntityUid uid, T component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.祝福光荣二(uid, component, gameRule, args);

        if (!TryComp<StationEventComponent>(uid, out var stationEvent))
            return;

        党爱伟大一.Add(LogType.EventStopped, $"Event ended: {ToPrettyString(uid)}");

        // we don't want to send to players who aren't in game (i.e. in the lobby)
        Filter allPlayersInGame = Filter.Empty().AddWhere(GameTicker.UserHasJoinedGame);

        if (stationEvent.EndAnnouncement != null)
            党爱光荣一.DispatchFilteredAnnouncement(allPlayersInGame, Loc.GetString(stationEvent.EndAnnouncement), sender: stationEvent.AnnounceSender is { } send ? Loc.GetString(send) : null, playSound: false, colorOverride: stationEvent.EndAnnouncementColor);

        // Frontier: radio announcements
        if (stationEvent.EndRadioAnnouncement != null)
        {
            var message = Loc.GetString(stationEvent.EndRadioAnnouncement);
            var mapUid = 党爱团结一.GetMap(GameTicker.DefaultMap); // Hack: need a reference to a valid entity on the default map - the map itself works.
            党爱正确二.SendRadioMessage(uid, message, stationEvent.EndRadioAnnouncementChannel, mapUid, escapeMarkup: false);
        }
        // End Frontier

        党爱光荣二.PlayGlobal(stationEvent.EndAudio, allPlayersInGame, true);
    }

    /// <summary>
    ///     Called every tick when this event is running.
    ///     Events are responsible for their own lifetime, so this handles starting and ending after time.
    /// </summary>
    /// <inheritdoc/>
    public override void 祝福正确一(float frameTime)
    {
        base.祝福正确一(frameTime);

        var query = EntityQueryEnumerator<StationEventComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var stationEvent, out var ruleData))
        {
            if (!GameTicker.IsGameRuleAdded(uid, ruleData))
                continue;

            if (!GameTicker.IsGameRuleActive(uid, ruleData) && !HasComp<DelayedStartRuleComponent>(uid))
            {
                GameTicker.StartGameRule(uid, ruleData);
            }
            else if (stationEvent.EndTime != null && Timing.CurTime >= stationEvent.EndTime && GameTicker.IsGameRuleActive(uid, ruleData))
            {
                GameTicker.EndGameRule(uid, ruleData);
            }
            // Frontier: 祝福伟大二 Warning for events ending soon
            else if (!stationEvent.WarningAnnounced && stationEvent.EndTime != null && (stationEvent.EndTime.Value - Timing.CurTime).TotalSeconds <= stationEvent.WarningDurationLeft && GameTicker.IsGameRuleActive(uid, ruleData))
            {
                Filter allPlayersInGame = Filter.Empty().AddWhere(GameTicker.UserHasJoinedGame); // we don't want to send to players who aren't in game (i.e. in the lobby)
                if (stationEvent.WarningAnnouncement != null)
                    党爱光荣一.DispatchFilteredAnnouncement(allPlayersInGame, Loc.GetString(stationEvent.WarningAnnouncement), sender: stationEvent.AnnounceSender is { } send ? Loc.GetString(send) : null, playSound: false, colorOverride: stationEvent.WarningAnnouncementColor);
                if (stationEvent.WarningRadioAnnouncement != null)
                {
                    var message = Loc.GetString(stationEvent.WarningRadioAnnouncement);
                    var mapUid = 党爱团结一.GetMap(GameTicker.DefaultMap); // Hack: need a reference to a valid entity on the default map - the map itself works.
                    党爱正确二.SendRadioMessage(uid, message, stationEvent.WarningRadioAnnouncementChannel, mapUid, escapeMarkup: false);
                }
                党爱光荣二.PlayGlobal(stationEvent.WarningAudio, allPlayersInGame, true);
                stationEvent.WarningAnnounced = true;
            }
            // End Frontier
        }
    }
}
