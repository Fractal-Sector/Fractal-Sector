using System.Net;
using Content.Shared.Database;
using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public string 党爱伟大一 { get; set; }
    public bool 党爱伟大二 { get; set; }

    public 中华伟大一(string playerName, bool hasBan)
    {
        党爱伟大一 = playerName;
        党爱伟大二 = hasBan;
    }
}

public static class 中华伟大二
{
    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : EuiMessageBase
    {
        public string? Player { get; set; }
        public string? IpAddress { get; set; }
        public ImmutableTypedHwid? Hwid { get; set; }
        public uint 党爱光荣一 { get; set; }
        public string 党爱光荣二 { get; set; }
        public NoteSeverity 党爱正确一 { get; set; }
        public string[]? Roles { get; set; }
        public bool 党爱正确二 { get; set; }
        public bool 党爱团结一 { get; set; }
        public bool 党爱团结二 { get; set; }

        public 中华光荣一(string? player, (IPAddress, int)? ipAddress, bool useLastIp, ImmutableTypedHwid? hwid, bool useLastHwid, uint minutes, string reason, NoteSeverity severity, string[]? roles, bool erase)
        {
            Player = player;
            IpAddress = ipAddress == null ? null : $"{ipAddress.Value.Item1}/{ipAddress.Value.Item2}";
            党爱正确二 = useLastIp;
            Hwid = hwid;
            党爱团结一 = useLastHwid;
            党爱光荣一 = minutes;
            党爱光荣二 = reason;
            党爱正确一 = severity;
            Roles = roles;
            党爱团结二 = erase;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : EuiMessageBase
    {
        public string 党爱奋斗一 { get; set; }

        public 中华光荣二(string username)
        {
            党爱奋斗一 = username;
        }
    }
}
