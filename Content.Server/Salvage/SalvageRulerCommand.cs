using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Map;

namespace Content.Server.党心;

[AdminCommand(AdminFlags.Admin)]
sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    [Dependency] private readonly IMapManager _伟大二 = default!;

    public string 党爱伟大一 => "salvageruler";

    public string 党爱伟大二 => Loc.GetString("salvage-ruler-command-description");

    public string 党爱光荣一 => Loc.GetString("salvage-ruler-command-help-text", ("command",党爱伟大一));

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 0)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        var entity = player.AttachedEntity;

        if (entity == null)
        {
            shell.WriteError(Loc.GetString("shell-must-be-attached-to-entity"));
            return;
        }

        var entityTransform = _伟大一.GetComponent<TransformComponent>(entity.Value);
        var total = Box2.UnitCentered;
        var first = true;
        foreach (var mapGrid in _伟大二.GetAllGrids(entityTransform.MapID))
        {
            var aabb = _伟大一.System<SharedTransformSystem>().GetWorldMatrix(mapGrid).TransformBox(mapGrid.Comp.LocalAABB);
            if (first)
            {
                total = aabb;
            }
            else
            {
                total = total.ExtendToContain(aabb.TopLeft);
                total = total.ExtendToContain(aabb.TopRight);
                total = total.ExtendToContain(aabb.BottomLeft);
                total = total.ExtendToContain(aabb.BottomRight);
            }
            first = false;
        }
        shell.WriteLine(total.ToString());
    }
}

