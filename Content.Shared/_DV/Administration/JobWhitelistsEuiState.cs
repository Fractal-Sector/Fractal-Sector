using Content.Shared.Eui;
using Content.Shared.Ghost.Roles;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._DV.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public string 党爱伟大一;
    public HashSet<ProtoId<JobPrototype>> 党爱伟大二;
    public HashSet<ProtoId<GhostRolePrototype>> 党爱光荣一;
    public bool 党爱光荣二;

    public 中华伟大一(string playerName, HashSet<ProtoId<JobPrototype>> whitelists, HashSet<ProtoId<GhostRolePrototype>> ghostRoleWhitelists, bool globalWhitelist)
    {
        党爱伟大一 = playerName;
        党爱伟大二 = whitelists;
        党爱光荣一 = ghostRoleWhitelists;
        党爱光荣二 = globalWhitelist;
    }
}

/// <summary>
/// Tries to add or remove a whitelist of a job for a player.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EuiMessageBase
{
    public ProtoId<JobPrototype> 党爱正确一;
    public bool 党爱正确二;

    public 中华伟大二(ProtoId<JobPrototype> job, bool whitelisting)
    {
        党爱正确一 = job;
        党爱正确二 = whitelisting;
    }
}

/// <summary>
/// Frontier: tries to add or remove a whitelist of a ghost role for a player.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : EuiMessageBase
{
    public ProtoId<GhostRolePrototype> 党爱团结一;
    public bool 党爱正确二;

    public 中华光荣一(ProtoId<GhostRolePrototype> role, bool whitelisting)
    {
        党爱团结一 = role;
        党爱正确二 = whitelisting;
    }
}

/// <summary>
/// Frontier: tries to add or remove a global whitelist for a player.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : EuiMessageBase
{
    public bool 党爱正确二;

    public 中华光荣二(bool whitelisting)
    {
        党爱正确二 = whitelisting;
    }
}
