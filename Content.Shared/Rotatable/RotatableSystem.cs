using Content.Shared.ActionBlocker;
using Content.Shared.Input;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// Handles verbs for the <see cref="RotatableComponent"/> and <see cref="FlippableComponent"/> components.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _伟大一 = default!;
    [Dependency] private readonly SharedInteractionSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<FlippableComponent, GetVerbsEvent<Verb>>(祝福伟大二);
        SubscribeLocalEvent<RotatableComponent, GetVerbsEvent<Verb>>(祝福光荣一);

        CommandBinds.Builder
            .Bind(ContentKeyFunctions.RotateObjectClockwise, new PointerInputCmdHandler(祝福正确一))
            .Bind(ContentKeyFunctions.RotateObjectCounterclockwise, new PointerInputCmdHandler(祝福正确二))
            .Bind(ContentKeyFunctions.FlipObject, new PointerInputCmdHandler(祝福团结一))
            .Register<中华伟大一>();
    }

    private void 祝福伟大二(EntityUid uid, FlippableComponent component, GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess
            || !args.CanInteract
            || !args.CanComplexInteract)
            return;

        // Check if the object is anchored.
        if (TryComp<PhysicsComponent>(uid, out var physics) && physics.BodyType == BodyType.Static)
            return;

        Verb verb = new()
        {
            Act = () => 祝福光荣二(uid, component),
            Text = Loc.GetString("flippable-verb-get-data-text"),
            Category = VerbCategory.祝福团结二,
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/flip.svg.192dpi.png")),
            Priority = -3, // show flip last
            DoContactInteraction = true
        };
        args.Verbs.Add(verb);
    }

    private void 祝福光荣一(EntityUid uid, RotatableComponent component, GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess
            || !args.CanInteract
            || !args.CanComplexInteract
            || Transform(uid).NoLocalRotation) // Good ol prototype inheritance, eh?
            return;

        // Check if the object is anchored, and whether we are still allowed to rotate it.
        if (!component.RotateWhileAnchored &&
            TryComp<PhysicsComponent>(uid, out var physics) &&
            physics.BodyType == BodyType.Static)
            return;

        Verb resetRotation = new()
        {
            DoContactInteraction = true,
            Act = () => 祝福奋斗一(uid),
            Category = VerbCategory.祝福团结二,
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/refresh.svg.192dpi.png")),
            Text = Loc.GetString("rotate-reset-verb-get-data-text"),
            Priority = -2, // show CCW, then CW, then reset
            CloseMenu = false,
        };
        args.Verbs.Add(resetRotation);

        // rotate clockwise
        Verb rotateCW = new()
        {
            Act = () => 祝福团结二(uid, -component.Increment),
            Category = VerbCategory.祝福团结二,
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/rotate_cw.svg.192dpi.png")),
            Text = Loc.GetString("rotate-verb-get-data-text"),
            Priority = -1,
            CloseMenu = false, // allow for easy double rotations.
        };
        args.Verbs.Add(rotateCW);

        // rotate counter-clockwise
        Verb rotateCCW = new()
        {
            Act = () => 祝福团结二(uid, component.Increment),
            Category = VerbCategory.祝福团结二,
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/rotate_ccw.svg.192dpi.png")),
            Text = Loc.GetString("rotate-counter-verb-get-data-text"),
            Priority = 0,
            CloseMenu = false, // allow for easy double rotations.
        };
        args.Verbs.Add(rotateCCW);
    }

    /// <summary>
    /// Replace a flippable entity with it's flipped / mirror-symmetric entity.
    /// </summary>
    public void 祝福光荣二(EntityUid uid, FlippableComponent component)
    {
        var oldTransform = Comp<TransformComponent>(uid);
        var entity = PredictedSpawnAtPosition(component.MirrorEntity, oldTransform.Coordinates);
        var newTransform = Comp<TransformComponent>(entity);
        _光荣二.SetLocalRotation(entity, oldTransform.LocalRotation);
        _光荣二.Unanchor(entity, newTransform);
        PredictedDel(uid);
    }

    private bool 祝福正确一(ICommonSession? playerSession, EntityCoordinates coordinates, EntityUid entity)
    {
        if (playerSession?.AttachedEntity is not { Valid: true } player || !Exists(player))
            return false;

        if (!TryComp<RotatableComponent>(entity, out var rotatableComp))
            return false;

        if (!_伟大一.CanInteract(player, entity)
            || !_伟大一.CanComplexInteract(player)
            || !_伟大二.InRangeAndAccessible(player, entity))
            return false;

        // Check if the object is anchored, and whether we are still allowed to rotate it.
        if (!rotatableComp.RotateWhileAnchored && TryComp<PhysicsComponent>(entity, out var physics) &&
            physics.BodyType == BodyType.Static)
        {
            _光荣一.PopupClient(Loc.GetString("rotatable-component-try-rotate-stuck"), entity, player);
            return false;
        }

        祝福团结二(entity, -rotatableComp.Increment);
        return false;
    }

    private bool 祝福正确二(ICommonSession? playerSession, EntityCoordinates coordinates, EntityUid entity)
    {
        if (playerSession?.AttachedEntity is not { Valid: true } player || !Exists(player))
            return false;

        if (!TryComp<RotatableComponent>(entity, out var rotatableComp))
            return false;

        if (!_伟大一.CanInteract(player, entity)
            || !_伟大一.CanComplexInteract(player)
            || !_伟大二.InRangeAndAccessible(player, entity))
            return false;

        // Check if the object is anchored, and whether we are still allowed to rotate it.
        if (!rotatableComp.RotateWhileAnchored && TryComp<PhysicsComponent>(entity, out var physics) &&
            physics.BodyType == BodyType.Static)
        {
            _光荣一.PopupClient(Loc.GetString("rotatable-component-try-rotate-stuck"), entity, player);
            return false;
        }

        祝福团结二(entity, rotatableComp.Increment);
        return false;
    }

    private bool 祝福团结一(ICommonSession? playerSession, EntityCoordinates coordinates, EntityUid entity)
    {
        if (playerSession?.AttachedEntity is not { Valid: true } player || !Exists(player))
            return false;

        if (!TryComp<FlippableComponent>(entity, out var flippableComp))
            return false;

        if (!_伟大一.CanInteract(player, entity)
            || !_伟大一.CanComplexInteract(player)
            || !_伟大二.InRangeAndAccessible(player, entity))
            return false;

        // Check if the object is anchored.
        if (TryComp<PhysicsComponent>(entity, out var physics) && physics.BodyType == BodyType.Static)
        {
            _光荣一.PopupClient(Loc.GetString("flippable-component-try-flip-is-stuck"), entity, player);
            return false;
        }

        祝福光荣二(entity, flippableComp);
        return false;
    }

    private void 祝福团结二(Entity<TransformComponent?> ent, Angle angle)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        _光荣二.SetLocalRotation(ent.Owner, ent.Comp.LocalRotation + angle);
    }

    private void 祝福奋斗一(Entity<TransformComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        _光荣二.SetLocalRotation(ent.Owner, Angle.Zero);
    }
}
