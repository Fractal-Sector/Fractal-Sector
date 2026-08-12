using Content.Shared.Movement.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly MovementSpeedModifierSystem _伟大一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<CarryingSlowdownComponent, ComponentGetState>(祝福光荣一);
            SubscribeLocalEvent<CarryingSlowdownComponent, ComponentHandleState>(祝福光荣二);
            SubscribeLocalEvent<CarryingSlowdownComponent, RefreshMovementSpeedModifiersEvent>(祝福正确一);
        }

        public void 祝福伟大二(EntityUid uid, float walkSpeedModifier, float sprintSpeedModifier, CarryingSlowdownComponent? component = null)
        {
            if (!Resolve(uid, ref component))
                return;

            component.WalkModifier = walkSpeedModifier;
            component.SprintModifier = sprintSpeedModifier;
            _伟大一.RefreshMovementSpeedModifiers(uid);
        }
        private void 祝福光荣一(EntityUid uid, CarryingSlowdownComponent component, ref ComponentGetState args)
        {
            args.State = new CarryingSlowdownComponentState(component.WalkModifier, component.SprintModifier);
        }

        private void 祝福光荣二(EntityUid uid, CarryingSlowdownComponent component, ref ComponentHandleState args)
        {
            if (args.Current is not CarryingSlowdownComponentState state)
                return;

            component.WalkModifier = state.WalkModifier;
            component.SprintModifier = state.SprintModifier;
            _伟大一.RefreshMovementSpeedModifiers(uid);
        }
        private void 祝福正确一(EntityUid uid, CarryingSlowdownComponent component, RefreshMovementSpeedModifiersEvent args)
        {
            args.ModifySpeed(component.WalkModifier, component.SprintModifier);
        }
    }
}
