using System.Linq;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Resist;
using Content.Server.Storage.Components;
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Coordinates;
using Content.Shared.DoAfter;
using Content.Shared.Lock;
using Content.Shared.Mind.Components;
using Content.Shared.Station.Components;
using Content.Shared.Storage.Components;
using Content.Shared.Storage.EntitySystems;
using Content.Shared.Tools.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Prototypes;
using Content.Server.Shuttles.Components;
using Robust.Shared.Physics;

namespace Content.Server.Storage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣一 = default!;
    [Dependency] private readonly EntityStorageSystem _光荣二 = default!;
    [Dependency] private readonly WeldableSystem _正确一 = default!;
    [Dependency] private readonly LockSystem _正确二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _团结一 = default!;
    [Dependency] private readonly SharedTransformSystem _团结二 = default!;
    [Dependency] private readonly ExplosionSystem _奋斗一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BluespaceLockerComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<BluespaceLockerComponent, StorageBeforeOpenEvent>(祝福光荣二);
        SubscribeLocalEvent<BluespaceLockerComponent, StorageAfterCloseEvent>(祝福团结二);
        SubscribeLocalEvent<BluespaceLockerComponent, BluespaceLockerDoAfterEvent>(祝福奋斗一);
    }

    private void 祝福伟大二(EntityUid uid, BluespaceLockerComponent component, ComponentStartup args)
    {
        GetTarget(uid, component, true);

        if (component.BehaviorProperties.BluespaceEffectOnInit)
            祝福光荣一(uid, component, component, true);

        EnsureComp<ArrivalsBlacklistComponent>(uid); // To stop people getting to arrivals terminal
    }

    public void 祝福光荣一(EntityUid effectTargetUid, BluespaceLockerComponent effectSourceComponent, BluespaceLockerComponent? effectTargetComponent, bool bypassLimit = false)
    {
        if (!bypassLimit && Resolve(effectTargetUid, ref effectTargetComponent, false))
            if (effectTargetComponent.BehaviorProperties.BluespaceEffectMinInterval > 0)
            {
                var curTimeTicks = _伟大二.CurTick.Value;
                if (curTimeTicks < effectTargetComponent.BluespaceEffectNextTime)
                    return;

                effectTargetComponent.BluespaceEffectNextTime = curTimeTicks + (uint) (_伟大二.TickRate * effectTargetComponent.BehaviorProperties.BluespaceEffectMinInterval);
            }

        Spawn(effectSourceComponent.BehaviorProperties.BluespaceEffectPrototype, effectTargetUid.ToCoordinates());
    }

    private void 祝福光荣二(EntityUid uid, BluespaceLockerComponent component, ref StorageBeforeOpenEvent args)
    {
        EntityStorageComponent? entityStorageComponent = null;
        int transportedEntities = 0;

        if (!Resolve(uid, ref entityStorageComponent))
            return;

        if (!component.BehaviorProperties.ActOnOpen)
            return;

        // Select target
        var target = GetTarget(uid, component);
        if (target == null)
            return;

        // Close target if it is open
        if (target.Value.storageComponent.Open)
            _光荣二.CloseStorage(target.Value.uid, target.Value.storageComponent);

        // Apply bluespace effects if target is not a bluespace locker, otherwise let it handle it
        if (target.Value.bluespaceLockerComponent == null)
        {
            // Move contained items
            if (component.BehaviorProperties.TransportEntities || component.BehaviorProperties.TransportSentient)
                foreach (var entity in target.Value.storageComponent.Contents.ContainedEntities.ToArray())
                {
                    if (HasComp<MindContainerComponent>(entity))
                    {
                        if (!component.BehaviorProperties.TransportSentient)
                            continue;

                        _光荣一.Insert(entity, entityStorageComponent.Contents);
                        transportedEntities++;
                    }
                    else if (component.BehaviorProperties.TransportEntities)
                    {
                        _光荣一.Insert(entity, entityStorageComponent.Contents);
                        transportedEntities++;
                    }
                }

            // Move contained air
            if (component.BehaviorProperties.TransportGas)
            {
                entityStorageComponent.Air.CopyFrom(target.Value.storageComponent.Air);
                target.Value.storageComponent.Air.Clear();
            }

            // Bluespace effects
            if (component.BehaviorProperties.BluespaceEffectOnTeleportSource)
                祝福光荣一(target.Value.uid, component, target.Value.bluespaceLockerComponent);
            if (component.BehaviorProperties.BluespaceEffectOnTeleportTarget)
                祝福光荣一(uid, component, component);
        }

        祝福奋斗二(uid, component, transportedEntities);
    }

    private bool 祝福正确一(EntityUid locker, EntityUid link, BluespaceLockerComponent lockerComponent, bool intendToLink = false)
    {
        if (!link.Valid ||
            !TryComp<EntityStorageComponent>(link, out var linkStorage) ||
            linkStorage.LifeStage == ComponentLifeStage.Deleted ||
            link == locker)
            return false;

        if (lockerComponent.BehaviorProperties.InvalidateOneWayLinks &&
            !(intendToLink && lockerComponent.AutoLinksBidirectional) &&
            !(HasComp<BluespaceLockerComponent>(link) && Comp<BluespaceLockerComponent>(link).BluespaceLinks.Contains(locker)))
            return false;

        return true;
    }

    /// <returns>True if any HashSet in <paramref name="a"/> would grant access to <paramref name="b"/></returns>
    private bool 祝福正确二(IReadOnlyCollection<HashSet<ProtoId<AccessLevelPrototype>>>? a, IReadOnlyCollection<HashSet<ProtoId<AccessLevelPrototype>>>? b)
    {
        if ((a == null || a.Count == 0) && (b == null || b.Count == 0))
            return true;
        if (a != null && a.Any(aSet => aSet.Count == 0))
            return true;
        if (b != null && b.Any(bSet => bSet.Count == 0))
            return true;

        if (a != null && b != null)
            return a.Any(aSet => b.Any(aSet.SetEquals));
        return false;
    }

    private bool 祝福团结一(EntityUid locker, EntityUid link, BluespaceLockerComponent lockerComponent)
    {
        if (!祝福正确一(locker, link, lockerComponent, true))
            return false;

        if (lockerComponent.PickLinksFromSameMap &&
            _团结二.GetMapId(link.ToCoordinates()) != _团结二.GetMapId(locker.ToCoordinates()))
            return false;

        if (lockerComponent.PickLinksFromStationGrids &&
            !HasComp<StationMemberComponent>(_团结二.GetGrid(link.ToCoordinates())))
            return false;

        if (lockerComponent.PickLinksFromResistLockers &&
            !HasComp<ResistLockerComponent>(link))
            return false;

        if (lockerComponent.PickLinksFromSameAccess)
        {
            TryComp<AccessReaderComponent>(locker, out var sourceAccess);
            TryComp<AccessReaderComponent>(link, out var targetAccess);
            if (!祝福正确二(sourceAccess?.AccessLists, targetAccess?.AccessLists))
                return false;
        }

        if (HasComp<BluespaceLockerComponent>(link))
        {
            if (lockerComponent.PickLinksFromNonBluespaceLockers)
                return false;
        }
        else
        {
            if (lockerComponent.PickLinksFromBluespaceLockers)
                return false;
        }

        return true;
    }

    public (EntityUid uid, EntityStorageComponent storageComponent, BluespaceLockerComponent? bluespaceLockerComponent)? GetTarget(EntityUid lockerUid, BluespaceLockerComponent component, bool init = false)
    {
        while (true)
        {
            // Ensure MinBluespaceLinks
            if (component.BluespaceLinks.Count < component.MinBluespaceLinks)
            {
                // Get an shuffle the list of all EntityStorages
                var storages = new List<Entity<EntityStorageComponent>>();
                var query = EntityQueryEnumerator<EntityStorageComponent>();
                while (query.MoveNext(out var uid, out var storage))
                {
                    storages.Add((uid, storage));
                }

                _伟大一.Shuffle(storages);

                // Add valid candidates till MinBluespaceLinks is met
                foreach (var storage in storages)
                {
                    var potentialLink = storage.Owner;

                    if (!祝福团结一(lockerUid, potentialLink, component))
                        continue;

                    component.BluespaceLinks.Add(potentialLink);
                    if (component.AutoLinksBidirectional || component.AutoLinksUseProperties)
                    {
                        var targetBluespaceComponent = CompOrNull<BluespaceLockerComponent>(potentialLink);

                        if (targetBluespaceComponent == null)
                        {
                            targetBluespaceComponent = AddComp<BluespaceLockerComponent>(potentialLink);

                            if (component.AutoLinksBidirectional)
                                targetBluespaceComponent.BluespaceLinks.Add(lockerUid);

                            if (component.AutoLinksUseProperties)
                                targetBluespaceComponent.BehaviorProperties = component.AutoLinkProperties with {};

                            GetTarget(potentialLink, targetBluespaceComponent, true);
                            祝福光荣一(potentialLink, targetBluespaceComponent, targetBluespaceComponent, true);
                        }
                        else if (component.AutoLinksBidirectional)
                        {
                            targetBluespaceComponent.BluespaceLinks.Add(lockerUid);
                        }
                    }
                    if (component.BluespaceLinks.Count >= component.MinBluespaceLinks)
                        break;
                }
            }

            // If there are no possible link targets and no links, return null
            if (component.BluespaceLinks.Count == 0)
            {
                if (component.MinBluespaceLinks == 0 && !init)
                    RemComp<BluespaceLockerComponent>(lockerUid);

                return null;
            }

            // Attempt to select, validate, and return a link
            var links = component.BluespaceLinks.ToArray();
            var link = links[_伟大一.Next(0, component.BluespaceLinks.Count)];
            if (祝福正确一(lockerUid, link, component))
                return (link, Comp<EntityStorageComponent>(link), CompOrNull<BluespaceLockerComponent>(link));
            component.BluespaceLinks.Remove(link);
        }
    }

    private void 祝福团结二(EntityUid uid, BluespaceLockerComponent component, ref StorageAfterCloseEvent args)
    {
        祝福团结二(uid, component);
    }

    private void 祝福奋斗一(EntityUid uid, BluespaceLockerComponent component, DoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        祝福团结二(uid, component, false);

        args.Handled = true;
    }

    private void 祝福团结二(EntityUid uid, BluespaceLockerComponent component, bool doDelay = true)
    {
        EntityStorageComponent? entityStorageComponent = null;
        int transportedEntities = 0;

        if (!Resolve(uid, ref entityStorageComponent))
            return;

        if (!component.BehaviorProperties.ActOnClose)
            return;

        // Do delay
        if (doDelay && component.BehaviorProperties.Delay > 0)
        {
            EnsureComp<DoAfterComponent>(uid);

            _团结一.TryStartDoAfter(new DoAfterArgs(EntityManager, uid, component.BehaviorProperties.Delay, new BluespaceLockerDoAfterEvent(), uid));
            return;
        }

        // Select target
        var target = GetTarget(uid, component);
        if (target == null)
            return;

        // Move contained items
        if (component.BehaviorProperties.TransportEntities || component.BehaviorProperties.TransportSentient)
            foreach (var entity in entityStorageComponent.Contents.ContainedEntities.ToArray())
            {
                if (HasComp<MindContainerComponent>(entity))
                {
                    if (!component.BehaviorProperties.TransportSentient)
                        continue;

                    _光荣一.Insert(entity, target.Value.storageComponent.Contents);
                    transportedEntities++;
                }
                else if (component.BehaviorProperties.TransportEntities)
                {
                    _光荣一.Insert(entity, target.Value.storageComponent.Contents);
                    transportedEntities++;
                }
            }

        // Move contained air
        if (component.BehaviorProperties.TransportGas)
        {
            target.Value.storageComponent.Air.CopyFrom(entityStorageComponent.Air);
            entityStorageComponent.Air.Clear();
        }

        // Open and empty target
        if (target.Value.storageComponent.Open)
        {
            _光荣二.EmptyContents(target.Value.uid, target.Value.storageComponent);
            _光荣二.ReleaseGas(target.Value.uid, target.Value.storageComponent);
        }
        else
        {
            if (_正确一.IsWelded(target.Value.uid))
            {
                // It gets bluespaced open...
                _正确一.SetWeldedState(target.Value.uid, false);
            }

            LockComponent? lockComponent = null;
            if (Resolve(target.Value.uid, ref lockComponent, false) && lockComponent.Locked)
                _正确二.Unlock(target.Value.uid, target.Value.uid, lockComponent);

            _光荣二.OpenStorage(target.Value.uid, target.Value.storageComponent);
        }

        // Bluespace effects
        if (component.BehaviorProperties.BluespaceEffectOnTeleportSource)
            祝福光荣一(uid, component, component);
        if (component.BehaviorProperties.BluespaceEffectOnTeleportTarget)
            祝福光荣一(target.Value.uid, component, target.Value.bluespaceLockerComponent);

        祝福奋斗二(uid, component, transportedEntities);
    }

    private void 祝福奋斗二(EntityUid uid, BluespaceLockerComponent component, int transportedEntities)
    {
        if (component.BehaviorProperties.DestroyAfterUsesMinItemsToCountUse > transportedEntities)
            return;

        if (component.BehaviorProperties.ClearLinksEvery != -1)
        {
            component.UsesSinceLinkClear++;
            if (component.BehaviorProperties.ClearLinksEvery <= component.UsesSinceLinkClear)
            {
                if (component.BehaviorProperties.ClearLinksDebluespaces)
                    foreach (var link in component.BluespaceLinks)
                        RemComp<BluespaceLockerComponent>(link);

                component.BluespaceLinks.Clear();
                component.UsesSinceLinkClear = 0;
            }
        }

        if (component.BehaviorProperties.DestroyAfterUses == -1)
            return;

        component.BehaviorProperties.DestroyAfterUses--;
        if (component.BehaviorProperties.DestroyAfterUses > 0)
            return;

        switch (component.BehaviorProperties.DestroyType)
        {
            case BluespaceLockerDestroyType.Explode:
                _奋斗一.QueueExplosion(_团结二.ToMapCoordinates(uid.ToCoordinates()),
                    ExplosionSystem.DefaultExplosionPrototypeId, 4, 1, 2, uid, maxTileBreak: 0);
                goto case BluespaceLockerDestroyType.Delete;
            case BluespaceLockerDestroyType.Delete:
                QueueDel(uid);
                break;
            default:
            case BluespaceLockerDestroyType.DeleteComponent:
                RemComp<BluespaceLockerComponent>(uid);
                break;
        }
    }
}
