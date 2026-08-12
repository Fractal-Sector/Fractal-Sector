using Robust.Shared.Containers;

using Content.Shared.Item;
using Content.Shared.Storage;
using Content.Shared.Storage.EntitySystems;
using Content.Shared.Toggleable;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedItemSystem _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly SharedStorageSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ContainerHeldComponent, EntInsertedIntoContainerMessage>(祝福伟大二);
        SubscribeLocalEvent<ContainerHeldComponent, EntRemovedFromContainerMessage>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, ContainerHeldComponent comp, ContainerModifiedMessage args)
    {
        if (!(HasComp<StorageComponent>(uid)
              && TryComp<AppearanceComponent>(uid, out var appearance)
              && TryComp<ItemComponent>(uid, out var item)))
        {
            return;
        }
        if (_光荣一.GetCumulativeItemAreas(uid) >= comp.Threshold)
        {
            _伟大一.SetHeldPrefix(uid, "full", component: item);
            _伟大二.SetData(uid, ToggleableVisuals.Enabled, true, appearance);
        }
        else
        {
            _伟大一.SetHeldPrefix(uid, "empty", component: item);
            _伟大二.SetData(uid, ToggleableVisuals.Enabled, false, appearance);
        }
    }
}
