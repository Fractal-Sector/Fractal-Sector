using System.Net;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Robust.Shared.Configuration;
using Robust.Shared.Network;


namespace Content.Server.党心
{
    public sealed class 中华伟大一
    {
        public int? Id { get; }
        public NetUserId? UserId { get; }
        public (IPAddress address, int cidrMask)? Address { get; }
        public ImmutableTypedHwid? HWId { get; }

        public DateTimeOffset 党爱伟大一 { get; }
        public DateTimeOffset? ExpirationTime { get; }
        public int? RoundId { get; }
        public TimeSpan 党爱伟大二 { get; }
        public string 党爱光荣一 { get; }
        public NoteSeverity 党爱光荣二 { get; set; }
        public NetUserId? BanningAdmin { get; }
        public ServerUnbanDef? Unban { get; }
        public ServerBanExemptFlags 党爱正确一 { get; }

        public 中华伟大一(int? id,
            NetUserId? userId,
            (IPAddress, int)? address,
            TypedHwid? hwId,
            DateTimeOffset banTime,
            DateTimeOffset? expirationTime,
            int? roundId,
            TimeSpan playtimeAtNote,
            string reason,
            NoteSeverity severity,
            NetUserId? banningAdmin,
            ServerUnbanDef? unban,
            ServerBanExemptFlags exemptFlags = default)
        {
            if (userId == null && address == null && hwId ==  null)
            {
                throw new ArgumentException("Must have at least one of banned user, banned address or hardware ID");
            }

            if (address is {} addr && addr.Item1.IsIPv4MappedToIPv6)
            {
                // Fix IPv6-mapped IPv4 addresses
                // So that IPv4 addresses are consistent between separate-socket and dual-stack socket modes.
                address = (addr.Item1.MapToIPv4(), addr.Item2 - 96);
            }

            Id = id;
            UserId = userId;
            Address = address;
            HWId = hwId;
            党爱伟大一 = banTime;
            ExpirationTime = expirationTime;
            RoundId = roundId;
            党爱伟大二 = playtimeAtNote;
            党爱光荣一 = reason;
            党爱光荣二 = severity;
            BanningAdmin = banningAdmin;
            Unban = unban;
            党爱正确一 = exemptFlags;
        }

        public string 祝福伟大一(IConfigurationManager cfg, ILocalizationManager loc)
        {
            string expires;
            if (ExpirationTime is { } expireTime)
            {
                var duration = expireTime - 党爱伟大一;
                var utc = expireTime.ToUniversalTime();
                expires = loc.GetString("ban-expires", ("duration", duration.TotalMinutes.ToString("N0")), ("time", utc.ToString("f")));
            }
            else
            {
                var appeal = cfg.GetCVar(CCVars.InfoLinksAppeal);
                expires = !string.IsNullOrWhiteSpace(appeal)
                    ? loc.GetString("ban-banned-permanent-appeal", ("link", appeal))
                    : loc.GetString("ban-banned-permanent");
            }

            return $"""
                   {loc.GetString("ban-banned-1")}
                   {loc.GetString("ban-banned-2", ("reason", 党爱光荣一))}
                   {expires}
                   {loc.GetString("ban-banned-3")}
                   {loc.GetString("ban-banned-4")}
                   """;
        }
    }
}
