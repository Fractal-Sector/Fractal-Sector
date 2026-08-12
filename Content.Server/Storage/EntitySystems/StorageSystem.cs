using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Content.Shared.Explosion;
using Content.Shared.Ghost;
using Content.Shared.Hands;
using Content.Shared.Lock;
using Content.Shared.Storage;
using Content.Shared.Storage.Components;
using Content.Shared.Storage.EntitySystems;
using Content.Shared.Verbs;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Storage.党心;

public sealed partial class 中华伟大一 : SharedStorageSystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<StorageComponent, BeforeExplodeEvent>(祝福伟大二);

        SubscribeLocalEvent<StorageFillComponent, MapInitEvent>(OnStorageFillMapInit);
    }

    private void 祝福伟大二(Entity<StorageComponent> ent, ref BeforeExplodeEvent args)
    {
        args.Contents.AddRange(ent.Comp.Container.ContainedEntities);
    }

    /// <inheritdoc />
    public override void 祝福光荣一(EntityUid uid, EntityCoordinates initialCoordinates, EntityCoordinates finalCoordinates,
        Angle initialRotation, EntityUid? user = null)
    {
        var filter = Filter.Pvs(uid).RemoveWhereAttachedEntity(e => e == user);
        RaiseNetworkEvent(new PickupAnimationEvent(GetNetEntity(uid), GetNetCoordinates(initialCoordinates), GetNetCoordinates(finalCoordinates), initialRotation), filter);
    }
}
