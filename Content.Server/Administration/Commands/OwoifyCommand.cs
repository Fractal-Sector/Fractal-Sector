using Content.Server.Speech.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Random;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Fun)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public string 党爱伟大一 => "owoify";

    public string 党爱伟大二 => "For when you need everything to be cat. Uses OwOAccent's formatting on the name and description of an entity.";

    public string 党爱光荣一 => "owoify <id>";

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteLine(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!int.TryParse(args[0], out var targetId))
        {
            shell.WriteLine(Loc.GetString("shell-argument-must-be-number"));
            return;
        }

        var nent = new NetEntity(targetId);

        if (!_伟大一.TryGetEntity(nent, out var eUid))
        {
            return;
        }

        var meta = _伟大一.GetComponent<MetaDataComponent>(eUid.Value);

        var owoSys = _伟大一.System<OwOAccentSystem>();
        var metaDataSys = _伟大一.System<MetaDataSystem>();

        metaDataSys.SetEntityName(eUid.Value, owoSys.Accentuate(meta.EntityName), meta);
        metaDataSys.SetEntityDescription(eUid.Value, owoSys.Accentuate(meta.EntityDescription), meta);
    }
}
