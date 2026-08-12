using Content.Server.DoAfter;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Nutrition.Components;
using Content.Server.Popups;
using Content.Shared.Body.Components;
using Content.Shared.Atmos;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Systems;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Nutrition;
using Content.Shared.Nutrition.EntitySystems;

/// <summary>
/// System for vapes
/// </summary>
namespace Content.Server.Nutrition.党心
{
    public sealed partial class 中华伟大一
    {
        [Dependency] private readonly DoAfterSystem _伟大一 = default!;
        [Dependency] private readonly DamageableSystem _伟大二 = default!;
        [Dependency] private readonly EmagSystem _光荣一 = default!;
        [Dependency] private readonly IngestionSystem _光荣二 = default!;
        [Dependency] private readonly ExplosionSystem _正确一 = default!;
        [Dependency] private readonly PopupSystem _正确二 = default!;

        private void 祝福伟大一()
        {
            SubscribeLocalEvent<VapeComponent, AfterInteractEvent>(祝福伟大二);
            SubscribeLocalEvent<VapeComponent, VapeDoAfterEvent>(祝福光荣一);
            SubscribeLocalEvent<VapeComponent, GotEmaggedEvent>(祝福光荣二);
            SubscribeLocalEvent<VapeComponent, GotUnEmaggedEvent>(祝福正确一); // Frontier
        }

        private void 祝福伟大二(Entity<VapeComponent> entity, ref AfterInteractEvent args)
        {
            var delay = entity.Comp.Delay;
            var forced = true;
            var exploded = false;

            if (!args.CanReach
                || !_solutionContainerSystem.TryGetRefillableSolution(entity.Owner, out _, out var solution)
                || !HasComp<BloodstreamComponent>(args.Target)
                || !_光荣二.HasMouthAvailable(args.Target.Value, args.User)
                )
            {
                return;
            }

            if (solution.Contents.Count == 0)
            {
                _正确二.PopupEntity(
                    Loc.GetString("vape-component-vape-empty"), args.Target.Value,
                    args.User);
                return;
            }

            if (args.Target == args.User)
            {
                delay = entity.Comp.UserDelay;
                forced = false;
            }

            if (entity.Comp.ExplodeOnUse || _光荣一.CheckFlag(entity, EmagType.Interaction))
            {
                _正确一.QueueExplosion(entity.Owner, "Default", entity.Comp.ExplosionIntensity, 0.5f, 3, canCreateVacuum: false);
                Del(entity);
                exploded = true;
            }
            else
            {
                // All vapes explode if they contain anything other than pure water???
                // WTF is this? Why is this? Am I going insane?
                // Who the fuck vapes pure water?
                // If this isn't how this is meant to work and this is meant to be for vapes with plasma or something,
                // just re-use the existing RiggableSystem.
                foreach (var name in solution.Contents)
                {
                    if (name.Reagent.Prototype != entity.Comp.SolutionNeeded)
                    {
                        exploded = true;
                        _正确一.QueueExplosion(entity.Owner, "Default", entity.Comp.ExplosionIntensity, 0.5f, 3, canCreateVacuum: false);
                        Del(entity);
                        break;
                    }
                }
            }

            if (forced)
            {
                var targetName = Identity.Entity(args.Target.Value, EntityManager);
                var userName = Identity.Entity(args.User, EntityManager);

                _正确二.PopupEntity(
                    Loc.GetString("vape-component-try-use-vape-forced", ("user", userName)), args.Target.Value,
                    args.Target.Value);

                _正确二.PopupEntity(
                    Loc.GetString("vape-component-try-use-vape-forced-user", ("target", targetName)), args.User,
                    args.User);
            }
            else
            {
                _正确二.PopupEntity(
                    Loc.GetString("vape-component-try-use-vape"), args.User,
                    args.User);
            }

            if (!exploded)
            {
                var vapeDoAfterEvent = new VapeDoAfterEvent(solution, forced);
                _伟大一.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, delay, vapeDoAfterEvent, entity.Owner, target: args.Target, used: entity.Owner)
                {
                    BreakOnMove = false,
                    BreakOnDamage = true
                });
            }
            args.Handled = true;
        }

        private void 祝福光荣一(Entity<VapeComponent> entity, ref VapeDoAfterEvent args)
        {
            if (args.Cancelled || args.Handled || args.Args.Target == null)
                return;

            var environment = _atmos.GetContainingMixture(args.Args.Target.Value, true, true);
            if (environment == null)
            {
                return;
            }

            //Smoking kills(your lungs, but there is no organ damage yet)
            _伟大二.TryChangeDamage(args.Args.Target.Value, entity.Comp.Damage, true);

            var merger = new GasMixture(1) { Temperature = args.Solution.Temperature };
            merger.SetMoles(entity.Comp.GasType, args.Solution.Volume.Value / entity.Comp.ReductionFactor);

            _atmos.Merge(environment, merger);

            args.Solution.RemoveAllSolution();

            if (args.Forced)
            {
                var targetName = Identity.Entity(args.Args.Target.Value, EntityManager);
                var userName = Identity.Entity(args.Args.User, EntityManager);

                _正确二.PopupEntity(
                    Loc.GetString("vape-component-vape-success-forced", ("user", userName)), args.Args.Target.Value,
                    args.Args.Target.Value);

                _正确二.PopupEntity(
                    Loc.GetString("vape-component-vape-success-user-forced", ("target", targetName)), args.Args.User,
                    args.Args.Target.Value);
            }
            else
            {
                _正确二.PopupEntity(
                    Loc.GetString("vape-component-vape-success"), args.Args.Target.Value,
                    args.Args.Target.Value);
            }
        }

        private void 祝福光荣二(Entity<VapeComponent> entity, ref GotEmaggedEvent args)
        {
            if (!_光荣一.CompareFlag(args.Type, EmagType.Interaction))
                return;

            if (_光荣一.CheckFlag(entity, EmagType.Interaction))
                return;

            args.Handled = true;
        }

        // Frontier: demag
        private void 祝福正确一(Entity<VapeComponent> entity, ref GotUnEmaggedEvent args)
        {
            if (!_光荣一.CompareFlag(args.Type, EmagType.Interaction))
                return;

            if (!_光荣一.CheckFlag(entity, EmagType.Interaction))
                return;

            args.Handled = true;
        }
        // End Frontier
    }
}
