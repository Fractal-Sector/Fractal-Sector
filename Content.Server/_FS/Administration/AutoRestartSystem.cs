using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.GameTicking;
using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Server._FS.Administration;

/// <summary>
/// Upon round end, notifies the server's own Status API /update
/// endpoint, triggering the existing ServerUpdateManager shutdown-after-round flow.
/// We deliberately do NOT call this at server Initialize() anymore: if we did,
/// ServerUpdateManager would restart as soon as the server goes empty (its "or
/// the server to empty" fallback), even if that happens between rounds in the
/// lobby. By waiting for GameRunLevel.PostRound, the round is already over by
/// the time we notify, so the restart only ever happens right after a round ends.
/// </summary>
public sealed class AutoRestartSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);
    private const int MaxAttempts = 5;
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(3);

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GameRunLevelChangedEvent>(OnRunLevelChanged);
    }

    private void OnRunLevelChanged(GameRunLevelChangedEvent args)
    {
        if (args.New != GameRunLevel.PostRound)
            return;

        _ = RequestRestartAfterThisRound();
    }

    public async Task RequestRestartAfterThisRound()
    {
        var port = _cfg.GetCVar(CVars.NetPort);
        var token = _cfg.GetCVar(CVars.WatchdogToken);

        if (string.IsNullOrEmpty(token))
        {
            Log.Warning("[AutoUpdate] watchdog.token empty");
            return;
        }

        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                using var http = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, $"http://127.0.0.1:{port}/update");
                request.Headers.Add("WatchdogToken", token);

                using var cts = new CancellationTokenSource(RequestTimeout);
                var response = await http.SendAsync(request, cts.Token);

                if (response.IsSuccessStatusCode)
                {
                    Log.Info("[AutoUpdate] A restart is scheduled after the end of the current round.");
                    return;
                }

                Log.Error($"[AutoUpdate] Request rejected, status: {response.StatusCode}");
                return;
            }
            catch (HttpRequestException e) when (attempt < MaxAttempts)
            {
                Log.Warning($"[AutoUpdate] Attempt {attempt}/{MaxAttempts} failed ({e.Message}), retrying in {RetryDelay.TotalSeconds}s");
                await Task.Delay(RetryDelay);
            }
            catch (OperationCanceledException)
            {
                Log.Error($"[AutoUpdate] The request to the local API timed out ({RequestTimeout.TotalSeconds} sec).");
                return;
            }
            catch (Exception e)
            {
                Log.Error($"[AutoUpdate] Failed: {e}");
                return;
            }

        }
    }
}
