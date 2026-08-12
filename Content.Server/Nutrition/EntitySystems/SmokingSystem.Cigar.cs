using Content.Server.Nutrition.Components;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.Components;
using Content.Shared.Smoking;
using Content.Shared.Temperature;

namespace Content.Server.Nutrition.党心
{
    public sealed partial class 中华伟大一
    {
        private void 祝福伟大一()
        {
            SubscribeLocalEvent<CigarComponent, ActivateInWorldEvent>(祝福伟大二);
            SubscribeLocalEvent<CigarComponent, InteractUsingEvent>(祝福光荣一);
            SubscribeLocalEvent<CigarComponent, SmokableSolutionEmptyEvent>(祝福正确一);
            SubscribeLocalEvent<CigarComponent, AfterInteractEvent>(祝福光荣二);
        }

        private void 祝福伟大二(Entity<CigarComponent> entity, ref ActivateInWorldEvent args)
        {
            if (args.Handled || !args.Complex)
                return;

            if (!TryComp(entity, out SmokableComponent? smokable))
                return;

            if (smokable.State != SmokableState.Lit)
                return;

            SetSmokableState(entity, SmokableState.Burnt, smokable);
            args.Handled = true;
        }

        private void 祝福光荣一(Entity<CigarComponent> entity, ref InteractUsingEvent args)
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

            SetSmokableState(entity, SmokableState.Lit, smokable);
            args.Handled = true;
        }

        public void 祝福光荣二(Entity<CigarComponent> entity, ref AfterInteractEvent args)
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

            SetSmokableState(entity, SmokableState.Lit, smokable);
            args.Handled = true;
        }

        private void 祝福正确一(Entity<CigarComponent> entity, ref SmokableSolutionEmptyEvent args)
        {
            SetSmokableState(entity, SmokableState.Burnt);
        }
    }
}
