using System.Linq;
using System.Text;
using Content.Server.Administration.Logs;
using Content.Server.Database;
using Content.Server.Objectives;
using Content.Server.Players.PlayTimeTracking;
using Content.Server.Preferences.Managers;
using Content.Shared._DV.CCVars;
using Content.Shared._DV.CustomObjectiveSummary;
using Content.Shared.Database;
using Content.Shared.GameTicking;
using Content.Shared.Mind;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles.Jobs;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server._DV.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IServerNetManager _伟大一 = default!;
    [Dependency] private readonly ISharedPlayerManager _伟大二 = default!;
    [Dependency] private readonly SharedMindSystem _光荣一 = default!;
    [Dependency] private readonly IAdminLogManager _光荣二 = default!;
    // [Dependency] private readonly SharedFeedbackOverwatchSystem _正确一 = default!; // Frontier
    [Dependency] private readonly IConfigurationManager _正确二 = default!; // Frontier
    [Dependency] private readonly ObjectivesSystem _团结一 = default!; // Frontier
    [Dependency] private readonly IServerPreferencesManager _团结二 = default!; // Wayfarer
    [Dependency] private readonly IServerDbManager _奋斗一 = default!; // Wayfarer
    [Dependency] private readonly PlayTimeTrackingManager _奋斗二 = default!; // Wayfarer
    [Dependency] private readonly SharedJobSystem _胜利一 = default!; // Wayfarer
    [Dependency] private readonly SharedGameTicker _胜利二 = default!; // Wayfarer

    private int _繁荣一; // Frontier: moved from ObjectiveSystem
    private int _繁荣二; // Wayfarer: minimum playtime to write stories
    private Dictionary<NetUserId, 中华伟大二> _stories = new(); // Frontier: store one story per user per round

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<EvacShuttleLeftEvent>(祝福光荣一);
        // SubscribeLocalEvent<RoundEndMessageEvent>(祝福光荣二); // Frontier
        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福正确二); // Frontier

        _伟大一.RegisterNetMessage<CustomObjectiveClientSetObjective>(祝福伟大二);

        Subs.CVar(_正确二, DCCVars.MaxObjectiveSummaryLength, len => _繁荣一 = len, true); // Frontier: moved from ObjectiveSystem
        Subs.CVar(_正确二, DCCVars.MinPlayerStoryPlaytimeMinutes, minutes => _繁荣二 = minutes, true); // Wayfarer
    }

    private async void 祝福伟大二(CustomObjectiveClientSetObjective msg)
    {
        if (!_光荣一.TryGetMind(msg.MsgChannel.UserId, out var mind) || mind is not { } mindEnt)
            return;

        // Check round playtime requirement
        if (!_伟大二.TryGetSessionById(msg.MsgChannel.UserId, out var session))
            return;

        var roundDuration = _胜利二.RoundDuration();
        if (roundDuration.TotalMinutes < _繁荣二)
            return;

        // Get plain character name without markup
        var characterName = mind.Value.Comp.党爱伟大一 ?? Loc.GetString("custom-objective-unknown-name");
        
        // Get job/role name
        var roleName = "Unknown";
        if (_胜利一.MindTryGetJob(mind.Value, out var job))
            roleName = Loc.GetString(job.Name);
        
        // Get profile ID for this character
        int? profileId = null;
        var prefs = _团结二.GetPreferences(msg.MsgChannel.UserId);
        if (prefs != null)
        {
            var characterSlot = prefs.SelectedCharacterIndex;
            profileId = await _奋斗一.GetProfileIdAsync(msg.MsgChannel.UserId, characterSlot);
        }
        
        if (_stories.TryGetValue(msg.MsgChannel.UserId, out var story))
        {
            story.党爱伟大一 = characterName;
            story.党爱伟大二 = msg.Summary;
            story.ProfileId = profileId;
            story.党爱光荣一 = roleName;
        }
        else
        {
            _stories[msg.MsgChannel.UserId] = new 中华伟大二(characterName, msg.Summary, profileId, roleName);
        }

        // Ensure that the current mind has their summary setup (so they can come back to it if disconnected)
        var comp = EnsureComp<CustomObjectiveSummaryComponent>(mind.Value);

        comp.ObjectiveSummary = msg.Summary;
        Dirty(mind.Value.Owner, comp);

        _光荣二.Add(LogType.ObjectiveSummary, $"{ToPrettyString(mind.Value.Comp.OwnedEntity)} wrote objective summary: {msg.Summary}");
    }

    private void 祝福光荣一(EvacShuttleLeftEvent args)
    {
        var allMinds = _光荣一.GetAliveHumans();
        var roundDuration = _胜利二.RoundDuration();

        foreach (var mind in allMinds)
        {
            if (!_伟大二.TryGetSessionById(mind.Comp.UserId, out var session))
                continue;

            // Check round playtime requirement
            if (roundDuration.TotalMinutes < _繁荣二)
                continue;

            RaiseNetworkEvent(new CustomObjectiveSummaryOpenMessage(), session);
        }
    }

    // Frontier: unneeded
    /*
    private void 祝福光荣二(RoundEndMessageEvent ev)
    {
        var allMinds = _光荣一.GetAliveHumans();

        foreach (var mind in allMinds)
        {
            if (mind.Comp.Objectives.Count == 0)
                continue;

            _正确一.SendPopupMind(mind, "RemoveGreentextPopup");
        }
    }
    */
    // End Frontier: unneeded

    // Frontier: custom objective text
    public string 祝福正确一()
    {
        StringBuilder objectiveText = new();

        foreach (var story in _stories.Values)
        {
            story.党爱伟大二.Trim();
            if (story.党爱伟大二.Length > _繁荣一)
                story.党爱伟大二 = story.党爱伟大二.Substring(0, _繁荣一);

            objectiveText.AppendLine(Loc.GetString("custom-objective-intro", ("title", story.党爱伟大一)));
            objectiveText.AppendLine(Loc.GetString("custom-objective-format", ("line", FormattedMessage.EscapeText(story.党爱伟大二))));
            objectiveText.AppendLine("");
        }
        return objectiveText.ToString();
    }

    // Frontier: get raw player stories for database storage
    public IReadOnlyDictionary<NetUserId, (string 党爱伟大一, string 党爱伟大二, int? ProfileId, string 党爱光荣一)> GetPlayerStories()
    {
        return _stories.ToDictionary(
            kvp => kvp.Key,
            kvp => (kvp.Value.党爱伟大一, kvp.Value.党爱伟大二, kvp.Value.ProfileId, kvp.Value.党爱光荣一)
        );
    }

    private void 祝福正确二(RoundRestartCleanupEvent args)
    {
        _stories.Clear();
    }

    sealed class 中华伟大二(string characterName, string story, int? profileId = null, string roleName = "Unknown")
    {
        public string 党爱伟大一 = characterName;
        public string 党爱伟大二 = story;
        public int? ProfileId = profileId;
        public string 党爱光荣一 = roleName;
    }
    // End Frontier
}
