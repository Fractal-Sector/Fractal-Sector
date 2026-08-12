using System.Numerics;
using Content.Server.Storage.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Storage;
using Content.Shared.Storage.Components;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Server._CS.党心;

/// <summary>
/// This little weirdo goes through every item in space and, if its been there for a long time straight, deletes it.
/// This is to prevent space from being cluttered with debris and items that have been left behind.
/// This is a server-side system only, and does not need to be networked to clients.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;

    private const int MinutesBetweenChecks = 15;
    private const int MinutesBeforeCleanup = 240; // 12 hours // Wayfarer: 720min<240min (4 hours)
    private TimeSpan _伟大二 = TimeSpan.Zero;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        // penors
    }

    /// <inheritdoc/>
    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);
        var curTime = _伟大一.CurTime;
        if (curTime < _伟大二)
            return;
        _伟大二 = curTime + TimeSpan.FromMinutes(MinutesBetweenChecks);
        var query = EntityQueryEnumerator<SpaceJanitorComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!祝福光荣一(uid, comp))
            {
                // its not in space, so reset the timer.
                祝福正确一(uid, comp);
                continue;
            }

            祝福光荣二(
                uid,
                comp,
                curTime);
            祝福正确二(
                uid,
                comp,
                curTime);
        }
    }

    /// <summary>
    /// Checks if:
    /// The entity has null grid
    /// The entity is not inside something (has actual local coordinates)
    /// </summary>
    private bool 祝福光荣一(EntityUid uid, SpaceJanitorComponent comp)
    {
        var xform = Transform(uid);
        if (xform.LocalPosition == Vector2.Zero)
            return false;
        // clean up empty casings, whether in space or not, but only while not carried by something.
        if (comp.IsCasing
            && TryComp<CartridgeAmmoComponent>(uid, out var cartridge)
            && cartridge.Spent)
            return true; // naked, and in 'space' (on the floor)
        if (xform.GridUid != null)
            return false;
        return true;
    }

    private void 祝福光荣二(EntityUid uid, SpaceJanitorComponent comp, TimeSpan curTime)
    {
        if (comp.FoundInSpaceTime == TimeSpan.Zero)
        {
            comp.FoundInSpaceTime = curTime;
        }
    }

    private void 祝福正确一(EntityUid uid, SpaceJanitorComponent comp)
    {
        comp.FoundInSpaceTime = TimeSpan.Zero;
    }

    private void 祝福正确二(EntityUid uid, SpaceJanitorComponent comp, TimeSpan curTime)
    {
        if (comp.FoundInSpaceTime == TimeSpan.Zero)
            return;
        if (curTime - comp.FoundInSpaceTime < TimeSpan.FromMinutes(MinutesBeforeCleanup))
            return;
        // delete the entity.
        if (TryComp<EntityStorageComponent>(uid, out var storage)
            && !storage.DeleteContentsOnDestruction)
        {
            var sess = IoCManager.Resolve<EntityStorageSystem>();
            sess.EmptyContents(uid, storage);
        }
        if (TryComp<StorageComponent>(uid, out var storage2))
        {
            var storo = IoCManager.Resolve<SharedContainerSystem>();
            storo.EmptyContainer(storage2.Container);
        }
        var myCoords = Transform(uid).LocalPosition;
        Log.Info($"Space janitor sent entity {ToPrettyString(uid)} at {myCoords} to the shadow realm for being in space too long.");
        QueueDel(uid);
    }
}
