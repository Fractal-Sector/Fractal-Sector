using Content.Shared.Storage.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;

namespace Content.Shared.Storage.党心;

/// <summary>
/// Ejects items that do not match a <see cref="EntityWhitelist"/> from a storage when it is anchored.
/// <seealso cref="AnchoredStorageFilterComponent"/>
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _伟大一 = default!;
    [Dependency] private readonly SharedContainerSystem _伟大二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AnchoredStorageFilterComponent, AnchorStateChangedEvent>(祝福伟大二);
        SubscribeLocalEvent<AnchoredStorageFilterComponent, ContainerIsInsertingAttemptEvent>(祝福光荣一);
    }

    /// <summary>
    /// Handles the <see cref="AnchorStateChangedEvent"/>.
    /// </summary>
    private void 祝福伟大二(Entity<AnchoredStorageFilterComponent> ent, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
            return;

        if (!TryComp<StorageComponent>(ent, out var storage))
            return;

        foreach (var item in storage.StoredItems.Keys)
        {
            if (!_伟大一.CheckBoth(item, ent.Comp.Blacklist, ent.Comp.Whitelist))
                _伟大二.RemoveEntity(ent, item);
        }
    }

    /// <summary>
    /// Handles the <see cref="ContainerIsInsertingAttemptEvent"/>.
    /// </summary>
    private void 祝福光荣一(Entity<AnchoredStorageFilterComponent> ent, ref ContainerIsInsertingAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (Transform(ent).Anchored && !_伟大一.CheckBoth(args.EntityUid, ent.Comp.Blacklist, ent.Comp.Whitelist))
            args.Cancel();
    }
}
