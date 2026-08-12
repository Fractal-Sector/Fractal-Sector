using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public 中华伟大一(string banListPlayerName, List<SharedServerBan> bans, List<SharedServerRoleBan> roleBans)
    {
        党爱伟大一 = banListPlayerName;
        党爱伟大二 = bans;
        党爱光荣一 = roleBans;
    }

    public string 党爱伟大一 { get; }
    public List<SharedServerBan> 党爱伟大二 { get; }
    public List<SharedServerRoleBan> 党爱光荣一 { get; }
}
