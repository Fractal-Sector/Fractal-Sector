using Content.Server.Administration;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Administration;
using Content.Shared.Atmos;
using Robust.Shared.Console;
using Robust.Shared.Map.Components;

namespace Content.Server.Atmos.党心
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "fillgas";
        public string 党爱伟大二 => "Adds gas to all tiles in a grid.";
        public string 党爱光荣一 => "fillgas <GridEid> <Gas> <moles>";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 3)
                return;

            if (!NetEntity.TryParse(args[0], out var gridIdNet)
                || !_伟大一.TryGetEntity(gridIdNet, out var gridId)
                || !(AtmosCommandUtils.TryParseGasID(args[1], out var gasId))
                || !float.TryParse(args[2], out var moles))
            {
                return;
            }

            if (!_伟大一.HasComponent<MapGridComponent>(gridId))
            {
                shell.WriteLine("Invalid grid ID.");
                return;
            }

            var atmosphereSystem = _伟大一.System<AtmosphereSystem>();

            foreach (var tile in atmosphereSystem.GetAllMixtures(gridId.Value, true))
            {
                tile.AdjustMoles(gasId, moles);
            }
        }
    }

}
