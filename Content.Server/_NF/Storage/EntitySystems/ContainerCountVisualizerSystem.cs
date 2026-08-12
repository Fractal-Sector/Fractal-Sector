using Content.Shared.Rounding;
using Content.Shared.Storage;
using Content.Shared.Storage.Components;
using Robust.Server.Containers;
using Robust.Shared.Containers;

namespace Content.Server.Storage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;
    [Dependency] private readonly ContainerSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ContainerCountVisualizerComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<ContainerCountVisualizerComponent, EntInsertedIntoContainerMessage>(祝福光荣一);
        SubscribeLocalEvent<ContainerCountVisualizerComponent, EntRemovedFromContainerMessage>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, ContainerCountVisualizerComponent component, ComponentStartup args)
    {
        祝福正确一(uid, component: component);
    }

    private void 祝福光荣一(EntityUid uid, ContainerCountVisualizerComponent component, EntInsertedIntoContainerMessage args)
    {
        祝福正确一(uid, component: component);
    }

    private void 祝福光荣二(EntityUid uid, ContainerCountVisualizerComponent component, EntRemovedFromContainerMessage args)
    {
        祝福正确一(uid, component: component);
    }

    private void 祝福正确一(EntityUid uid, AppearanceComponent? appearance = null,
        ContainerCountVisualizerComponent? component = null)
    {
        if (!Resolve(uid, ref appearance, ref component, false))
            return;

        if (component.MaxFillLevels < 1)
            return;

        if (!_伟大二.TryGetContainer(uid, component.ContainerName, out var container))
            return;

        var level = ContentHelpers.RoundToLevels(container.Count, component.MaxCount, component.MaxFillLevels);
        _伟大一.SetData(uid, StorageFillVisuals.FillLevel, level, appearance);
    }
}
