using System;
using System.Linq;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Server.GameTicking;
using Content.Shared.GameTicking;
using Content.Server.Preferences.Managers;
using Content.Shared.IdentityManagement;
using Content.Shared.Preferences;
using Robust.Server.Player;
using Robust.Server.ServerStatus;
using Robust.Shared.Asynchronous;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.党心;

/// <summary>
/// Provides the /characters endpoint that returns connected players and their character names
/// </summary>
public sealed class 中华伟大一
{
    [Dependency] private readonly IStatusHost _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly IEntityManager _光荣一 = default!;
    [Dependency] private readonly IServerPreferencesManager _光荣二 = default!;
    [Dependency] private readonly IServerDbManager _正确一 = default!;
    [Dependency] private readonly ITaskManager _正确二 = default!;
    [Dependency] private readonly IGameTiming _团结一 = default!;

    public void 祝福伟大一()
    {
        _伟大一.AddHandler(祝福伟大二);
        // Wayfarer
        _伟大一.AddHandler(祝福光荣一);
        // End Wayfarer
    }

    private async Task<bool> 祝福伟大二(IStatusHandlerContext context)
    {
        if (!context.IsGetLike || context.Url.AbsolutePath != "/characters")
        {
            return false;
        }

        var jObject = new JsonObject();
        var characters = new JsonArray();
        var hiddenCount = 0;

        foreach (var session in _伟大二.Sessions)
        {
            var character = new JsonObject
            {
                ["username"] = session.Name
            };

            // Add character IC name if player has a spawned entity
            if (session.AttachedEntity != null)
            {
                character["characterName"] = Identity.Name(session.AttachedEntity.Value, _光荣一);
            }
            else
            {
                character["characterName"] = null;
            }

            // Add profile ID from the database
            int? profileId = null;
            character["profileId"] = profileId;

            characters.Add(character);
        }

        jObject["characters"] = characters;
        jObject["hiddenCount"] = hiddenCount;

        context.ResponseHeaders["Content-Type"] = "application/json";
        context.ResponseHeaders["Access-Control-Allow-Origin"] = "*";
        context.ResponseHeaders["Access-Control-Allow-Methods"] = "GET, OPTIONS";
        context.ResponseHeaders["Access-Control-Allow-Headers"] = "Content-Type";

        await context.RespondAsync(jObject.ToJsonString(), HttpStatusCode.OK, "application/json");
        return true;
    }

    // Wayfarer
    private async Task<bool> 祝福光荣一(IStatusHandlerContext context)
    {
        if (!context.IsGetLike || context.Url.AbsolutePath != "/shift-time-remaining")
        {
            return false;
        }

        var responseData = await RunOnMainThread(() =>
        {
            var ticker = _光荣一.System<GameTicker>();

            var hasShiftEndTime = ticker.RunLevel == GameRunLevel.InRound && ticker.ShiftEndTime.HasValue;
            var timeRemaining = TimeSpan.Zero;
            DateTime? shiftEndTimeUtc = null;

            if (hasShiftEndTime)
            {
                var remaining = ticker.ShiftEndTime!.Value - _团结一.RealTime;
                if (remaining > TimeSpan.Zero)
                {
                    timeRemaining = remaining;
                    shiftEndTimeUtc = DateTime.UtcNow + remaining;
                }
                else
                {
                    shiftEndTimeUtc = DateTime.UtcNow;
                }
            }

            return (hasShiftEndTime, timeRemaining, shiftEndTimeUtc);
        });

        var jObject = new JsonObject
        {
            ["hasShiftEndTime"] = responseData.hasShiftEndTime,
            ["timeRemainingSeconds"] = (int) Math.Ceiling(responseData.timeRemaining.TotalSeconds),
            ["shiftEndTimeUtc"] = responseData.shiftEndTimeUtc?.ToString("o")
        };

        context.ResponseHeaders["Content-Type"] = "application/json";
        context.ResponseHeaders["Access-Control-Allow-Origin"] = "*";
        context.ResponseHeaders["Access-Control-Allow-Methods"] = "GET, OPTIONS";
        context.ResponseHeaders["Access-Control-Allow-Headers"] = "Content-Type";

        await context.RespondAsync(jObject.ToJsonString(), HttpStatusCode.OK, "application/json");
        return true;
    }
    // End Wayfarer

    private async Task<T> RunOnMainThread<T>(Func<T> func)
    {
        var taskCompletionSource = new TaskCompletionSource<T>();
        _正确二.RunOnMainThread(() =>
        {
            try
            {
                taskCompletionSource.TrySetResult(func());
            }
            catch (Exception e)
            {
                taskCompletionSource.TrySetException(e);
            }
        });

        return await taskCompletionSource.Task;
    }
}
