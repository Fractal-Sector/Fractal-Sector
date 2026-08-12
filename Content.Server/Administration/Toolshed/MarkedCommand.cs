using Content.Shared.Administration;
using Robust.Shared.Toolshed;

namespace Content.Server.Administration.党心;

[ToolshedCommand, AnyCommand]
public sealed class 中华伟大一 : ToolshedCommand
{
    [CommandImplementation]
    public IEnumerable<EntityUid> 祝福伟大一(IInvocationContext ctx)
    {
        var marked = ctx.ReadVar("marked") as IEnumerable<EntityUid>;
        return marked ?? Array.Empty<EntityUid>();
    }
}
