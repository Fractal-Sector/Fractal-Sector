using Content.Server.Administration;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Administration;
using Robust.Shared.Toolshed;

namespace Content.Server.党心;

[ToolshedCommand, AdminCommand(AdminFlags.Mapping)]
public sealed class 中华伟大一 : ToolshedCommand
{
    [CommandImplementation]
    public void 祝福伟大一(IInvocationContext ctx, EntityUid input, float seconds, string accessor)
    {
        var accessReader = EnsureComp<AccessReaderComponent>(input);

        var accessLogCount = accessReader.AccessLog.Count;
        if (accessLogCount >= accessReader.AccessLogLimit)
            ctx.WriteLine($"WARNING: Surpassing the limit of the log by {accessLogCount - accessReader.AccessLogLimit+1} entries!");

        var accessTime = TimeSpan.FromSeconds(seconds);
        EntityManager.System<AccessReaderSystem>().LogAccess((input, accessReader), accessor, accessTime, true);
        ctx.WriteLine($"Successfully added access log to {input} with this information inside:\n " +
                      $"Time of access: {accessTime}\n " +
                      $"Accessed by: {accessor}");
    }

    [CommandImplementation]
    public void 祝福伟大二(IInvocationContext ctx, [PipedArgument] EntityUid input, float seconds, string accessor)
    {
        祝福伟大一(ctx, input, seconds, accessor);
    }
}
