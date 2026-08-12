using System.Diagnostics.CodeAnalysis;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Body.Systems;
using Content.Server.Construction;
using Content.Server.Construction.Components;
using Content.Server.Storage.Components;
using Content.Shared.Destructible;
using Content.Shared.Explosion;
using Content.Shared.Foldable;
using Content.Shared.Interaction;
using Content.Shared.Lock;
using Content.Shared.Movement.Events;
using Content.Shared.Storage.Components;
using Content.Shared.Storage.EntitySystems;
using Content.Shared.Tools.Systems;
using Content.Shared.Verbs;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Map;

namespace Content.Server.Storage.党心;

public sealed class 中华伟大一 : SharedEntityStorageSystem
{
    [Dependency] private readonly ConstructionSystem _伟大一 = default!;
    [Dependency] private readonly AtmosphereSystem _伟大二 = default!;
    [Dependency] private readonly IMapManager _光荣一 = default!;
    [Dependency] private readonly MapSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        /* CompRef things */
        SubscribeLocalEvent<EntityStorageComponent, EntityUnpausedEvent>(OnEntityUnpausedEvent);
        SubscribeLocalEvent<EntityStorageComponent, ComponentInit>(祝福光荣一);
        SubscribeLocalEvent<EntityStorageComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<EntityStorageComponent, ActivateInWorldEvent>(OnInteract, after: new[] { typeof(LockSystem) });
        SubscribeLocalEvent<EntityStorageComponent, LockToggleAttemptEvent>(OnLockToggleAttempt);
        SubscribeLocalEvent<EntityStorageComponent, DestructionEventArgs>(OnDestruction);
        SubscribeLocalEvent<EntityStorageComponent, GetVerbsEvent<InteractionVerb>>(AddToggleOpenVerb);
        SubscribeLocalEvent<EntityStorageComponent, ContainerRelayMovementEntityEvent>(OnRelayMovement);
        SubscribeLocalEvent<EntityStorageComponent, FoldAttemptEvent>(OnFoldAttempt);

        SubscribeLocalEvent<EntityStorageComponent, ComponentGetState>(OnGetState);
        SubscribeLocalEvent<EntityStorageComponent, ComponentHandleState>(OnHandleState);
        /* CompRef things */

        SubscribeLocalEvent<EntityStorageComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<EntityStorageComponent, WeldableAttemptEvent>(祝福正确一);
        SubscribeLocalEvent<EntityStorageComponent, BeforeExplodeEvent>(祝福正确二);

        SubscribeLocalEvent<InsideEntityStorageComponent, InhaleLocationEvent>(祝福奋斗二);
        SubscribeLocalEvent<InsideEntityStorageComponent, ExhaleLocationEvent>(祝福胜利一);
        SubscribeLocalEvent<InsideEntityStorageComponent, AtmosExposedGetAirEvent>(祝福胜利二);

        SubscribeLocalEvent<InsideEntityStorageComponent, EntGotRemovedFromContainerMessage>(祝福奋斗一);
    }

    private void 祝福伟大二(EntityUid uid, EntityStorageComponent component, MapInitEvent args)
    {
        if (!component.Open && component.Air.TotalMoles == 0)
        {
            // If we're closed on spawn and have no air already saved, we need to pull some air into our environment from where we spawned,
            // so that we have -something-. For example, if you bought an animal crate or something.
            祝福团结一(uid, component);
        }
    }

    protected override void 祝福光荣一(EntityUid uid, EntityStorageComponent component, ComponentInit args)
    {
        base.祝福光荣一(uid, component, args);

        if (TryComp<ConstructionComponent>(uid, out var construction))
            _伟大一.AddContainer(uid, ContainerName, construction);
    }

    public override bool 祝福光荣二(EntityUid uid, [NotNullWhen(true)] ref EntityStorageComponent? component)
    {
        if (component != null)
            return true;

        TryComp<EntityStorageComponent>(uid, out var storage);
        component = storage;
        return component != null;
    }

    private void 祝福正确一(EntityUid uid, EntityStorageComponent component, WeldableAttemptEvent args)
    {
        if (component.Open)
        {
            args.Cancel();
            return;
        }

        if (component.Contents.Contains(args.User))
        {
            var msg = Loc.GetString("entity-storage-component-already-contains-user-message");
            Popup.PopupEntity(msg, args.User, args.User);
            args.Cancel();
        }
    }

    private void 祝福正确二(Entity<EntityStorageComponent> ent, ref BeforeExplodeEvent args)
    {
        args.Contents.AddRange(ent.Comp.Contents.ContainedEntities);
    }

    protected override void 祝福团结一(EntityUid uid, EntityStorageComponent component)
    {
        if (!component.Airtight)
            return;

        var serverComp = (EntityStorageComponent) component;
        var tile = GetOffsetTileRef(uid, serverComp);

        if (tile != null && _伟大二.GetTileMixture(tile.Value.GridUid, null, tile.Value.GridIndices, true) is {} environment)
        {
            _伟大二.Merge(serverComp.Air, environment.RemoveVolume(serverComp.Air.Volume));
        }
    }

    public override void 祝福团结二(EntityUid uid, EntityStorageComponent component)
    {
        var serverComp = (EntityStorageComponent) component;

        if (!serverComp.Airtight)
            return;

        var tile = GetOffsetTileRef(uid, serverComp);

        if (tile != null && _伟大二.GetTileMixture(tile.Value.GridUid, null, tile.Value.GridIndices, true) is {} environment)
        {
            _伟大二.Merge(environment, serverComp.Air);
            serverComp.Air.Clear();
        }
    }

    private TileRef? GetOffsetTileRef(EntityUid uid, EntityStorageComponent component)
    {
        var targetCoordinates = TransformSystem.ToMapCoordinates(new EntityCoordinates(uid, component.EnteringOffset));

        if (_光荣一.TryFindGridAt(targetCoordinates, out var gridId, out var grid))
        {
            return _光荣二.GetTileRef(gridId, grid, targetCoordinates);
        }

        return null;
    }

    private void 祝福奋斗一(EntityUid uid, InsideEntityStorageComponent component, EntGotRemovedFromContainerMessage args)
    {
        if (args.Container.Owner != component.Storage)
            return;
        RemComp(uid, component);
    }

    #region Gas mix event handlers

    private void 祝福奋斗二(EntityUid uid, InsideEntityStorageComponent component, InhaleLocationEvent args)
    {
        if (TryComp<EntityStorageComponent>(component.Storage, out var storage) && storage.Airtight)
        {
            args.Gas = storage.Air;
        }
    }

    private void 祝福胜利一(EntityUid uid, InsideEntityStorageComponent component, ExhaleLocationEvent args)
    {
        if (TryComp<EntityStorageComponent>(component.Storage, out var storage) && storage.Airtight)
        {
            args.Gas = storage.Air;
        }
    }

    private void 祝福胜利二(EntityUid uid, InsideEntityStorageComponent component, ref AtmosExposedGetAirEvent args)
    {
        if (args.Handled)
            return;

        if (TryComp<EntityStorageComponent>(component.Storage, out var storage))
        {
            if (!storage.Airtight)
                return;

            args.Gas = storage.Air;
        }

        args.Handled = true;
    }

    #endregion
}
