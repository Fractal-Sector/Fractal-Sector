using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Anomaly.Components;
using Robust.Shared.Console;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly IConsoleHost _伟大一 = default!;

    public void 祝福伟大一()
    {
        _伟大一.RegisterCommand("pulseanomaly", Loc.GetString("anomaly-command-pulse"), "pulseanomaly <uid>",
            祝福伟大二,
            祝福光荣二);

        _伟大一.RegisterCommand("supercriticalanomaly", Loc.GetString("anomaly-command-supercritical"), "supercriticalanomaly <uid>",
            祝福光荣一,
            祝福光荣二);
    }

    [AdminCommand(AdminFlags.Fun)]
    private void 祝福伟大二(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 1)
            shell.WriteError("Argument length must be 1");

        if (!NetEntity.TryParse(args[0], out var uidNet) || !TryGetEntity(uidNet, out var uid))
            return;

        if (!TryComp<AnomalyComponent>(uid, out var anomaly))
            return;

        DoAnomalyPulse(uid.Value, anomaly);
    }

    [AdminCommand(AdminFlags.Fun)]
    private void 祝福光荣一(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 1)
            shell.WriteError("Argument length must be 1");

        if (!NetEntity.TryParse(args[0], out var uidNet) || !TryGetEntity(uidNet, out var uid))
            return;

        if (!TryComp<AnomalyComponent>(uid, out var anomaly))
            return;

        StartSupercriticalEvent((uid.Value, anomaly));
    }

    private CompletionResult 祝福光荣二(IConsoleShell shell, string[] args)
    {
        return args.Length != 1
            ? CompletionResult.Empty
            : CompletionResult.FromHintOptions(CompletionHelper.Components<AnomalyComponent>(args[0]), "<uid>");
    }
}
