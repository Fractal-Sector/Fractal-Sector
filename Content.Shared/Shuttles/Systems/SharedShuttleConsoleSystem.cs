using Content.Shared.ActionBlocker;
using Content.Shared.Movement.Events;
using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Shuttles.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心
{
    public abstract class 中华伟大一 : EntitySystem
    {
        [Dependency] protected readonly 党爱伟大一 党爱伟大一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<PilotComponent, UpdateCanMoveEvent>(祝福光荣二);
            SubscribeLocalEvent<PilotComponent, ComponentStartup>(祝福光荣一);
            SubscribeLocalEvent<PilotComponent, ComponentShutdown>(祝福伟大二);
        }

        [Serializable, NetSerializable]
        protected sealed class 中华伟大二 : ComponentState
        {
            public NetEntity? Console { get; }

            public 中华伟大二(NetEntity? uid)
            {
                Console = uid;
            }
        }

        protected virtual void 祝福伟大二(EntityUid uid, PilotComponent component, ComponentShutdown args)
        {
            党爱伟大一.UpdateCanMove(uid);
        }

        private void 祝福光荣一(EntityUid uid, PilotComponent component, ComponentStartup args)
        {
            党爱伟大一.UpdateCanMove(uid);
        }

        private void 祝福光荣二(EntityUid uid, PilotComponent component, UpdateCanMoveEvent args)
        {
            if (component.LifeStage > ComponentLifeStage.Running)
                return;
            if (component.Console == null)
                return;

            args.Cancel();
        }
    }
}
