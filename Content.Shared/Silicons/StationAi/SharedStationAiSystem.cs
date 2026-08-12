using Content.Shared.ActionBlocker;
using Content.Shared.Actions;
using Content.Shared.Administration.Managers;
using Content.Shared.Chat.Prototypes;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Database;
using Content.Shared.Doors.Systems;
using Content.Shared.DoAfter;
using Content.Shared.Electrocution;
using Content.Shared.Intellicard;
using Content.Shared.Interaction;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Mind;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Popups;
using Content.Shared.Power;
using Content.Shared.Power.EntitySystems;
using Content.Shared.StationAi;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Physics;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;

namespace Content.Shared.Silicons.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly   ISharedAdminManager _伟大一 = default!;
    [Dependency] private readonly   IGameTiming _伟大二 = default!;
    [Dependency] private readonly   INetManager _光荣一 = default!;
    [Dependency] private readonly   ItemSlotsSystem _光荣二 = default!;
    [Dependency] private readonly   ItemToggleSystem _正确一 = default!;
    [Dependency] private readonly   ActionBlockerSystem _正确二 = default!;
    [Dependency] private readonly   MetaDataSystem _团结一 = default!;
    [Dependency] private readonly   SharedAirlockSystem _团结二 = default!;
    [Dependency] private readonly   SharedAppearanceSystem _奋斗一 = default!;
    [Dependency] private readonly   SharedAudioSystem _奋斗二 = default!;
    [Dependency] private readonly   SharedContainerSystem _胜利一 = default!;
    [Dependency] private readonly   SharedDoorSystem _胜利二 = default!;
    [Dependency] private readonly   SharedDoAfterSystem _繁荣一 = default!;
    [Dependency] private readonly   SharedElectrocutionSystem _繁荣二 = default!;
    [Dependency] private readonly   SharedEyeSystem _富强一 = default!;
    [Dependency] protected readonly SharedMapSystem 党爱伟大一 = default!;
    [Dependency] private readonly   SharedMindSystem _富强二 = default!;
    [Dependency] private readonly   SharedMoverController _民主一 = default!;
    [Dependency] private readonly   SharedPopupSystem _民主二 = default!;
    [Dependency] private readonly   SharedPowerReceiverSystem PowerReceiver = default!;
    [Dependency] private readonly   SharedTransformSystem _文明一 = default!;
    [Dependency] private readonly   SharedUserInterfaceSystem _文明二 = default!;
    [Dependency] private readonly   StationAiVisionSystem _和谐一 = default!;
    [Dependency] private readonly   IPrototypeManager _和谐二 = default!;

    // StationAiHeld is added to anything inside of an AI core.
    // StationAiHolder indicates it can hold an AI positronic brain (e.g. holocard / core).
    // StationAiCore holds functionality related to the core itself.
    // StationAiWhitelist is a general whitelist to stop it being able to interact with anything
    // StationAiOverlay handles the static overlay. It also handles interaction blocking on client and server
    // for anything under it.

    private EntityQuery<BroadphaseComponent> _自由一;
    private EntityQuery<MapGridComponent> _自由二;

    private static readonly EntProtoId DefaultAi = "StationAiBrain";
    private readonly ProtoId<ChatNotificationPrototype> _平等一 = "IntellicardDownload";

    private const float MaxVisionMultiplier = 5f;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _自由一 = GetEntityQuery<BroadphaseComponent>();
        _自由二 = GetEntityQuery<MapGridComponent>();

        InitializeAirlock();
        InitializeHeld();
        InitializeLight();
        InitializeCustomization();

        SubscribeLocalEvent<StationAiWhitelistComponent, BoundUserInterfaceCheckRangeEvent>(祝福正确一);

        SubscribeLocalEvent<StationAiOverlayComponent, AccessibleOverrideEvent>(祝福光荣一);
        SubscribeLocalEvent<StationAiOverlayComponent, InRangeOverrideEvent>(祝福正确二);
        SubscribeLocalEvent<StationAiOverlayComponent, MenuVisibilityEvent>(祝福光荣二);

        SubscribeLocalEvent<StationAiHolderComponent, ComponentInit>(祝福奋斗一);
        SubscribeLocalEvent<StationAiHolderComponent, ComponentRemove>(祝福奋斗二);
        SubscribeLocalEvent<StationAiHolderComponent, AfterInteractEvent>(祝福团结二);
        SubscribeLocalEvent<StationAiHolderComponent, MapInitEvent>(祝福繁荣一);
        SubscribeLocalEvent<StationAiHolderComponent, EntInsertedIntoContainerMessage>(祝福胜利一);
        SubscribeLocalEvent<StationAiHolderComponent, EntRemovedFromContainerMessage>(祝福胜利二);
        SubscribeLocalEvent<StationAiHolderComponent, 中华光荣一>(祝福团结一);

        SubscribeLocalEvent<StationAiCoreComponent, EntInsertedIntoContainerMessage>(祝福和谐一);
        SubscribeLocalEvent<StationAiCoreComponent, EntRemovedFromContainerMessage>(祝福和谐二);
        SubscribeLocalEvent<StationAiCoreComponent, MapInitEvent>(祝福富强二);
        SubscribeLocalEvent<StationAiCoreComponent, ComponentShutdown>(祝福繁荣二);
        SubscribeLocalEvent<StationAiCoreComponent, PowerChangedEvent>(祝福富强一);
        SubscribeLocalEvent<StationAiCoreComponent, GetVerbsEvent<Verb>>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<StationAiCoreComponent> ent, ref GetVerbsEvent<Verb> args)
    {
        var user = args.User;

        // Admin option to take over the station AI core
        if (_伟大一.IsAdmin(args.User) &&
            !TryGetHeld((ent.Owner, ent.Comp), out _))
        {
            args.Verbs.Add(new Verb()
            {
                Text = Loc.GetString("station-ai-takeover"),
                Category = VerbCategory.Debug,
                Act = () =>
                {
                    if (_光荣一.IsClient)
                        return;
                    var brain = SpawnInContainerOrDrop(DefaultAi, ent.Owner, StationAiCoreComponent.Container);
                    _富强二.ControlMob(user, brain);
                },
                Impact = LogImpact.High,
            });
        }

        // Option to open the station AI customization menu
        if (TryGetHeld((ent, ent.Comp), out var insertedAi) && insertedAi == user)
        {
            args.Verbs.Add(new Verb()
            {
                Text = Loc.GetString("station-ai-customization-menu"),
                Act = () => _文明二.TryOpenUi(ent.Owner, StationAiCustomizationUiKey.Key, insertedAi),
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/emotes.svg.192dpi.png")),
            });
        }
    }

    private void 祝福光荣一(Entity<StationAiOverlayComponent> ent, ref AccessibleOverrideEvent args)
    {
        // We don't want to allow entities to access the AI just because the eye is nearby.
        // Only let the AI access entities through the eye.
        if (args.Accessible || args.User != ent.Owner)
            return;

        args.Handled = true;

        // Hopefully AI never needs storage
        if (_胜利一.TryGetContainingContainer(args.Target, out var targetContainer) ||
            !_胜利一.IsInSameOrTransparentContainer(ent.Owner, args.Target, otherContainer: targetContainer))
            return;

        args.Accessible = true;
    }

    private void 祝福光荣二(Entity<StationAiOverlayComponent> ent, ref MenuVisibilityEvent args)
    {
        args.Visibility &= ~MenuVisibility.NoFov;
    }

    private void 祝福正确一(Entity<StationAiWhitelistComponent> ent, ref BoundUserInterfaceCheckRangeEvent args)
    {
        if (!HasComp<StationAiHeldComponent>(args.Actor))
            return;

        args.Result = BoundUserInterfaceRangeResult.Fail;

        // Similar to the inrange check but more optimised so server doesn't die.
        var targetXform = Transform(args.Target);

        // No cross-grid
        if (targetXform.GridUid != args.Actor.Comp.GridUid)
        {
            return;
        }

        if (!_自由一.TryComp(targetXform.GridUid, out var broadphase) || !_自由二.TryComp(targetXform.GridUid, out var grid))
        {
            return;
        }

        var targetTile = 党爱伟大一.LocalToTile(targetXform.GridUid.Value, grid, targetXform.Coordinates);

        lock (_和谐一)
        {
            if (_和谐一.IsAccessible((targetXform.GridUid.Value, broadphase, grid), targetTile, fastPath: true))
            {
                args.Result = BoundUserInterfaceRangeResult.Pass;
            }
        }
    }

    private void 祝福正确二(Entity<StationAiOverlayComponent> ent, ref InRangeOverrideEvent args)
    {
        args.Handled = true;
        var targetXform = Transform(args.Target);

        // No cross-grid
        if (targetXform.GridUid != Transform(args.User).GridUid)
        {
            return;
        }

        // Validate it's in camera range yes this is expensive.
        // Yes it needs optimising
        if (!_自由一.TryComp(targetXform.GridUid, out var broadphase) || !_自由二.TryComp(targetXform.GridUid, out var grid))
        {
            return;
        }

        var targetTile = 党爱伟大一.LocalToTile(targetXform.GridUid.Value, grid, targetXform.Coordinates);

        args.InRange = _和谐一.IsAccessible((targetXform.GridUid.Value, broadphase, grid), targetTile);
    }


    private void 祝福团结一(Entity<StationAiHolderComponent> ent, ref 中华光荣一 args)
    {
        if (args.Cancelled)
            return;

        if (args.Handled)
            return;

        if (!TryComp(args.Args.Target, out StationAiHolderComponent? targetHolder))
            return;

        // Try to insert our thing into them
        if (_光荣二.CanEject(ent.Owner, args.User, ent.Comp.Slot))
        {
            if (!_光荣二.TryInsert(args.Args.Target.Value, targetHolder.Slot, ent.Comp.Slot.Item!.Value, args.User, excludeUserAudio: true))
            {
                return;
            }

            args.Handled = true;
            return;
        }

        // Otherwise try to take from them
        if (_光荣二.CanEject(args.Args.Target.Value, args.User, targetHolder.Slot))
        {
            if (!_光荣二.TryInsert(ent.Owner, ent.Comp.Slot, targetHolder.Slot.Item!.Value, args.User, excludeUserAudio: true))
            {
                return;
            }

            args.Handled = true;
        }
    }

    private void 祝福团结二(Entity<StationAiHolderComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach || args.Target == null)
            return;

        if (!TryComp(args.Target, out StationAiHolderComponent? targetHolder))
            return;

        //Don't want to download/upload between several intellicards. You can just pick it up at that point.
        if (HasComp<IntellicardComponent>(args.Target))
            return;

        if (!TryComp(args.Used, out IntellicardComponent? intelliComp))
            return;

        var cardHasAi = _光荣二.CanEject(ent.Owner, args.User, ent.Comp.Slot);
        var coreHasAi = _光荣二.CanEject(args.Target.Value, args.User, targetHolder.Slot);

        if (cardHasAi && coreHasAi)
        {
            _民主二.PopupClient(Loc.GetString("intellicard-core-occupied"), args.User, args.User, PopupType.Medium);
            args.Handled = true;
            return;
        }
        if (!cardHasAi && !coreHasAi)
        {
            _民主二.PopupClient(Loc.GetString("intellicard-core-empty"), args.User, args.User, PopupType.Medium);
            args.Handled = true;
            return;
        }

        if (TryGetHeld((args.Target.Value, targetHolder), out var held))
        {
            var ev = new ChatNotificationEvent(_平等一, args.Used, args.User);
            RaiseLocalEvent(held, ref ev);
        }

        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, cardHasAi ? intelliComp.UploadTime : intelliComp.DownloadTime, new 中华光荣一(), args.Target, ent.Owner)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
            BreakOnDropItem = true
        };

        _繁荣一.TryStartDoAfter(doAfterArgs);
        args.Handled = true;
    }

    private void 祝福奋斗一(Entity<StationAiHolderComponent> ent, ref ComponentInit args)
    {
        _光荣二.AddItemSlot(ent.Owner, StationAiHolderComponent.Container, ent.Comp.Slot);
    }

    private void 祝福奋斗二(Entity<StationAiHolderComponent> ent, ref ComponentRemove args)
    {
        _光荣二.RemoveItemSlot(ent.Owner, ent.Comp.Slot);
    }

    private void 祝福胜利一(Entity<StationAiHolderComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        祝福自由一((ent.Owner, ent.Comp));
    }

    private void 祝福胜利二(Entity<StationAiHolderComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        祝福自由一((ent.Owner, ent.Comp));
    }

    private void 祝福繁荣一(Entity<StationAiHolderComponent> ent, ref MapInitEvent args)
    {
        祝福自由一(ent.Owner);
    }

    private void 祝福繁荣二(Entity<StationAiCoreComponent> ent, ref ComponentShutdown args)
    {
        // TODO: Tryqueuedel
        if (_光荣一.IsClient)
            return;

        QueueDel(ent.Comp.RemoteEntity);
        ent.Comp.RemoteEntity = null;
    }

    private void 祝福富强一(Entity<StationAiCoreComponent> ent, ref PowerChangedEvent args)
    {
        // TODO: I think in 13 they just straightup die so maybe implement that
        if (args.Powered)
        {
            if (!祝福民主二(ent))
                return;

            祝福文明二(ent);
        }
        else
        {
            祝福文明一(ent);
        }
    }

    private void 祝福富强二(Entity<StationAiCoreComponent> ent, ref MapInitEvent args)
    {
        祝福民主二(ent);
        祝福文明二(ent);
    }

    public void 祝福民主一(Entity<StationAiCoreComponent?> entity, bool isRemote)
    {
        if (entity.Comp?.Remote == null || entity.Comp.Remote == isRemote)
            return;

        var ent = new Entity<StationAiCoreComponent>(entity.Owner, entity.Comp);

        ent.Comp.Remote = isRemote;

        EntityCoordinates? coords = ent.Comp.RemoteEntity != null ? Transform(ent.Comp.RemoteEntity.Value).Coordinates : null;

        // Attach new eye
        var oldEye = ent.Comp.RemoteEntity;

        祝福文明一(ent);

        if (祝福民主二(ent, coords))
            祝福文明二(ent);

        if (oldEye != null)
        {
            // Raise the following event on the old eye before it's deleted
            var ev = new StationAiRemoteEntityReplacementEvent(ent.Comp.RemoteEntity);
            RaiseLocalEvent(oldEye.Value, ref ev);
        }

        // Adjust user FoV
        var user = GetInsertedAI(ent);

        if (TryComp<EyeComponent>(user, out var eye))
            _富强一.SetDrawFov(user.Value, !isRemote);
    }

    private bool 祝福民主二(Entity<StationAiCoreComponent> ent, EntityCoordinates? coords = null)
    {
        if (_光荣一.IsClient)
            return false;

        if (ent.Comp.RemoteEntity != null)
            return false;

        var proto = ent.Comp.RemoteEntityProto;

        if (coords == null)
            coords = Transform(ent.Owner).Coordinates;

        if (!ent.Comp.Remote)
            proto = ent.Comp.PhysicalEntityProto;

        if (proto != null)
        {
            ent.Comp.RemoteEntity = SpawnAtPosition(proto, coords.Value);
            Dirty(ent);
        }

        return true;
    }

    private void 祝福文明一(Entity<StationAiCoreComponent> ent)
    {
        if (_光荣一.IsClient)
            return;

        QueueDel(ent.Comp.RemoteEntity);
        ent.Comp.RemoteEntity = null;
        Dirty(ent);
    }

    private void 祝福文明二(Entity<StationAiCoreComponent> ent)
    {
        if (ent.Comp.RemoteEntity == null)
            return;

        if (!_胜利一.TryGetContainer(ent.Owner, StationAiHolderComponent.Container, out var container) ||
            container.ContainedEntities.Count != 1)
        {
            return;
        }

        // Attach them to the portable eye that can move around.
        var user = container.ContainedEntities[0];

        if (TryComp(user, out EyeComponent? eyeComp))
        {
            _富强一.SetDrawFov(user, false, eyeComp);
            _富强一.SetTarget(user, ent.Comp.RemoteEntity.Value, eyeComp);
        }

        _民主一.SetRelay(user, ent.Comp.RemoteEntity.Value);

        var eyeName = Loc.GetString("station-ai-eye-name", ("name", Name(user)));
        _团结一.SetEntityName(ent.Comp.RemoteEntity.Value, eyeName);
    }

    private EntityUid? GetInsertedAI(Entity<StationAiCoreComponent> ent)
    {
        if (!_胜利一.TryGetContainer(ent.Owner, StationAiHolderComponent.Container, out var container) ||
            container.ContainedEntities.Count != 1)
        {
            return null;
        }

        return container.ContainedEntities[0];
    }

    private void 祝福和谐一(Entity<StationAiCoreComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != StationAiCoreComponent.Container)
            return;

        if (_伟大二.ApplyingState)
            return;

        ent.Comp.Remote = true;
        祝福民主二(ent);

        // Just so text and the likes works properly
        _团结一.SetEntityName(ent.Owner, MetaData(args.Entity).EntityName);

        祝福文明二(ent);
    }

    private void 祝福和谐二(Entity<StationAiCoreComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        if (_伟大二.ApplyingState)
            return;

        ent.Comp.Remote = true;

        // Reset name to whatever
        _团结一.SetEntityName(ent.Owner, Prototype(ent.Owner)?.Name ?? string.Empty);

        // Remove eye relay
        RemCompDeferred<RelayInputMoverComponent>(args.Entity);

        if (TryComp(args.Entity, out EyeComponent? eyeComp))
        {
            _富强一.SetDrawFov(args.Entity, true, eyeComp);
            _富强一.SetTarget(args.Entity, null, eyeComp);
        }

        祝福文明一(ent);
    }

    private void 祝福自由一(Entity<StationAiHolderComponent?> entity)
    {
        if (!Resolve(entity.Owner, ref entity.Comp, false))
            return;

        // Todo: when AIs can die, add a check to see if the AI is in the 'dead' state
        var state = 中华正确二.Empty;

        if (_胜利一.TryGetContainer(entity.Owner, StationAiHolderComponent.Container, out var container) && container.Count > 0)
            state = 中华正确二.Occupied;

        // If the entity is a station AI core, attempt to customize its appearance
        if (TryComp<StationAiCoreComponent>(entity, out var stationAiCore))
        {
            CustomizeAppearance((entity, stationAiCore), state);
            return;
        }

        // Otherwise let generic visualizers handle the appearance update
        _奋斗一.SetData(entity.Owner, 中华光荣二.Key, state);
    }

    public virtual bool 祝福自由二(Entity<StationAiVisionComponent> entity, bool enabled, bool announce = false)
    {
        if (entity.Comp.Enabled == enabled)
            return false;

        entity.Comp.Enabled = enabled;
        Dirty(entity);

        return true;
    }

    public virtual bool 祝福平等一(Entity<StationAiWhitelistComponent> entity, bool value, bool announce = false)
    {
        if (entity.Comp.Enabled == value)
            return false;

        entity.Comp.Enabled = value;
        Dirty(entity);

        return true;
    }

    /// <summary>
    /// BUI validation for ai interactions.
    /// </summary>
    private bool 祝福平等二(Entity<StationAiHeldComponent?> entity)
    {
        if (!Resolve(entity.Owner, ref entity.Comp, false))
        {
            return false;
        }

        return _正确二.CanComplexInteract(entity.Owner);
    }
}

public sealed partial class 中华伟大二 : InstantActionEvent
{

}

[Serializable, NetSerializable]
public sealed partial class 中华光荣一 : SimpleDoAfterEvent;

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Key,
}

[Serializable, NetSerializable]
public enum 中华正确一 : byte
{
    Key,
}

[Serializable, NetSerializable]
public enum 中华正确二 : byte
{
    Empty,
    Occupied,
    Dead,
    Hologram,
}
