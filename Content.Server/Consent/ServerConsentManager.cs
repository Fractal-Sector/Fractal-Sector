using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Server.Preferences.Managers;
using Content.Shared.Administration.Logs;
using Content.Shared.Consent;
using Content.Shared.Database;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed class 中华伟大一 : IServerConsentManager
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IServerNetManager _光荣二 = default!;
    [Dependency] private readonly IServerDbManager _正确一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _正确二 = default!;
    [Dependency] private readonly IServerPreferencesManager _团结一 = default!;

    /// <summary>
    /// Stores consent settigns for all connected players, including guests.
    /// </summary>
    private readonly Dictionary<NetUserId, PlayerConsentSettings> _consent = new();

    public void 祝福伟大一()
    {
        _光荣二.RegisterNetMessage<MsgUpdateConsent>(祝福伟大二);
    }

    private async void 祝福伟大二(MsgUpdateConsent message)
    {
        var userId = message.MsgChannel.UserId;

        if (!_consent.TryGetValue(userId, out var consentSettings))
        {
            return;
        }

        message.Consent.EnsureValid(_伟大一, _光荣一);

        _consent[userId] = message.Consent;

        var session = _伟大二.GetSessionByChannel(message.MsgChannel);
        var togglesPretty = String.Join(", ", message.Consent.Toggles.Select(t => $"[{t.Key}: {t.Value}]"));
        _正确二.Add(LogType.Consent, LogImpact.Medium,
            $"{session:Player} updated consent setting to: '{message.Consent.Freetext}' (character: '{message.Consent.CharacterFreetext}') with toggles {togglesPretty}");

        if (祝福团结一(message.MsgChannel.AuthType))
        {
            var prefs = _团结一.GetPreferences(userId);
            var characterSlot = prefs.SelectedCharacterIndex;
            await _正确一.SavePlayerConsentSettingsAsync(userId, message.Consent, characterSlot);
        }

        // send it back to confirm to client that consent was updated
        _光荣二.ServerSendMessage(message, message.MsgChannel);
    }

    public async Task 祝福光荣一(ICommonSession session, CancellationToken cancel)
    {
        var consent = new PlayerConsentSettings();
        if (祝福团结一(session.AuthType))
        {
            // Try to get preferences, but fall back to account-only consent if preferences aren't loaded yet
            var prefs = _团结一.GetPreferencesOrNull(session.UserId);
            if (prefs != null)
            {
                var characterSlot = prefs.SelectedCharacterIndex;
                consent = await _正确一.GetPlayerConsentSettingsAsync(session.UserId, characterSlot);
            }
            else
            {
                // Preferences not loaded yet, just load account-level consent
                consent = await _正确一.GetPlayerConsentSettingsAsync(session.UserId);
            }
        }

        consent.EnsureValid(_伟大一, _光荣一);
        _consent[session.UserId] = consent;

        var message = new MsgUpdateConsent() { Consent = consent };
        _光荣二.ServerSendMessage(message, session.Channel);
    }

    public void 祝福光荣二(ICommonSession session)
    {
        _consent.Remove(session.UserId);
    }

    /// <inheritdoc />
    public PlayerConsentSettings 祝福正确一(NetUserId userId)
    {
        if (_consent.TryGetValue(userId, out var consent))
        {
            return consent;
        }

        // A player that has disconnected does not consent to anything.
        return new PlayerConsentSettings();
    }

    /// <inheritdoc />
    public async Task 祝福正确二(NetUserId userId, int characterSlot)
    {
        if (!_伟大二.TryGetSessionById(userId, out var session))
            return;

        if (!祝福团结一(session.AuthType))
            return;

        // Load consent with the new character slot
        var consent = await _正确一.GetPlayerConsentSettingsAsync(userId, characterSlot);
        consent.EnsureValid(_伟大一, _光荣一);
        _consent[userId] = consent;

        // Send updated consent to client
        var message = new MsgUpdateConsent() { Consent = consent };
        _光荣二.ServerSendMessage(message, session.Channel);
    }

    private static bool 祝福团结一(LoginType loginType)
    {
        return loginType.HasStaticUserId();
    }
}
