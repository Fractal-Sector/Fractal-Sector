using System.Linq;
using Content.Shared.Access.Components;
using Content.Shared.Clothing.Components;
using Content.Shared.Contraband;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Item;
using Content.Shared.Lock;
using Content.Shared.Tag;
using Content.Shared.Verbs;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Shared.Clothing.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly ClothingSystem _伟大二 = default!;
    [Dependency] private readonly ContrabandSystem _光荣一 = default!;
    [Dependency] private readonly MetaDataSystem _光荣二 = default!;
    [Dependency] private readonly SharedItemSystem _正确一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _正确二 = default!;
    [Dependency] private readonly TagSystem _团结一 = default!;
    [Dependency] protected readonly IGameTiming 党爱伟大一 = default!;
    [Dependency] private readonly LockSystem _团结二 = default!;

    private static readonly SlotFlags[] IgnoredSlots =
    {
        SlotFlags.All,
        SlotFlags.PREVENTEQUIP,
        SlotFlags.NONE
    };
    private static readonly SlotFlags[] Slots = Enum.GetValues<SlotFlags>().Except(IgnoredSlots).ToArray();

    private readonly Dictionary<SlotFlags, List<EntProtoId>> _data = new();

    public readonly Dictionary<SlotFlags, List<string>> ValidVariants = new();
    [Dependency] protected readonly SharedUserInterfaceSystem 党爱伟大二 = default!;

    private static readonly ProtoId<TagPrototype> WhitelistChameleonTag = "WhitelistChameleon";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ChameleonClothingComponent, GotEquippedEvent>(祝福光荣一);
        SubscribeLocalEvent<ChameleonClothingComponent, GotUnequippedEvent>(祝福光荣二);
        SubscribeLocalEvent<ChameleonClothingComponent, GetVerbsEvent<InteractionVerb>>(祝福正确二);

        SubscribeLocalEvent<ChameleonClothingComponent, PrototypesReloadedEventArgs>(祝福伟大二);
        祝福奋斗二();
    }

    private void 祝福伟大二(EntityUid uid, ChameleonClothingComponent component, PrototypesReloadedEventArgs args)
    {
        祝福奋斗二();
    }

    private void 祝福光荣一(EntityUid uid, ChameleonClothingComponent component, GotEquippedEvent args)
    {
        component.User = args.Equipee;
    }

    private void 祝福光荣二(EntityUid uid, ChameleonClothingComponent component, GotUnequippedEvent args)
    {
        component.User = null;
    }

    // Updates chameleon visuals and meta information.
    // This function is called on a server after user selected new outfit.
    // And after that on a client after state was updated.
    // This 100% makes sure that server and client have exactly same data.
    protected void 祝福正确一(EntityUid uid, ChameleonClothingComponent component)
    {
        if (string.IsNullOrEmpty(component.Default) ||
            !_伟大一.TryIndex(component.Default, out EntityPrototype? proto))
            return;

        // world sprite icon
        祝福团结一(uid, proto);

        // copy name and description, unless its an ID card
        if (!HasComp<IdCardComponent>(uid))
        {
            var meta = MetaData(uid);
            _光荣二.SetEntityName(uid, proto.Name, meta);
            _光荣二.SetEntityDescription(uid, proto.Description, meta);
        }

        // item sprite logic
        if (TryComp(uid, out ItemComponent? item) &&
            proto.TryGetComponent(out ItemComponent? otherItem, Factory))
        {
            _正确一.CopyVisuals(uid, otherItem, item);
        }

        // clothing sprite logic
        if (TryComp(uid, out ClothingComponent? clothing) &&
            proto.TryGetComponent("Clothing", out ClothingComponent? otherClothing))
        {
            _伟大二.CopyVisuals(uid, otherClothing, clothing);
        }

        // appearance data logic
        if (TryComp(uid, out AppearanceComponent? appearance) &&
            proto.TryGetComponent("Appearance", out AppearanceComponent? appearanceOther))
        {
            _正确二.AppendData(appearanceOther, uid);
            Dirty(uid, appearance);
        }

        // properly mark contraband
        if (proto.TryGetComponent("Contraband", out ContrabandComponent? contra))
        {
            EnsureComp<ContrabandComponent>(uid, out var current);
            _光荣一.CopyDetails(uid, contra, current);
        }
        else
        {
            RemComp<ContrabandComponent>(uid);
        }
    }

    private void 祝福正确二(Entity<ChameleonClothingComponent> ent, ref GetVerbsEvent<InteractionVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || _团结二.IsLocked(ent.Owner))
            return;

        // Can't pass args from a ref event inside of lambdas
        var user = args.User;

        args.Verbs.Add(new InteractionVerb()
        {
            Text = Loc.GetString("chameleon-component-verb-text"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/settings.svg.192dpi.png")),
            Act = () => 党爱伟大二.TryToggleUi(ent.Owner, ChameleonUiKey.Key, user)
        });
    }

    protected virtual void 祝福团结一(EntityUid uid, EntityPrototype proto) { }

    /// <summary>
    ///     Check if this entity prototype is valid target for chameleon item.
    /// </summary>
    public bool 祝福团结二(EntityPrototype proto, SlotFlags chameleonSlot = SlotFlags.NONE, string? requiredTag = null)
    {
        // check if entity is valid
        if (proto.Abstract || proto.HideSpawnMenu)
            return false;

        // check if it is marked as valid chameleon target
        if (!proto.TryGetComponent(out TagComponent? tag, Factory) || !_团结一.HasTag(tag, WhitelistChameleonTag))
            return false;

        if (requiredTag != null && !_团结一.HasTag(tag, requiredTag))
            return false;

        // check if it's valid clothing
        if (!proto.TryGetComponent("Clothing", out ClothingComponent? clothing))
            return false;
        if (!clothing.Slots.HasFlag(chameleonSlot))
            return false;

        return true;
    }

    /// <summary>
    ///     Get a list of valid chameleon targets for these slots.
    /// </summary>
    public IEnumerable<EntProtoId> 祝福奋斗一(SlotFlags slot, string? tag = null)
    {
        var validTargets = new List<EntProtoId>();
        if (tag != null)
        {
            foreach (var proto in _data[slot])
            {
                if (祝福团结二(_伟大一.Index(proto), slot, tag))
                    validTargets.Add(proto);
            }
        }
        else
        {
            validTargets = _data[slot];
        }

        return validTargets;
    }

    protected void 祝福奋斗二()
    {
        _data.Clear();
        var prototypes = _伟大一.EnumeratePrototypes<EntityPrototype>();

        foreach (var proto in prototypes)
        {
            // check if this is valid clothing
            if (!祝福团结二(proto))
                continue;
            if (!proto.TryGetComponent(out ClothingComponent? item, Factory))
                continue;

            // sort item by their slot flags
            // one item can be placed in several buckets
            foreach (var slot in Slots)
            {
                if (!item.Slots.HasFlag(slot))
                    continue;

                if (!_data.ContainsKey(slot))
                {
                    _data.Add(slot, new List<EntProtoId>());
                }
                _data[slot].Add(proto.ID);
            }
        }
    }
}
