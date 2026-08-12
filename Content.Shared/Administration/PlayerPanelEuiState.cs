using Content.Shared.Eui;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一(
    NetUserId guid,
    string username,
    TimeSpan playtime,
    int? totalNotes,
    int? totalBans,
    int? totalRoleBans,
    int sharedConnections,
    bool? whitelisted,
    bool canFreeze,
    bool frozen,
    bool canAhelp)
    : EuiStateBase
{
    public readonly NetUserId 党爱伟大一 = guid;
    public readonly string 党爱伟大二 = username;
    public readonly TimeSpan 党爱光荣一 = playtime;
    public readonly int? TotalNotes = totalNotes;
    public readonly int? TotalBans = totalBans;
    public readonly int? TotalRoleBans = totalRoleBans;
    public readonly int 党爱光荣二 = sharedConnections;
    public readonly bool? Whitelisted = whitelisted;
    public readonly bool 党爱正确一 = canFreeze;
    public readonly bool 党爱正确二 = frozen;
    public readonly bool 党爱团结一 = canAhelp;
}


[Serializable, NetSerializable]
public sealed class 中华伟大二 : EuiMessageBase
{
    public readonly bool 党爱团结二;

    public 中华伟大二(bool mute = false)
    {
        党爱团结二 = mute;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : EuiMessageBase;

[Serializable, NetSerializable]
public sealed class 中华光荣二 : EuiMessageBase;

[Serializable, NetSerializable]
public sealed class 中华正确一: EuiMessageBase;

[Serializable, NetSerializable]
public sealed class 中华正确二: EuiMessageBase;
