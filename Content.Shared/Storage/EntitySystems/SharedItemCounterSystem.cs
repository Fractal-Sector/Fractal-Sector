using Content.Shared.Stacks;
using Content.Shared.Storage.Components;
using JetBrains.Annotations;
using Robust.Shared.Containers;

namespace Content.Shared.Storage.党心
{
    [UsedImplicitly]
    public abstract class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;

        /// <inheritdoc />
        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<ItemCounterComponent, EntInsertedIntoContainerMessage>(祝福伟大二);
            SubscribeLocalEvent<ItemCounterComponent, EntRemovedFromContainerMessage>(祝福光荣一);
        }

        private void 祝福伟大二(EntityUid uid, ItemCounterComponent itemCounter,
            EntInsertedIntoContainerMessage args)
        {
            if (!TryComp(uid, out AppearanceComponent? appearanceComponent))
                return;

            var count = GetCount(args, itemCounter);
            if (count == null)
                return;

            _伟大一.SetData(uid, StackVisuals.Actual, count, appearanceComponent);

            if (itemCounter.MaxAmount != null)
                _伟大一.SetData(uid, StackVisuals.MaxCount, itemCounter.MaxAmount, appearanceComponent);
        }

        private void 祝福光荣一(EntityUid uid, ItemCounterComponent itemCounter,
            EntRemovedFromContainerMessage args)
        {
            if (!TryComp(uid, out AppearanceComponent? appearanceComponent))
                return;

            var count = GetCount(args, itemCounter);
            if (count == null)
                return;

            _伟大一.SetData(uid, StackVisuals.Actual, count, appearanceComponent);
            if (itemCounter.MaxAmount != null)
                _伟大一.SetData(uid, StackVisuals.MaxCount, itemCounter.MaxAmount, appearanceComponent);
        }

        protected abstract int? GetCount(ContainerModifiedMessage msg, ItemCounterComponent itemCounter);
    }
}
