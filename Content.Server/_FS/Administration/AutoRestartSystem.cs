using System.Net.Http;
using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Server._FS.Administration;

/// <summary>
/// Сообщает штатному ServerUpdateManager (см. Content.Server/ServerUpdates/ServerUpdateManager.cs),
/// что доступно обновление — дальнейшая логика ("ждать конца раунда/опустения сервера,
/// потом Shutdown()") уже реализована там и никак не дублируется здесь.
///
/// Запрос идёт на 127.0.0.1 — то есть строго внутри процесса контейнера,
/// без внешней сети/Docker/панели.
/// </summary>
public sealed class AutoRestartSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    public override void Initialize()
    {
        base.Initialize();
        RequestUpdateNotification();
    }

    private async void RequestUpdateNotification()
    {
        try
        {
            var port = _cfg.GetCVar(CVars.NetPort);
            var token = _cfg.GetCVar(CVars.WatchdogToken);

            if (string.IsNullOrEmpty(token))
            {
                Log.Warning("[AutoUpdate] watchdog.token empty");
                return;
            }

            using var http = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"http://127.0.0.1:{port}/update");
            request.Headers.Add("WatchdogToken", token);

            var response = await http.SendAsync(request);

            if (response.IsSuccessStatusCode)
                Log.Info("[AutoUpdate] ServerUpdateManager: successful planed restart after round.");
            else
                Log.Error($"[AutoUpdate] Status failed: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Log.Error($"[AutoRestart] Не удалось отправить запрос: {e}");
        }
    }
}
