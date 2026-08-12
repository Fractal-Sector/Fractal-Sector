using Content.Shared.Storage.Components;
using Content.Shared.Storage.Events;

namespace Content.Shared.Storage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedStorageSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<StoreAfterFailedInteractComponent, StorageInsertFailedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<StoreAfterFailedInteractComponent> ent, ref StorageInsertFailedEvent args)
    {
        _伟大一.PlayerInsertHeldEntity(args.Storage, args.Player);
    }
}
