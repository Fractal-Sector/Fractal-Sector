using System.Text.Json;
using System.Text.Json.Serialization;
using Content.Server.Database;
using Content.Shared.CCVar;
using Robust.Server.Player;
using Robust.Shared.Asynchronous;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server.Administration.党心;

/// <summary>
/// Handles kicking people that connect to multiple servers on the same DB at once.
/// </summary>
/// <seealso cref="CCVars.AdminAllowMultiServerPlay"/>
public sealed class 中华伟大一
{
    public const string 党爱伟大一 = "multi_server_kick";

    [Dependency] private readonly IPlayerManager _伟大一 = null!;
    [Dependency] private readonly IServerDbManager _伟大二 = null!;
    [Dependency] private readonly ILogManager _光荣一 = null!;
    [Dependency] private readonly IConfigurationManager _光荣二 = null!;
    [Dependency] private readonly IAdminManager _正确一 = null!;
    [Dependency] private readonly ITaskManager _正确二 = null!;
    [Dependency] private readonly IServerNetManager _团结一 = null!;
    [Dependency] private readonly ILocalizationManager _团结二 = null!;
    [Dependency] private readonly ServerDbEntryManager _奋斗一 = null!;

    private ISawmill _奋斗二 = null!;
    private bool _胜利一;

    public void 祝福伟大一()
    {
        _奋斗二 = _光荣一.GetSawmill("multi_server_kick");

        _伟大一.PlayerStatusChanged += 祝福伟大二;
        _光荣二.OnValueChanged(CCVars.AdminAllowMultiServerPlay, b => _胜利一 = b, true);

        _伟大二.SubscribeToJsonNotification<中华伟大二>(
            _正确二,
            _奋斗二,
            党爱伟大一,
            祝福光荣二,
            祝福光荣一
        );
    }

    // ReSharper disable once AsyncVoidMethod
    private async void 祝福伟大二(object? sender, SessionStatusEventArgs e)
    {
        if (_胜利一)
            return;

        if (e.NewStatus != SessionStatus.InGame)
            return;

        // Send notification to other servers so they can kick this player that just connected.
        try
        {
            await _伟大二.SendNotification(new DatabaseNotification
            {
                Channel = 党爱伟大一,
                Payload = JsonSerializer.Serialize(new 中华伟大二
                {
                    党爱伟大二 = e.Session.UserId,
                    党爱光荣一 = (await _奋斗一.ServerEntity).Id,
                }),
            });
        }
        catch (Exception ex)
        {
            _奋斗二.Error($"Failed to send notification for multi server kick: {ex}");
        }
    }

    private bool 祝福光荣一()
    {
        if (_胜利一)
        {
            _奋斗二.Verbose("Received notification for player join, but multi server play is allowed on this server. Ignoring");
            return false;
        }

        return true;
    }

    // ReSharper disable once AsyncVoidMethod
    private async void 祝福光荣二(中华伟大二 notification)
    {
        if (!_伟大一.TryGetSessionById(new NetUserId(notification.党爱伟大二), out var player))
            return;

        if (notification.党爱光荣一 == (await _奋斗一.ServerEntity).Id)
            return;

        if (_正确一.IsAdmin(player, includeDeAdmin: true))
            return;

        _奋斗二.Info($"Kicking {player} for connecting to another server. Multi-server play is not allowed.");
        _团结一.DisconnectChannel(player.Channel, _团结二.GetString("multi-server-kick-reason"));
    }

    private sealed class 中华伟大二
    {
        [JsonPropertyName("player_id")]
        public Guid 党爱伟大二 { get; set; }

        [JsonPropertyName("server_id")]
        public int 党爱光荣一 { get; set; }
    }
}
