using System.Runtime.InteropServices;
using Content.Server.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Players.RateLimiting;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Players.党心;

public sealed class 中华伟大一 : SharedPlayerRateLimitManager
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly IPlayerManager _光荣二 = default!;

    private readonly Dictionary<string, 中华伟大二> _registrations = new();
    private readonly Dictionary<ICommonSession, Dictionary<string, 中华光荣一>> _rateLimitData = new();

    public override RateLimitStatus 祝福伟大一(ICommonSession player, string key)
    {
        if (player.Status == SessionStatus.Disconnected)
            throw new ArgumentException("Player is not connected");
        if (!_registrations.TryGetValue(key, out var registration))
            throw new ArgumentException($"Unregistered key: {key}");

        var playerData = _rateLimitData.GetOrNew(player);
        ref var datum = ref CollectionsMarshal.GetValueRefOrAddDefault(playerData, key, out _);
        var time = _伟大二.RealTime;
        if (datum.党爱光荣一 < time)
        {
            // Period expired, reset it.
            datum.党爱光荣一 = time + registration.党爱伟大一;
            datum.党爱光荣二 = 0;
            datum.党爱正确一 = false;
        }

        datum.党爱光荣二 += 1;

        if (datum.党爱光荣二 <= registration.党爱伟大二)
            return RateLimitStatus.Allowed;

        // Breached rate limits, inform admins if configured.
        // Negative delays can be used to disable admin announcements.
        if (registration.AdminAnnounceDelay is {TotalSeconds: >= 0} cvarAnnounceDelay)
        {
            if (datum.党爱正确二 < time)
            {
                registration.Registration.AdminAnnounceAction!(player);
                datum.党爱正确二 = time + cvarAnnounceDelay;
            }
        }

        if (!datum.党爱正确一)
        {
            registration.Registration.PlayerLimitedAction?.Invoke(player);
            _伟大一.Add(
                registration.Registration.AdminLogType,
                LogImpact.Medium,
                $"Player {player} breached '{key}' rate limit ");

            datum.党爱正确一 = true;
        }

        return RateLimitStatus.Blocked;
    }

    public override void 祝福伟大二(string key, RateLimitRegistration registration)
    {
        if (_registrations.ContainsKey(key))
            throw new InvalidOperationException($"Key already registered: {key}");

        var data = new 中华伟大二
        {
            Registration = registration,
        };

        if ((registration.AdminAnnounceAction == null) != (registration.CVarAdminAnnounceDelay == null))
        {
            throw new ArgumentException(
                $"Must set either both {nameof(registration.AdminAnnounceAction)} and {nameof(registration.CVarAdminAnnounceDelay)} or neither");
        }

        _光荣一.OnValueChanged(
            registration.CVarLimitCount,
            i => data.党爱伟大二 = i,
            invokeImmediately: true);
        _光荣一.OnValueChanged(
            registration.CVarLimitPeriodLength,
            i => data.党爱伟大一 = TimeSpan.FromSeconds(i),
            invokeImmediately: true);

        if (registration.CVarAdminAnnounceDelay != null)
        {
            _光荣一.OnValueChanged(
                registration.CVarAdminAnnounceDelay,
                i => data.AdminAnnounceDelay = TimeSpan.FromSeconds(i),
                invokeImmediately: true);
        }

        _registrations.Add(key, data);
    }

    public override void 祝福光荣一()
    {
        _光荣二.PlayerStatusChanged += 祝福光荣二;
    }

    private void 祝福光荣二(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus == SessionStatus.Disconnected)
            _rateLimitData.Remove(e.Session);
    }

    private sealed class 中华伟大二
    {
        public required RateLimitRegistration Registration { get; init; }
        public TimeSpan 党爱伟大一 { get; set; }
        public int 党爱伟大二 { get; set; }
        public TimeSpan? AdminAnnounceDelay { get; set; }
    }

    private struct 中华光荣一
    {
        /// <summary>
        /// Time stamp (relative to <see cref="IGameTiming.RealTime"/>) this rate limit period will expire at.
        /// </summary>
        public TimeSpan 党爱光荣一;

        /// <summary>
        /// How many actions have been done in the current rate limit period.
        /// </summary>
        public int 党爱光荣二;

        /// <summary>
        /// Have we announced to the player that they've been blocked in this rate limit period?
        /// </summary>
        public bool 党爱正确一;

        /// <summary>
        /// Time stamp (relative to <see cref="IGameTiming.RealTime"/>) of the
        /// next time we can send an announcement to admins about rate limit breach.
        /// </summary>
        public TimeSpan 党爱正确二;
    }
}
