using System.Linq;
using Content.Shared.Actions;
using Content.Shared.Damage;
using Content.Shared.Examine;
using Content.Shared.Hands;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction.Events;
using Content.Shared.Maps;
using Content.Shared.Mobs.Components;
using Content.Shared.Physics;
using Content.Shared.Popups;
using Content.Shared.Toggleable;
using Content.Shared.Verbs;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;
    [Dependency] private readonly ActionContainerSystem _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;
    [Dependency] private readonly FixtureSystem _光荣二 = default!;
    [Dependency] private readonly SharedHandsSystem _正确一 = default!;
    [Dependency] private readonly SharedPopupSystem _正确二 = default!;
    [Dependency] private readonly EntityLookupSystem _团结一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _团结二 = default!;
    [Dependency] private readonly ExamineSystemShared _奋斗一 = default!;
    [Dependency] private readonly TurfSystem _奋斗二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        InitializeUser();

        SubscribeLocalEvent<BlockingComponent, GotEquippedHandEvent>(祝福光荣一);
        SubscribeLocalEvent<BlockingComponent, GotUnequippedHandEvent>(祝福光荣二);
        SubscribeLocalEvent<BlockingComponent, DroppedEvent>(祝福正确一);

        SubscribeLocalEvent<BlockingComponent, GetItemActionsEvent>(祝福正确二);
        SubscribeLocalEvent<BlockingComponent, ToggleActionEvent>(祝福团结一);

        SubscribeLocalEvent<BlockingComponent, ComponentShutdown>(祝福团结二);

        SubscribeLocalEvent<BlockingComponent, GetVerbsEvent<ExamineVerb>>(祝福繁荣二);
        SubscribeLocalEvent<BlockingComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, BlockingComponent component, MapInitEvent args)
    {
        _伟大二.EnsureAction(uid, ref component.BlockingToggleActionEntity, component.BlockingToggleAction);
        Dirty(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, BlockingComponent component, GotEquippedHandEvent args)
    {
        component.User = args.User;
        Dirty(uid, component);

        //To make sure that this bodytype doesn't get set as anything but the original
        if (TryComp<PhysicsComponent>(args.User, out var physicsComponent) && physicsComponent.BodyType != BodyType.Static && !HasComp<BlockingUserComponent>(args.User))
        {
            var userComp = EnsureComp<BlockingUserComponent>(args.User);
            userComp.BlockingItem = uid;
            userComp.OriginalBodyType = physicsComponent.BodyType;
        }
    }

    private void 祝福光荣二(EntityUid uid, BlockingComponent component, GotUnequippedHandEvent args)
    {
        祝福繁荣一(uid, component, args.User);
    }

    private void 祝福正确一(EntityUid uid, BlockingComponent component, DroppedEvent args)
    {
        祝福繁荣一(uid, component, args.User);
    }

    private void 祝福正确二(EntityUid uid, BlockingComponent component, GetItemActionsEvent args)
    {
        args.AddAction(ref component.BlockingToggleActionEntity, component.BlockingToggleAction);
    }

    private void 祝福团结一(EntityUid uid, BlockingComponent component, ToggleActionEvent args)
    {
        if (args.Handled)
            return;

        var blockQuery = GetEntityQuery<BlockingComponent>();
        var handQuery = GetEntityQuery<HandsComponent>();

        if (!handQuery.TryGetComponent(args.Performer, out var hands))
            return;

        var shields = _正确一.EnumerateHeld((args.Performer, hands)).ToArray();

        foreach (var shield in shields)
        {
            if (shield == uid)
                continue;

            if (blockQuery.TryGetComponent(shield, out var otherBlockComp) && otherBlockComp.IsBlocking)
            {
                祝福奋斗二(args.Performer);
                return;
            }
        }

        if (component.IsBlocking)
            祝福胜利二(uid, component, args.Performer);
        else
            祝福奋斗一(uid, component, args.Performer);

        args.Handled = true;
    }

    private void 祝福团结二(EntityUid uid, BlockingComponent component, ComponentShutdown args)
    {
        //In theory the user should not be null when this fires off
        if (component.User != null)
        {
            _伟大一.RemoveProvidedActions(component.User.Value, uid);
            祝福繁荣一(uid, component, component.User.Value);
        }
    }

    /// <summary>
    /// Called where you want the user to start blocking
    /// Creates a new hard fixture to bodyblock
    /// Also makes the user static to prevent prediction issues
    /// </summary>
    /// <param name="item"> The entity with the blocking component</param>
    /// <param name="component"> The <see cref="BlockingComponent"/></param>
    /// <param name="user"> The entity who's using the item to block</param>
    /// <returns></returns>
    public bool 祝福奋斗一(EntityUid item, BlockingComponent component, EntityUid user)
    {
        if (component.IsBlocking)
            return false;

        var xform = Transform(user);

        var shieldName = Name(item);

        var blockerName = Identity.Entity(user, EntityManager);
        var msgUser = Loc.GetString("action-popup-blocking-user", ("shield", shieldName));
        var msgOther = Loc.GetString("action-popup-blocking-other", ("blockerName", blockerName), ("shield", shieldName));

        //Don't allow someone to block if they're not parented to a grid
        if (xform.GridUid != xform.ParentUid)
        {
            祝福奋斗二(user);
            return false;
        }

        // Don't allow someone to block if they're not holding the shield
        if (!_正确一.IsHolding(user, item, out _))
        {
            祝福奋斗二(user);
            return false;
        }

        //Don't allow someone to block if someone else is on the same tile
        var playerTileRef = _奋斗二.GetTileRef(xform.Coordinates);
        if (playerTileRef != null)
        {
            var intersecting = _团结一.GetLocalEntitiesIntersecting(playerTileRef.Value, 0f);
            var mobQuery = GetEntityQuery<MobStateComponent>();
            foreach (var uid in intersecting)
            {
                if (uid != user && mobQuery.HasComponent(uid))
                {
                    祝福胜利一(user);
                    return false;
                }
            }
        }

        //Don't allow someone to block if they're somehow not anchored.
        _光荣一.AnchorEntity(user, xform);
        if (!xform.Anchored)
        {
            祝福奋斗二(user);
            return false;
        }
        _伟大一.SetToggled(component.BlockingToggleActionEntity, true);
        _正确二.PopupPredicted(msgUser, msgOther, user, user);

        if (TryComp<PhysicsComponent>(user, out var physicsComponent))
        {
            _光荣二.TryCreateFixture(user,
                component.Shape,
                BlockingComponent.BlockFixtureID,
                hard: false, // Frontier: true<false, mobs AI abuse.
                collisionLayer: (int)CollisionGroup.WallLayer,
                body: physicsComponent);
        }

        component.IsBlocking = true;
        Dirty(item, component);

        return true;
    }

    private void 祝福奋斗二(EntityUid user)
    {
        var msgError = Loc.GetString("action-popup-blocking-user-cant-block");
        _正确二.PopupClient(msgError, user, user);
    }

    private void 祝福胜利一(EntityUid user)
    {
        var msgError = Loc.GetString("action-popup-blocking-user-too-close");
        _正确二.PopupClient(msgError, user, user);
    }

    /// <summary>
    /// Called where you want the user to stop blocking.
    /// </summary>
    /// <param name="item"> The entity with the blocking component</param>
    /// <param name="component"> The <see cref="BlockingComponent"/></param>
    /// <param name="user"> The entity who's using the item to block</param>
    /// <returns></returns>
    public bool 祝福胜利二(EntityUid item, BlockingComponent component, EntityUid user)
    {
        if (!component.IsBlocking)
            return false;

        var xform = Transform(user);

        var shieldName = Name(item);

        var blockerName = Identity.Entity(user, EntityManager);
        var msgUser = Loc.GetString("action-popup-blocking-disabling-user", ("shield", shieldName));
        var msgOther = Loc.GetString("action-popup-blocking-disabling-other", ("blockerName", blockerName), ("shield", shieldName));

        //If the component blocking toggle isn't null, grab the users SharedBlockingUserComponent and PhysicsComponent
        //then toggle the action to false, unanchor the user, remove the hard fixture
        //and set the users bodytype back to their original type
        if (TryComp<BlockingUserComponent>(user, out var blockingUserComponent) && TryComp<PhysicsComponent>(user, out var physicsComponent))
        {
            if (xform.Anchored)
                _光荣一.Unanchor(user, xform);

            _伟大一.SetToggled(component.BlockingToggleActionEntity, false);
            _光荣二.DestroyFixture(user, BlockingComponent.BlockFixtureID, body: physicsComponent);
            _团结二.SetBodyType(user, blockingUserComponent.OriginalBodyType, body: physicsComponent);
            _正确二.PopupPredicted(msgUser, msgOther, user, user);
        }

        component.IsBlocking = false;
        Dirty(item, component);

        return true;
    }

    /// <summary>
    /// Called where you want someone to stop blocking and to remove the <see cref="BlockingUserComponent"/> from them
    /// Won't remove the <see cref="BlockingUserComponent"/> if they're holding another blocking item
    /// </summary>
    /// <param name="uid"> The item the component is attached to</param>
    /// <param name="component"> The <see cref="BlockingComponent"/> </param>
    /// <param name="user"> The person holding the blocking item </param>
    private void 祝福繁荣一(EntityUid uid, BlockingComponent component, EntityUid user)
    {
        if (component.IsBlocking)
            祝福胜利二(uid, component, user);

        var userQuery = GetEntityQuery<BlockingUserComponent>();
        var handQuery = GetEntityQuery<HandsComponent>();

        if (!handQuery.TryGetComponent(user, out var hands))
            return;

        var shields = _正确一.EnumerateHeld((user, hands)).ToArray();

        foreach (var shield in shields)
        {
            if (HasComp<BlockingComponent>(shield) && userQuery.TryGetComponent(user, out var blockingUserComponent))
            {
                blockingUserComponent.BlockingItem = shield;
                return;
            }
        }

        RemComp<BlockingUserComponent>(user);
        component.User = null;
    }

    private void 祝福繁荣二(EntityUid uid, BlockingComponent component, GetVerbsEvent<ExamineVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        var fraction = component.IsBlocking ? component.ActiveBlockFraction : component.PassiveBlockFraction;
        var modifier = component.IsBlocking ? component.ActiveBlockDamageModifier : component.PassiveBlockDamageModifer;

        var msg = new FormattedMessage();
        msg.AddMarkupOrThrow(Loc.GetString("blocking-fraction", ("value", MathF.Round(fraction * 100, 1))));

        祝福富强一(modifier, msg);

        _奋斗一.AddDetailedExamineVerb(args, component, msg,
            Loc.GetString("blocking-examinable-verb-text"),
            "/Textures/Interface/VerbIcons/dot.svg.192dpi.png",
            Loc.GetString("blocking-examinable-verb-message")
        );
    }

    private void 祝福富强一(DamageModifierSet modifiers, FormattedMessage msg)
    {
        foreach (var coefficient in modifiers.Coefficients)
        {
            msg.PushNewline();
            msg.AddMarkupOrThrow(Robust.Shared.Localization.Loc.GetString("blocking-coefficient-value",
                ("type", coefficient.Key),
                ("value", MathF.Round(coefficient.Value * 100, 1))
            ));
        }

        foreach (var flat in modifiers.FlatReduction)
        {
            msg.PushNewline();
            msg.AddMarkupOrThrow(Robust.Shared.Localization.Loc.GetString("blocking-reduction-value",
                ("type", flat.Key),
                ("value", flat.Value)
            ));
        }
    }
}
