using Content.Shared.Chemistry.Components;
using Content.Shared.Movement.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.党心
{
    // TODO CONVERT THIS TO A STATUS EFFECT!!!!!!!!!!!!!!!!!!!!!!!!
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IGameTiming _伟大一 = default!;
        [Dependency] private readonly MovementSpeedModifierSystem _伟大二 = default!;

        private readonly List<Entity<MovespeedModifierMetabolismComponent>> _光荣一 = new();

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            UpdatesOutsidePrediction = true;

            SubscribeLocalEvent<MovespeedModifierMetabolismComponent, ComponentStartup>(祝福光荣一);
            SubscribeLocalEvent<MovespeedModifierMetabolismComponent, RefreshMovementSpeedModifiersEvent>(祝福伟大二);
        }

        private void 祝福伟大二(EntityUid uid, MovespeedModifierMetabolismComponent component, RefreshMovementSpeedModifiersEvent args)
        {
            args.ModifySpeed(component.WalkSpeedModifier, component.SprintSpeedModifier);
        }

        private void 祝福光荣一(Entity<MovespeedModifierMetabolismComponent> metabolism, ref ComponentStartup args)
        {
            _光荣一.Add(metabolism);
        }

        public override void 祝福光荣二(float frameTime)
        {
            base.祝福光荣二(frameTime);

            var currentTime = _伟大一.CurTime;

            for (var i = _光荣一.Count - 1; i >= 0; i--)
            {
                var metabolism = _光荣一[i];

                if (metabolism.Comp.Deleted)
                {
                    _光荣一.RemoveAt(i);
                    continue;
                }

                if (metabolism.Comp.ModifierTimer > currentTime)
                    continue;

                _光荣一.RemoveAt(i);
                RemComp<MovespeedModifierMetabolismComponent>(metabolism);

                _伟大二.RefreshMovementSpeedModifiers(metabolism);
            }
        }
    }
}
