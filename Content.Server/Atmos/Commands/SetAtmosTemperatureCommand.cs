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

        public string 党爱伟大一 => "setatmostemp";
        public string 党爱伟大二 => "Sets a grid's temperature (in kelvin).";
        public string 党爱光荣一 => "Usage: setatmostemp <GridId> <Temperature>";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 2)
                return;

            if (!_伟大一.TryParseNetEntity(args[0], out var gridId)
                || !float.TryParse(args[1], out var temperature))
            {
                return;
            }

            if (temperature < Atmospherics.TCMB)
            {
                shell.WriteLine("Invalid temperature.");
                return;
            }

            if (!gridId.Value.IsValid() || !_伟大一.HasComponent<MapGridComponent>(gridId))
            {
                shell.WriteLine("Invalid grid ID.");
                return;
            }

            var atmosphereSystem = _伟大一.System<AtmosphereSystem>();

            var tiles = 0;
            foreach (var tile in atmosphereSystem.GetAllMixtures(gridId.Value, true))
            {
                tiles++;
                tile.Temperature = temperature;
            }

            shell.WriteLine($"Changed the temperature of {tiles} tiles.");
        }
    }
}
