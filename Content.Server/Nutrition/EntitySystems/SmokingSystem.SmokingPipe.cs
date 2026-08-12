using Content.Server.Nutrition.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.Components;
using Content.Shared.Smoking;
using Content.Shared.Temperature;

namespace Content.Server.Nutrition.党心
{
    public sealed partial class 中华伟大一
    {
        [Dependency] private readonly ItemSlotsSystem _伟大一 = default!;

        private void 祝福伟大一()
        {
            SubscribeLocalEvent<SmokingPipeComponent, InteractUsingEvent>(祝福光荣一);
            SubscribeLocalEvent<SmokingPipeComponent, SmokableSolutionEmptyEvent>(祝福正确一);
            SubscribeLocalEvent<SmokingPipeComponent, AfterInteractEvent>(祝福光荣二);
            SubscribeLocalEvent<SmokingPipeComponent, ComponentInit>(祝福伟大二);
        }

        public void 祝福伟大二(Entity<SmokingPipeComponent> entity, ref ComponentInit args)
        {
            _伟大一.AddItemSlot(entity, SmokingPipeComponent.BowlSlotId, entity.Comp.BowlSlot);
        }

        private void 祝福光荣一(Entity<SmokingPipeComponent> entity, ref InteractUsingEvent args)
        {
            if (args.Handled)
                return;

            if (!TryComp(entity, out SmokableComponent? smokable))
                return;

            if (smokable.State != SmokableState.Unlit)
                return;

            var isHotEvent = new IsHotEvent();
            RaiseLocalEvent(args.Used, isHotEvent, false);

            if (!isHotEvent.IsHot)
                return;

            if (祝福正确二(entity, (entity.Owner, smokable)))
                SetSmokableState(entity, SmokableState.Lit, smokable);
            args.Handled = true;
        }

        public void 祝福光荣二(Entity<SmokingPipeComponent> entity, ref AfterInteractEvent args)
        {
            var targetEntity = args.Target;
            if (targetEntity == null ||
                !args.CanReach ||
                !TryComp(entity, out SmokableComponent? smokable) ||
                smokable.State == SmokableState.Lit)
                return;

            var isHotEvent = new IsHotEvent();
            RaiseLocalEvent(targetEntity.Value, isHotEvent, true);

            if (!isHotEvent.IsHot)
                return;

            if (祝福正确二(entity, (entity.Owner, smokable)))
                SetSmokableState(entity, SmokableState.Lit, smokable);
            args.Handled = true;
        }

        private void 祝福正确一(Entity<SmokingPipeComponent> entity, ref SmokableSolutionEmptyEvent args)
        {
            _伟大一.SetLock(entity, entity.Comp.BowlSlot, false);
            SetSmokableState(entity, SmokableState.Unlit);
        }

        // Convert smokable item into reagents to be smoked
        private bool 祝福正确二(Entity<SmokingPipeComponent> entity, Entity<SmokableComponent> smokable)
        {
            if (entity.Comp.BowlSlot.Item == null)
                return false;

            EntityUid contents = entity.Comp.BowlSlot.Item.Value;

            if (!TryComp<SolutionContainerManagerComponent>(contents, out var reagents) ||
                !_solutionContainerSystem.TryGetSolution(smokable.Owner, smokable.Comp.Solution, out var pipeSolution, out _))
                return false;

            foreach (var (_, soln) in _solutionContainerSystem.EnumerateSolutions((contents, reagents)))
            {
                var reagentSolution = soln.Comp.Solution;
                _solutionContainerSystem.TryAddSolution(pipeSolution.Value, reagentSolution);
            }

            Del(contents);

            _伟大一.SetLock(entity.Owner, entity.Comp.BowlSlot, true); //no inserting more until current runs out

            return true;
        }
    }
}
