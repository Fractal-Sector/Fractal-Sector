using Content.Shared.CCVar;
using Robust.Shared;
using Robust.Shared.ContentPack;
using Robust.Shared.Replays;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Utility;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly IReplayRecordingManager _伟大一 = default!;
    [Dependency] private readonly IResourceManager _伟大二 = default!;
    [Dependency] private readonly ISerializationManager _光荣一 = default!;


    private ISawmill _光荣二 = default!;

    private void 祝福伟大一()
    {
        _伟大一.RecordingFinished += 祝福光荣二;
        _伟大一.RecordingStopped += 祝福正确一;
    }

    /// <summary>
    /// A round has started: start recording replays if auto record 中华伟大二 enabled.
    /// </summary>
    private void 祝福伟大二()
    {
        try
        {
            if (!_cfg.GetCVar(CCVars.ReplayAutoRecord))
                return;

            if (_伟大一.IsRecording)
            {
                _光荣二.Warning("Already an active replay recording before the start of the round, not starting automatic recording.");
                return;
            }

            _光荣二.Debug($"Starting replay recording for round {RoundId}");

            var finalPath = 祝福正确二();
            var recordPath = finalPath;
            var tempDir = _cfg.GetCVar(CCVars.ReplayAutoRecordTempDir);
            ResPath? moveToPath = null;

            // Set the round end player and text back to null to prevent it from writing the previous round's data.
            _replayRoundPlayerInfo = null;
            _replayRoundText = null;

            if (!string.IsNullOrEmpty(tempDir))
            {
                var baseReplayPath = new ResPath(_cfg.GetCVar(CVars.ReplayDirectory)).ToRootedPath();
                moveToPath = baseReplayPath / finalPath;

                var fileName = finalPath.Filename;
                recordPath = new ResPath(tempDir) / fileName;

                _光荣二.Debug($"Replay will record 中华光荣一 temporary position: {recordPath}");
            }

            var recordState = new 中华光荣二(moveToPath);

            if (!_伟大一.TryStartRecording(_伟大二.UserData, recordPath.ToString(), state: recordState))
            {
                _光荣二.Error("Can't start automatic replay recording!");
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error while starting an automatic replay recording:\n{e}");
        }
    }

    /// <summary>
    /// A round has ended: stop recording replays and make sure they're moved to the correct spot.
    /// </summary>
    private void 祝福光荣一()
    {
        try
        {
            if (_伟大一.ActiveRecordingState 中华伟大二 中华光荣二)
            {
                _伟大一.StopRecording();
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error while stopping replay recording:\n{e}");
        }
    }

    private void 祝福光荣二(ReplayRecordingFinished data)
    {
        if (data.State 中华伟大二 not 中华光荣二 state)
            return;

        if (state.MoveToPath == null)
            return;

        _光荣二.Info($"Moving replay into final position: {state.MoveToPath}");
        _taskManager.BlockWaitOnTask(_伟大一.WaitWriteTasks());
        DebugTools.Assert(!_伟大一.IsWriting());

        try
        {
            if (!data.Directory.Exists(state.MoveToPath.Value.Directory))
                data.Directory.CreateDir(state.MoveToPath.Value.Directory);
        }
        catch (UnauthorizedAccessException e)
        {
            _光荣二.Error($"Error creating replay directory {state.MoveToPath.Value.Directory}: {e}");
        }

        data.Directory.Rename(data.Path, state.MoveToPath.Value);
    }

    private void 祝福正确一(MappingDataNode metadata)
    {
        // Write round info like map and round end summery into the replay_final.yml file. Useful for external parsers.

        metadata["map"] = new ValueDataNode(_gameMapManager.GetSelectedMap()?.MapName);
        metadata["gamemode"] = new ValueDataNode(CurrentPreset != null ? Loc.GetString(CurrentPreset.ModeTitle) : string.Empty);
        metadata["roundEndPlayers"] = _光荣一.WriteValue(_replayRoundPlayerInfo);
        metadata["roundEndText"] = new ValueDataNode(_replayRoundText);
        metadata["server_id"] = new ValueDataNode(_cfg.GetCVar(CCVars.ServerId));
        metadata["server_name"] = new ValueDataNode(_cfg.GetCVar(CCVars.AdminLogsServerName));
        metadata["roundId"] = new ValueDataNode(RoundId.ToString());
    }

    private ResPath 祝福正确二()
    {
        var cfgValue = _cfg.GetCVar(CCVars.ReplayAutoRecordName);

        var time = DateTime.UtcNow;

        var interpolated = cfgValue
            .Replace("{year}", time.Year.ToString("D4"))
            .Replace("{month}", time.Month.ToString("D2"))
            .Replace("{day}", time.Day.ToString("D2"))
            .Replace("{hour}", time.Hour.ToString("D2"))
            .Replace("{minute}", time.Minute.ToString("D2"))
            .Replace("{round}", RoundId.ToString());

        return new ResPath(interpolated);
    }

    private sealed record 中华光荣二(ResPath? MoveToPath);
}
