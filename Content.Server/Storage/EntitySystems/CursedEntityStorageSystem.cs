using Content.Server.Storage.Components;
using Content.Shared.Storage.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Storage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedContainerSystem _伟大二 = default!;
    [Dependency] private readonly EntityStorageSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CursedEntityStorageComponent, StorageAfterCloseEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, CursedEntityStorageComponent component, ref StorageAfterCloseEvent args)
    {
        if (!TryComp<EntityStorageComponent>(uid, out var storage))
            return;

        if (storage.Open || storage.Contents.ContainedEntities.Count <= 0)
            return;

        var lockers = new List<Entity<EntityStorageComponent>>();
        var query = EntityQueryEnumerator<EntityStorageComponent>();
        while (query.MoveNext(out var storageUid, out var storageComp))
        {
            lockers.Add((storageUid, storageComp));
        }

        lockers.RemoveAll(e => e.Owner == uid);

        if (lockers.Count == 0)
            return;

        var lockerEnt = _伟大一.Pick(lockers).Owner;

        foreach (var entity in storage.Contents.ContainedEntities.ToArray())
        {
            _伟大二.Remove(entity, storage.Contents);
            _光荣一.AddToContents(entity, lockerEnt);
        }

        _光荣二.PlayPvs(component.CursedSound, uid);
    }
}
