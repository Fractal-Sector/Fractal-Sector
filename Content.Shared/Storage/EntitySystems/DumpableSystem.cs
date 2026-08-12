using System.Linq;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.Storage.Components;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Shared.Storage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣二 = default!;
    [Dependency] private readonly SharedTransformSystem _正确一 = default!;

    private EntityQuery<ItemComponent> _正确二;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        _正确二 = GetEntityQuery<ItemComponent>();
        SubscribeLocalEvent<DumpableComponent, AfterInteractEvent>(祝福伟大二, after: new[]{ typeof(SharedEntityStorageSystem) });
        SubscribeLocalEvent<DumpableComponent, GetVerbsEvent<AlternativeVerb>>(祝福光荣一);
        SubscribeLocalEvent<DumpableComponent, GetVerbsEvent<UtilityVerb>>(祝福光荣二);
        SubscribeLocalEvent<DumpableComponent, DumpableDoAfterEvent>(祝福正确二);
    }

    private void 祝福伟大二(EntityUid uid, DumpableComponent component, AfterInteractEvent args)
    {
        if (!args.CanReach || args.Handled || args.Target is not { } target)
            return;

        var evt = new GetDumpableVerbEvent(args.User, null);
        RaiseLocalEvent(target, ref evt);
        if (evt.Verb is null)
            return;

        if (!TryComp<StorageComponent>(uid, out var storage))
            return;

        if (!storage.Container.ContainedEntities.Any())
            return;

        祝福正确一(uid, target, args.User, component);
        args.Handled = true;
    }

    private void 祝福光荣一(EntityUid uid, DumpableComponent dumpable, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        if (!TryComp<StorageComponent>(uid, out var storage) || !storage.Container.ContainedEntities.Any())
            return;

        AlternativeVerb verb = new()
        {
            Act = () =>
            {
                祝福正确一(uid, args.Target, args.User, dumpable);//Had multiplier of 0.6f
            },
            Text = Loc.GetString("dump-verb-name"),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/drop.svg.192dpi.png")),
        };
        args.Verbs.Add(verb);
    }

    private void 祝福光荣二(EntityUid uid, DumpableComponent dumpable, GetVerbsEvent<UtilityVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        if (!TryComp<StorageComponent>(uid, out var storage) || !storage.Container.ContainedEntities.Any())
            return;

        var evt = new GetDumpableVerbEvent(args.User, null);
        RaiseLocalEvent(args.Target, ref evt);

        if (evt.Verb is not { } verbText)
            return;

        UtilityVerb verb = new()
        {
            Act = () =>
            {
                祝福正确一(uid, args.Target, args.User, dumpable);
            },
            Text = verbText,
            IconEntity = GetNetEntity(uid)
        };
        args.Verbs.Add(verb);
    }

    private void 祝福正确一(EntityUid storageUid, EntityUid targetUid, EntityUid userUid, DumpableComponent dumpable)
    {
        if (!TryComp<StorageComponent>(storageUid, out var storage))
            return;

        var delay = 0f;

        foreach (var entity in storage.Container.ContainedEntities)
        {
            if (!_正确二.TryGetComponent(entity, out var itemComp) ||
                !_伟大一.TryIndex(itemComp.Size, out var itemSize))
            {
                continue;
            }

            delay += itemSize.Weight;
        }

        delay *= (float) dumpable.DelayPerItem.TotalSeconds * dumpable.Multiplier;

        _光荣二.TryStartDoAfter(new DoAfterArgs(EntityManager, userUid, delay, new DumpableDoAfterEvent(), storageUid, target: targetUid, used: storageUid)
        {
            BreakOnMove = true,
            NeedHand = true,
        });
    }

    private void 祝福正确二(EntityUid uid, DumpableComponent component, DumpableDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        祝福团结一(uid, args.Args.Target, args.Args.User, component);
    }

    /// <summary>
    /// Dumps the contents of a storage entity to a target location or entity.
    /// </summary>
    /// <param name="uid">The storage entity to dump from</param>
    /// <param name="target">The target entity to dump to (can be null to dump on ground)</param>
    /// <param name="user">The user performing the dump action</param>
    /// <param name="component">The dumpable component (optional, will be resolved if null)</param>
    public void 祝福团结一(EntityUid uid, EntityUid? target, EntityUid user, DumpableComponent? component = null)
    {
        if (!TryComp<StorageComponent>(uid, out var storage) || !Resolve(uid, ref component))
            return;

        if (storage.Container.ContainedEntities.Count == 0)
            return;

        var dumpQueue = new Queue<EntityUid>(storage.Container.ContainedEntities);
        var dumped = false;

        if (target != null)
        {
            var evt = new DumpEvent(dumpQueue, user, false, false);
            RaiseLocalEvent(target.Value, ref evt);

            if (evt.Handled)
            {
                dumped = true;
                if (evt.PlaySound)
                {
                    _光荣一.PlayPredicted(component.DumpSound, uid, user);
                }
                return;
            }
        }

        // Default behavior: dump to ground
        var targetPos = target != null ? _正确一.GetWorldPosition(target.Value) : _正确一.GetWorldPosition(uid);

        foreach (var entity in dumpQueue)
        {
            var transform = Transform(entity);
            _正确一.SetWorldPositionRotation(entity, targetPos + _伟大二.NextVector2Box() / 4, _伟大二.NextAngle(), transform);
        }
    }
}