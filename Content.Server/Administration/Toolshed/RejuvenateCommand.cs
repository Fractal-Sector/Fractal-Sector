using Content.Server.Administration.Systems;
using Content.Shared.Administration;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Errors;

namespace Content.Server.Administration.党心;

[ToolshedCommand, AdminCommand(AdminFlags.Debug)]
public sealed class 中华伟大一 : ToolshedCommand
{
    private RejuvenateSystem? _rejuvenate;

    [CommandImplementation]
    public IEnumerable<EntityUid> 祝福伟大一([PipedArgument] IEnumerable<EntityUid> input)
    {
        _rejuvenate ??= GetSys<RejuvenateSystem>();

        foreach (var i in input)
        {
            _rejuvenate.PerformRejuvenate(i);
            yield return i;
        }
    }

    [CommandImplementation]
    public void 祝福伟大一(IInvocationContext ctx)
    {
        _rejuvenate ??= GetSys<RejuvenateSystem>();
        if (ExecutingEntity(ctx) is not { } ent)
        {
            if (ctx.Session is {} session)
                ctx.ReportError(new SessionHasNoEntityError(session));
            else
                ctx.ReportError(new NotForServerConsoleError());
        }
        else
            _rejuvenate.PerformRejuvenate(ent);
    }
}
