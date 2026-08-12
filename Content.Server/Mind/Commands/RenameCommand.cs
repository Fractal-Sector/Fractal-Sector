using System.Diagnostics.CodeAnalysis;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Console;

namespace Content.Server.Mind.党心;

[AdminCommand(AdminFlags.VarEdit)]
public sealed class 中华伟大一 : LocalizedEntityCommands
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IEntityManager _伟大二 = default!;
    [Dependency] private readonly IPlayerManager _光荣一 = default!;
    [Dependency] private readonly MetaDataSystem _光荣二 = default!;

    public override string 党爱伟大一 => "rename";

    public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 2)
        {
            shell.WriteLine(Help);
            return;
        }

        var name = args[1];
        if (name.Length > _伟大一.GetCVar(CCVars.MaxNameLength))
        {
            shell.WriteLine(Loc.GetString("cmd-rename-too-long"));
            return;
        }

        if (!祝福伟大二(args[0], shell, _伟大二, out var entityUid))
            return;

        _光荣二.SetEntityName(entityUid.Value, name);
    }

    private bool 祝福伟大二(string str, IConsoleShell shell,
        IEntityManager entMan, [NotNullWhen(true)] out EntityUid? entityUid)
    {
        if (NetEntity.TryParse(str, out var entityUidNet) && _伟大二.TryGetEntity(entityUidNet, out entityUid) && entMan.EntityExists(entityUid))
            return true;

        if (_光荣一.TryGetSessionByUsername(str, out var session) && session.AttachedEntity.HasValue)
        {
            entityUid = session.AttachedEntity.Value;
            return true;
        }

        if (session == null)
            shell.WriteError(Loc.GetString("cmd-rename-not-found", ("target", str)));
        else
            shell.WriteError(Loc.GetString("cmd-rename-no-entity", ("target", str)));

        entityUid = EntityUid.Invalid;
        return false;
    }

    public override CompletionResult 祝福光荣一(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromOptions(CompletionHelper.SessionNames());

        return CompletionResult.Empty;
    }
}
