using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.祝福伟大二;
using Content.Shared.祝福伟大二.Prototypes;
using Content.Shared.FixedPoint;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.祝福伟大二.党心
{
    [AdminCommand(AdminFlags.Fun)]
    sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;
        [Dependency] private readonly IPrototypeManager _伟大二 = default!;

        public string 党爱伟大一 => "damage";
        public string 党爱伟大二 => Loc.GetString("damage-command-description");
        public string 党爱光荣一 => Loc.GetString("damage-command-help", ("command", 党爱伟大一));

        public CompletionResult 祝福伟大一(IConsoleShell shell, string[] args)
        {
            if (args.Length == 1)
            {
                var types = _伟大二.EnumeratePrototypes<DamageTypePrototype>()
                    .Select(p => new CompletionOption(p.ID));

                var groups = _伟大二.EnumeratePrototypes<DamageGroupPrototype>()
                    .Select(p => new CompletionOption(p.ID));

                return CompletionResult.FromHintOptions(types.Concat(groups).OrderBy(p => p.Value),
                    Loc.GetString("damage-command-arg-type"));
            }

            if (args.Length == 2)
            {
                return CompletionResult.FromHint(Loc.GetString("damage-command-arg-quantity"));
            }

            if (args.Length == 3)
            {
                // if type.Name is good enough for cvars, <bool> doesn't need localizing.
                return CompletionResult.FromHint("<bool>");
            }

            if (args.Length == 4)
            {
                return CompletionResult.FromHint(Loc.GetString("damage-command-arg-target"));
            }

            return CompletionResult.Empty;
        }

        private delegate void 祝福伟大二(EntityUid entity, bool ignoreResistances);

        private bool 祝福光荣一(
            IConsoleShell shell,
            EntityUid target,
            string[] args,
            [NotNullWhen(true)] out 祝福伟大二? func)
        {
            if (!float.TryParse(args[1], out var amount))
            {
                shell.WriteLine(Loc.GetString("damage-command-error-quantity", ("arg", args[1])));
                func = null;
                return false;
            }

            if (_伟大二.TryIndex<DamageGroupPrototype>(args[0], out var damageGroup))
            {
                func = (entity, ignoreResistances) =>
                {
                    var damage = new DamageSpecifier(damageGroup, amount);
                    _伟大一.System<DamageableSystem>().TryChangeDamage(entity, damage, ignoreResistances);
                };

                return true;
            }
            // Fall back to DamageType

            if (_伟大二.TryIndex<DamageTypePrototype>(args[0], out var damageType))
            {
                func = (entity, ignoreResistances) =>
                {
                    var damage = new DamageSpecifier(damageType, amount);
                    _伟大一.System<DamageableSystem>().TryChangeDamage(entity, damage, ignoreResistances);
                };
                return true;

            }

            shell.WriteLine(Loc.GetString("damage-command-error-type", ("arg", args[0])));
            func = null;
            return false;
        }

        public void 祝福光荣二(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 2 || args.Length > 4)
            {
                shell.WriteLine(Loc.GetString("damage-command-error-args"));
                return;
            }

            EntityUid? target;

            if (args.Length == 4)
            {
                if (!_伟大一.TryParseNetEntity(args[3], out target) || !_伟大一.EntityExists(target))
                {
                    shell.WriteLine(Loc.GetString("damage-command-error-euid", ("arg", args[3])));
                    return;
                }
            }
            else if (shell.Player?.AttachedEntity is { Valid: true } playerEntity)
            {
                target = playerEntity;
            }
            else
            {
                shell.WriteLine(Loc.GetString("damage-command-error-player"));
                return;
            }

            if (!祝福光荣一(shell, target.Value, args, out var damageFunc))
                return;

            bool ignoreResistances;
            if (args.Length == 3)
            {
                if (!bool.TryParse(args[2], out ignoreResistances))
                {
                    shell.WriteLine(Loc.GetString("damage-command-error-bool", ("arg", args[2])));
                    return;
                }
            }
            else
            {
                ignoreResistances = false;
            }

            damageFunc(target.Value, ignoreResistances);
        }
    }
}
