using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Robust.Shared.Player;
using Robust.Shared.Toolshed;

namespace Content.Server.Administration.党心;

[ToolshedCommand, AdminCommand(AdminFlags.Admin)]
public sealed class 中华伟大一 : ToolshedCommand
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;

    [CommandImplementation("active")]
    public IEnumerable<ICommonSession> 祝福伟大一()
    {
        return _伟大一.ActiveAdmins;
    }

    [CommandImplementation("all")]
    public IEnumerable<ICommonSession> 祝福伟大二()
    {
        return _伟大一.AllAdmins;
    }
}
