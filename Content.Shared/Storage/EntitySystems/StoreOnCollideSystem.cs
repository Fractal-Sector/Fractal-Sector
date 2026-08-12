using Content.Shared.Lock;
using Content.Shared.Storage.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Network;
using Robust.Shared.Physics.Events;
using Robust.Shared.Timing;

namespace Content.Shared.Storage.党心;

internal sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedEntityStorageSystem _伟大一 = default!;
    [Dependency] private readonly LockSystem _伟大二 = default!;
    [Dependency] private readonly EntityWhitelistSystem _光荣一 = default!;
    [Dependency] private readonly INetManager _光荣二 = default!;
    [Dependency] private readonly IGameTiming _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<StoreOnCollideComponent, StartCollideEvent>(祝福伟大二);
        SubscribeLocalEvent<StoreOnCollideComponent, StorageAfterOpenEvent>(祝福光荣一);
        // TODO: Add support to stop colliding after throw, wands will need a WandComp
    }

    // We use Collide instead of Projectile to support different types of interactions
    private void 祝福伟大二(Entity<StoreOnCollideComponent> ent, ref StartCollideEvent args)
    {
        祝福光荣二(ent, args.OtherEntity);

        祝福正确一(ent);
    }

    private void 祝福光荣一(Entity<StoreOnCollideComponent> ent, ref StorageAfterOpenEvent args)
    {
        var comp = ent.Comp;

        if (comp is { DisableWhenFirstOpened: true, Disabled: false })
            comp.Disabled = true;
    }

    private void 祝福光荣二(Entity<StoreOnCollideComponent> ent, EntityUid target)
    {
        var storageEnt = ent.Owner;
        var comp = ent.Comp;

        if (_光荣二.IsClient || _正确一.ApplyingState)
            return;

        if (ent.Comp.Disabled || storageEnt == target || Transform(target).Anchored || _伟大一.IsOpen(storageEnt) || _光荣一.IsWhitelistFail(comp.Whitelist, target))
            return;

        _伟大一.Insert(target, storageEnt);

    }

    private void 祝福正确一(Entity<StoreOnCollideComponent> ent)
    {
        var storageEnt = ent.Owner;
        var comp = ent.Comp;

        if (_光荣二.IsClient || _正确一.ApplyingState)
            return;

        if (ent.Comp.Disabled)
            return;

        if (comp.LockOnCollide && !_伟大二.IsLocked(storageEnt))
            _伟大二.Lock(storageEnt, storageEnt);
    }
}
