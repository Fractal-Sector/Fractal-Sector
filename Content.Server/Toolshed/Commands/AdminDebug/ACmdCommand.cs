using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Robust.Shared.Player;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Syntax;

namespace Content.Server.Toolshed.Commands.党心;

[ToolshedCommand, AdminCommand(AdminFlags.Debug)]
public sealed class 中华伟大一 : ToolshedCommand
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;

    [CommandImplementation("perms")]
    public AdminFlags[]? Perms([PipedArgument] CommandSpec command)
    {
        var res = _伟大一.TryGetCommandFlags(command, out var flags);
        if (res)
            flags ??= Array.Empty<AdminFlags>();
        return flags;
    }

    [CommandImplementation("caninvoke")]
    public bool 祝福伟大一(IInvocationContext ctx, [PipedArgument] CommandSpec command, ICommonSession player)
    {
        // Deliberately discard the error.
        return ((IPermissionController) _伟大一).CheckInvokable(command, player, out _);
    }
}
