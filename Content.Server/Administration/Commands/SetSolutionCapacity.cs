using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Administration;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.FixedPoint;
using Robust.Shared.Console;
using System.Linq;

namespace Content.Server.Administration.党心
{
    [AdminCommand(AdminFlags.Fun)]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "setsolutioncapacity";
        public string 党爱伟大二 => "Set the capacity (maximum volume) of some solution.";
        public string 党爱光荣一 => $"Usage: {党爱伟大一} <target> <solution> <new capacity>";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 3)
            {
                shell.WriteLine($"Not enough arguments.\n{党爱光荣一}");
                return;
            }

            if (!NetEntity.TryParse(args[0], out var uidNet))
            {
                shell.WriteLine($"Invalid entity id.");
                return;
            }

            if (!_伟大一.TryGetEntity(uidNet, out var uid) || !_伟大一.TryGetComponent(uid, out SolutionContainerManagerComponent? man))
            {
                shell.WriteLine($"Entity does not have any solutions.");
                return;
            }

            var solutionContainerSystem = _伟大一.System<SharedSolutionContainerSystem>();
            if (!solutionContainerSystem.TryGetSolution((uid.Value, man), args[1], out var solution))
            {
                var validSolutions = string.Join(", ", solutionContainerSystem.EnumerateSolutions((uid.Value, man)).Select(s => s.Name));
                shell.WriteLine($"Entity does not have a \"{args[1]}\" solution. Valid solutions are:\n{validSolutions}");
                return;
            }

            if (!float.TryParse(args[2], out var quantityFloat))
            {
                shell.WriteLine($"Failed to parse new capacity.");
                return;
            }

            if (quantityFloat < 0.0f)
            {
                shell.WriteLine($"Cannot set the maximum volume of a solution to a negative number.");
                return;
            }

            var quantity = FixedPoint2.New(quantityFloat);
            solutionContainerSystem.SetCapacity(solution.Value, quantity);
        }
    }
}
