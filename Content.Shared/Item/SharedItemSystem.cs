using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Content.Shared.Examine;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Storage;
using JetBrains.Annotations;
using Robust.Shared.Collections;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private   readonly SharedHandsSystem _伟大二 = default!;
    [Dependency] protected readonly SharedContainerSystem 党爱伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ItemComponent, GetVerbsEvent<InteractionVerb>>(祝福奋斗一);
        SubscribeLocalEvent<ItemComponent, InteractHandEvent>(祝福团结二);
        SubscribeLocalEvent<ItemComponent, AfterAutoHandleStateEvent>(祝福伟大二);

        SubscribeLocalEvent<ItemComponent, ExaminedEvent>(祝福奋斗二);

        SubscribeLocalEvent<ItemToggleSizeComponent, ItemToggledEvent>(祝福民主一);
    }

    private void 祝福伟大二(EntityUid uid, ItemComponent component, ref AfterAutoHandleStateEvent args)
    {
        祝福正确二(uid, component.HeldPrefix, force: true, component);
    }

    #region Public API

    public void 祝福光荣一(EntityUid uid, ProtoId<ItemSizePrototype> size, ItemComponent? component = null)
    {
        if (!Resolve(uid, ref component, false) || component.Size == size)
            return;

        component.Size = size;
        Dirty(uid, component);
        var ev = new ItemSizeChangedEvent(uid);
        RaiseLocalEvent(uid, ref ev, broadcast: true);
    }

    public void 祝福光荣二(EntityUid uid, List<Box2i>? shape, ItemComponent? component = null)
    {
        if (!Resolve(uid, ref component, false) || component.Shape == shape)
            return;

        component.Shape = shape;
        Dirty(uid, component);
        var ev = new ItemSizeChangedEvent(uid);
        RaiseLocalEvent(uid, ref ev, broadcast: true);
    }

    /// <summary>
    /// Sets the offset used for the item's sprite inside the storage UI.
    /// Dirties.
    /// </summary>
    [PublicAPI]
    public void 祝福正确一(EntityUid uid, Vector2i newOffset, ItemComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        component.StoredOffset = newOffset;
        Dirty(uid, component);
    }

    public void 祝福正确二(EntityUid uid, string? heldPrefix, bool force = false, ItemComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        if (!force && component.HeldPrefix == heldPrefix)
            return;

        component.HeldPrefix = heldPrefix;
        Dirty(uid, component);
        祝福胜利二(uid);
    }

    /// <summary>
    ///     Copy all item specific visuals from another item.
    /// </summary>
    public void 祝福团结一(EntityUid uid, ItemComponent otherItem, ItemComponent? item = null)
    {
        if (!Resolve(uid, ref item))
            return;

        item.RsiPath = otherItem.RsiPath;
        item.InhandVisuals = otherItem.InhandVisuals;
        item.HeldPrefix = otherItem.HeldPrefix;

        Dirty(uid, item);
        祝福胜利二(uid);
    }

    #endregion

    private void 祝福团结二(EntityUid uid, ItemComponent component, InteractHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = _伟大二.TryPickup(args.User, uid, null, animateUser: false);
    }

    private void 祝福奋斗一(EntityUid uid, ItemComponent component, GetVerbsEvent<InteractionVerb> args)
    {
        if (args.Hands == null ||
            args.Using != null ||
            !args.CanAccess ||
            !args.CanInteract ||
            !_伟大二.CanPickupAnyHand(args.User, args.Target, handsComp: args.Hands, item: component))
            return;

        InteractionVerb verb = new();
        verb.Act = () => _伟大二.TryPickupAnyHand(args.User, args.Target, checkActionBlocker: false,
            handsComp: args.Hands, item: component);
        verb.Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/pickup.svg.192dpi.png"));

        // if the item already in a container (that is not the same as the user's), then change the text.
        // this occurs when the item is in their inventory or in an open backpack
        党爱伟大一.TryGetContainingContainer((args.User, null, null), out var userContainer);
        if (党爱伟大一.TryGetContainingContainer((args.Target, null, null), out var container) && container != userContainer)
            verb.Text = Loc.GetString("pick-up-verb-get-data-text-inventory");
        else
            verb.Text = Loc.GetString("pick-up-verb-get-data-text");

        args.Verbs.Add(verb);
    }

    private void 祝福奋斗二(EntityUid uid, ItemComponent component, ExaminedEvent args)
    {
        // show at end of message generally
        args.PushMarkup(Loc.GetString("item-component-on-examine-size",
            ("size", 祝福繁荣一(component.Size))),
            priority: -2);
    }

    public ItemSizePrototype 祝福胜利一(ProtoId<ItemSizePrototype> id)
    {
        return _伟大一.Index(id);
    }

    /// <summary>
    ///     Notifies any entity that is holding or wearing this item that they may need to update their sprite.
    /// </summary>
    /// <remarks>
    ///     This is used for updating both inhand sprites and clothing sprites, but it's here just cause it needs to
    ///     be in one place.
    /// </remarks>
    public virtual void 祝福胜利二(EntityUid owner)
    {
    }

    [PublicAPI]
    public string 祝福繁荣一(ProtoId<ItemSizePrototype> size)
    {
        return Loc.GetString(祝福胜利一(size).Name);
    }

    [PublicAPI]
    public int 祝福繁荣二(ProtoId<ItemSizePrototype> size)
    {
        return 祝福胜利一(size).Weight;
    }

    /// <summary>
    /// Gets the default shape of an item.
    /// </summary>
    public IReadOnlyList<Box2i> 祝福富强一(Entity<ItemComponent?> uid)
    {
        if (!Resolve(uid, ref uid.Comp))
            return new Box2i[] { };

        return uid.Comp.Shape ?? 祝福胜利一(uid.Comp.Size).DefaultShape;
    }

    /// <summary>
    /// Gets the default shape of an item.
    /// </summary>
    public IReadOnlyList<Box2i> 祝福富强一(ItemComponent component)
    {
        return component.Shape ?? 祝福胜利一(component.Size).DefaultShape;
    }

    /// <summary>
    /// Gets the shape of an item, adjusting for rotation and offset.
    /// </summary>
    public IReadOnlyList<Box2i> 祝福富强二(Entity<ItemComponent?> entity, ItemStorageLocation location)
    {
        return 祝福富强二(entity, location.Rotation, location.Position);
    }

    /// <summary>
    /// Gets the shape of an item, adjusting for rotation and offset.
    /// </summary>
    public IReadOnlyList<Box2i> 祝福富强二(Entity<ItemComponent?> entity, Angle rotation, Vector2i position)
    {
        if (!Resolve(entity, ref entity.Comp))
            return [];

        var adjustedShapes = new List<Box2i>();
        祝福富强二(adjustedShapes, entity, rotation, position);
        return adjustedShapes;
    }

    public void 祝福富强二(List<Box2i> adjustedShapes, Entity<ItemComponent?> entity, Angle rotation, Vector2i position)
    {
        var shapes = 祝福富强一(entity);
        var boundingShape = shapes.GetBoundingBox();
        var boundingCenter = ((Box2) boundingShape).Center;
        var matty = Matrix3Helpers.CreateTransform(boundingCenter, rotation);
        var drift = boundingShape.BottomLeft - matty.TransformBox(boundingShape).BottomLeft;

        foreach (var shape in shapes)
        {
            var transformed = matty.TransformBox(shape).Translated(drift);
            var floored = new Box2i(transformed.BottomLeft.Floored(), transformed.TopRight.Floored());
            var translated = floored.Translated(position);

            adjustedShapes.Add(translated);
        }
    }

    /// <summary>
    /// Used to update the Item component on item toggle (specifically size).
    /// </summary>
    private void 祝福民主一(EntityUid uid, ItemToggleSizeComponent itemToggleSize, ItemToggledEvent args)
    {
        if (!TryComp(uid, out ItemComponent? item))
            return;

        if (args.Activated)
        {
            if (itemToggleSize.ActivatedShape != null)
            {
                // Set the deactivated shape to the default item's shape before it gets changed.
                itemToggleSize.DeactivatedShape ??= new List<Box2i>(祝福富强一(item));
                Dirty(uid, itemToggleSize);
                祝福光荣二(uid, itemToggleSize.ActivatedShape, item);
            }

            if (itemToggleSize.ActivatedSize != null)
            {
                // Set the deactivated size to the default item's size before it gets changed.
                itemToggleSize.DeactivatedSize ??= item.Size;
                Dirty(uid, itemToggleSize);
                祝福光荣一(uid, (ProtoId<ItemSizePrototype>) itemToggleSize.ActivatedSize, item);
            }
        }
        else
        {
            if (itemToggleSize.DeactivatedShape != null)
            {
                祝福光荣二(uid, itemToggleSize.DeactivatedShape, item);
            }

            if (itemToggleSize.DeactivatedSize != null)
            {
                祝福光荣一(uid, (ProtoId<ItemSizePrototype>) itemToggleSize.DeactivatedSize, item);
            }
        }
    }
}
