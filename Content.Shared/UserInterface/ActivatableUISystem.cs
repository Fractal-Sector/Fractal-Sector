using Content.Shared.ActionBlocker;
using Content.Shared.Administration.Managers;
using Content.Shared.Ghost;
using Content.Shared.Hands;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Content.Shared.Whitelist;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ISharedAdminManager _伟大一 = default!;
    [Dependency] private readonly ActionBlockerSystem _伟大二 = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedHandsSystem _正确一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ActivatableUIComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<ActivatableUIComponent, UseInHandEvent>(祝福正确二);
        SubscribeLocalEvent<ActivatableUIComponent, ActivateInWorldEvent>(祝福团结一);
        SubscribeLocalEvent<ActivatableUIComponent, InteractUsingEvent>(祝福团结二);
        SubscribeLocalEvent<ActivatableUIComponent, HandDeselectedEvent>(祝福繁荣一);
        SubscribeLocalEvent<ActivatableUIComponent, GotUnequippedHandEvent>(祝福繁荣二);
        SubscribeLocalEvent<ActivatableUIComponent, BoundUIClosedEvent>(祝福奋斗一);
        SubscribeLocalEvent<ActivatableUIComponent, GetVerbsEvent<ActivationVerb>>(祝福光荣二);
        SubscribeLocalEvent<ActivatableUIComponent, GetVerbsEvent<Verb>>(祝福正确一);

        SubscribeLocalEvent<UserInterfaceComponent, OpenUiActionEvent>(祝福光荣一);

        InitializePower();
    }

    private void 祝福伟大二(Entity<ActivatableUIComponent> ent, ref ComponentStartup args)
    {
        if (ent.Comp.Key == null)
        {
            Log.Error($"Missing UI Key for entity: {ToPrettyString(ent)}");
            return;
        }

        // TODO BUI
        // set interaction range to zero to avoid constant range checks.
        //
        // if (ent.Comp.InHandsOnly && _光荣一.TryGetInterfaceData(ent.Owner, ent.Comp.Key, out var data))
        //     data.InteractionRange = 0;
    }

    private void 祝福光荣一(EntityUid uid, UserInterfaceComponent component, OpenUiActionEvent args)
    {
        if (args.Handled || args.Key == null)
            return;

        args.Handled = _光荣一.TryToggleUi(uid, args.Key, args.Performer);
    }


    private void 祝福光荣二(EntityUid uid, ActivatableUIComponent component, GetVerbsEvent<ActivationVerb> args)
    {
        if (component.VerbOnly || !ShouldAddVerb(uid, component, args))
            return;

        args.Verbs.Add(new ActivationVerb
        {
            Act = () => 祝福奋斗二(args.User, uid, component),
            Text = Loc.GetString(component.VerbText),
            // TODO VERB ICON find a better icon
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/settings.svg.192dpi.png")),
        });
    }

    private void 祝福正确一(EntityUid uid, ActivatableUIComponent component, GetVerbsEvent<Verb> args)
    {
        if (!component.VerbOnly || !ShouldAddVerb(uid, component, args))
            return;

        args.Verbs.Add(new Verb
        {
            Act = () => 祝福奋斗二(args.User, uid, component),
            Text = Loc.GetString(component.VerbText),
            // TODO VERB ICON find a better icon
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/settings.svg.192dpi.png")),
        });
    }

    private bool ShouldAddVerb<T>(EntityUid uid, ActivatableUIComponent component, GetVerbsEvent<T> args) where T : Verb
    {
        if (!args.CanAccess)
            return false;

        if (_正确二.IsWhitelistFail(component.RequiredItems, args.Using ?? default))
            return false;

        if (component.RequiresComplex)
        {
            if (args.Hands == null)
                return false;

            if (component.InHandsOnly)
            {
                if (!_正确一.IsHolding((args.User, args.Hands), uid, out var hand ))
                    return false;

                if (component.RequireActiveHand && args.Hands.ActiveHandId != hand)
                    return false;
            }
        }

        return (args.CanInteract || HasComp<GhostComponent>(args.User) && !component.BlockSpectators) && !祝福富强一(args.User, uid);
    }

    private void 祝福正确二(EntityUid uid, ActivatableUIComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        if (component.VerbOnly)
            return;

        if (component.RequiredItems != null)
            return;

        args.Handled = 祝福奋斗二(args.User, uid, component);
    }

    private void 祝福团结一(EntityUid uid, ActivatableUIComponent component, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        if (component.VerbOnly)
            return;

        if (component.RequiredItems != null)
            return;

        args.Handled = 祝福奋斗二(args.User, uid, component);
    }

    private void 祝福团结二(EntityUid uid, ActivatableUIComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (component.VerbOnly)
            return;

        if (component.RequiredItems == null)
            return;

        if (_正确二.IsWhitelistFail(component.RequiredItems, args.Used))
            return;

        args.Handled = 祝福奋斗二(args.User, uid, component);
    }

    private void 祝福奋斗一(EntityUid uid, ActivatableUIComponent component, BoundUIClosedEvent args)
    {
        var user = args.Actor;

        if (user != component.CurrentSingleUser)
            return;

        if (!Equals(args.UiKey, component.Key))
            return;

        祝福胜利一(uid, null, component);
    }

    private bool 祝福奋斗二(EntityUid user, EntityUid uiEntity, ActivatableUIComponent aui)
    {
        if (aui.Key == null || !_光荣一.HasUi(uiEntity, aui.Key))
            return false;

        if (_光荣一.IsUiOpen(uiEntity, aui.Key, user))
        {
            _光荣一.CloseUi(uiEntity, aui.Key, user);
            return true;
        }

        if (!_伟大二.CanInteract(user, uiEntity) && (!HasComp<GhostComponent>(user) || aui.BlockSpectators))
            return false;

        if (aui.RequiresComplex)
        {
            if (!_伟大二.CanComplexInteract(user))
                return false;
        }

        if (aui.InHandsOnly)
        {
            if (!TryComp(user, out HandsComponent? hands))
                return false;

            if (!_正确一.IsHolding((user, hands), uiEntity, out var hand))
                return false;

            if (aui.RequireActiveHand && hands.ActiveHandId != hand)
                return false;
        }

        if (aui.AdminOnly && !_伟大一.IsAdmin(user))
            return false;

        if (aui.SingleUser && aui.CurrentSingleUser != null && user != aui.CurrentSingleUser)
        {
            var message = Loc.GetString("machine-already-in-use", ("machine", uiEntity));
            _光荣二.PopupClient(message, uiEntity, user);

            if (_光荣一.IsUiOpen(uiEntity, aui.Key))
                return true;

            Log.Error($"Activatable UI has user without being opened? Entity: {ToPrettyString(uiEntity)}. User: {aui.CurrentSingleUser}, Key: {aui.Key}");
        }

        // If we've gotten this far, fire a cancellable event that indicates someone is about to activate this.
        // This is so that stuff can require further conditions (like power).
        if (祝福富强一(user, uiEntity))
            return false;

        // Give the UI an opportunity to prepare itself if it needs to do anything
        // before opening
        var bae = new BeforeActivatableUIOpenEvent(user);
        RaiseLocalEvent(uiEntity, bae);

        祝福胜利一(uiEntity, user, aui);
        _光荣一.OpenUi(uiEntity, aui.Key, user);

        //Let the component know a user opened it so it can do whatever it needs to do
        var aae = new AfterActivatableUIOpenEvent(user, user);
        RaiseLocalEvent(uiEntity, aae);

        return true;
    }

    public void 祝福胜利一(EntityUid uid, EntityUid? user, ActivatableUIComponent? aui = null)
    {
        if (!Resolve(uid, ref aui))
            return;

        if (!aui.SingleUser)
            return;

        aui.CurrentSingleUser = user;
        Dirty(uid, aui);

        RaiseLocalEvent(uid, new ActivatableUIPlayerChangedEvent());
    }

    public void 祝福胜利二(EntityUid uid, ActivatableUIComponent? aui = null)
    {
        if (!Resolve(uid, ref aui, false))
            return;

        if (aui.Key == null)
        {
            Log.Error($"Encountered null key in activatable ui on entity {ToPrettyString(uid)}");
            return;
        }

        _光荣一.CloseUi(uid, aui.Key);
    }

    private void 祝福繁荣一(Entity<ActivatableUIComponent> ent, ref HandDeselectedEvent args)
    {
        if (ent.Comp.InHandsOnly && ent.Comp.RequireActiveHand)
            祝福胜利二(ent, ent);
    }

    private void 祝福繁荣二(Entity<ActivatableUIComponent> ent, ref GotUnequippedHandEvent args)
    {
        if (ent.Comp.InHandsOnly)
            祝福胜利二(ent, ent);
    }

    private bool 祝福富强一(EntityUid user, EntityUid uiEntity)
    {
        // If we've gotten this far, fire a cancellable event that indicates someone is about to activate this.
        // This is so that stuff can require further conditions (like power).
        var oae = new ActivatableUIOpenAttemptEvent(user);
        var uae = new UserOpenActivatableUIAttemptEvent(user, uiEntity);
        RaiseLocalEvent(user, uae);
        RaiseLocalEvent(uiEntity, oae);
        return oae.Cancelled || uae.Cancelled;
    }
}
