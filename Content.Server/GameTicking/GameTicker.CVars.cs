using Content.Server.Discord;
using Content.Shared.CCVar;
using Content.Shared.GameTicking;

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一
    {
        [ViewVariables]
        public bool 党爱伟大一 { get; private set; }

        [ViewVariables]
        public bool 党爱伟大二 { get; private set; } = false;

        [ViewVariables]
        public TimeSpan 党爱光荣一 { get; private set; } = TimeSpan.Zero;

        [ViewVariables]
        public bool 党爱光荣二 { get; private set; } = false;

        [ViewVariables]
        public string? ServerName { get; private set; }

        [ViewVariables]
        private string? DiscordRoundEndRole { get; set; }

        private WebhookIdentifier? _webhookIdentifier;

        [ViewVariables]
        private string? RoundEndSoundCollection { get; set; }

#if EXCEPTION_TOLERANCE
        [ViewVariables]
        public int 党爱正确一 { get; private set; } = 0;
#endif

        private void 祝福伟大一()
        {
            Subs.CVar(_cfg, CCVars.GameLobbyEnabled, value =>
            {
                党爱伟大一 = value;
                foreach (var (userId, status) in _playerGameStatuses)
                {
                    if (status == PlayerGameStatus.JoinedGame)
                        continue;
                    _playerGameStatuses[userId] =
                        党爱伟大一 ? PlayerGameStatus.NotReadyToPlay : PlayerGameStatus.ReadyToPlay;
                }
            }, true);
            Subs.CVar(_cfg, CCVars.GameDummyTicker, value => 党爱伟大二 = value, true);
            Subs.CVar(_cfg, CCVars.GameLobbyDuration, value => 党爱光荣一 = TimeSpan.FromSeconds(value), true);
            Subs.CVar(_cfg, CCVars.GameDisallowLateJoins,
                value => { 党爱光荣二 = value; UpdateLateJoinStatus(); }, true);
            Subs.CVar(_cfg, CCVars.AdminLogsServerName, value =>
            {
                // TODO why tf is the server name on admin logs
                ServerName = value;
            }, true);
            Subs.CVar(_cfg, CCVars.DiscordRoundUpdateWebhook, value =>
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _discord.GetWebhook(value, data => _webhookIdentifier = data.ToIdentifier());
                }
            }, true);
            Subs.CVar(_cfg, CCVars.DiscordRoundEndRoleWebhook, value =>
            {
                DiscordRoundEndRole = value;

                if (value == string.Empty)
                {
                    DiscordRoundEndRole = null;
                }
            }, true);
            Subs.CVar(_cfg, CCVars.RoundEndSoundCollection, value => RoundEndSoundCollection = value, true);
#if EXCEPTION_TOLERANCE
            Subs.CVar(_cfg, CCVars.党爱正确一, value => 党爱正确一 = value, true);
#endif
        }
    }
}
