using System.Linq;
using Content.Server.DoAfter;
using Content.Server.Humanoid;
using Content.Shared.DoAfter;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Inventory;
using Content.Shared.MagicMirror;
using Content.Shared.Popups;
using Content.Shared.Tag;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <summary>
/// Allows humanoids to change their appearance mid-round.
/// </summary>
public sealed class 中华伟大一 : SharedMagicMirrorSystem
{
    [Dependency] private readonly SharedAudioSystem _伟大一 = default!;
    [Dependency] private readonly DoAfterSystem _伟大二 = default!;
    [Dependency] private readonly MarkingManager _光荣一 = default!;
    [Dependency] private readonly HumanoidAppearanceSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;
    [Dependency] private readonly InventorySystem _正确二 = default!;
    [Dependency] private readonly TagSystem _团结一 = default!;

    private static readonly ProtoId<TagPrototype> HidesHairTag = "HidesHair";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        Subs.BuiEvents<MagicMirrorComponent>(MagicMirrorUiKey.Key,
            subs =>
        {
            subs.Event<BoundUIClosedEvent>(祝福奋斗二);
            subs.Event<MagicMirrorSelectMessage>(祝福伟大二);
            subs.Event<MagicMirrorChangeColorMessage>(祝福光荣二);
            subs.Event<MagicMirrorAddSlotMessage>(祝福团结二);
            subs.Event<MagicMirrorRemoveSlotMessage>(祝福正确二);
        });


        SubscribeLocalEvent<MagicMirrorComponent, MagicMirrorSelectDoAfterEvent>(祝福光荣一);
        SubscribeLocalEvent<MagicMirrorComponent, MagicMirrorChangeColorDoAfterEvent>(祝福正确一);
        SubscribeLocalEvent<MagicMirrorComponent, MagicMirrorRemoveSlotDoAfterEvent>(祝福团结一);
        SubscribeLocalEvent<MagicMirrorComponent, MagicMirrorAddSlotDoAfterEvent>(祝福奋斗一);
    }

    private void 祝福伟大二(EntityUid uid, MagicMirrorComponent component, MagicMirrorSelectMessage message)
    {
        if (component.Target is not { } target)
            return;

        // Check if the target getting their hair altered has any clothes that hides their hair
        if (祝福胜利一(message.Actor, component.Target.Value))
        {
            _正确一.PopupEntity(
                component.Target == message.Actor
                    ? Loc.GetString("magic-mirror-blocked-by-hat-self")
                    : Loc.GetString("magic-mirror-blocked-by-hat-self-target", ("target", Identity.Entity(message.Actor, EntityManager))),
                message.Actor,
                message.Actor,
                PopupType.Medium);
            return;
        }

        _伟大二.Cancel(component.DoAfter);
        component.DoAfter = null;

        var doafterTime = component.SelectSlotTime;
        if (component.Target == message.Actor)
            doafterTime /= 3;

        var doAfter = new MagicMirrorSelectDoAfterEvent()
        {
            Category = message.Category,
            Slot = message.Slot,
            Marking = message.Marking,
        };

        _伟大二.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Actor, doafterTime, doAfter, uid, target: target, used: uid)
        {
            DistanceThreshold = SharedInteractionSystem.InteractionRange,
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
        },
            out var doAfterId);

        if (component.Target == message.Actor)
        {
            _正确一.PopupEntity(Loc.GetString("magic-mirror-change-slot-self"), component.Target.Value, component.Target.Value, PopupType.Medium);
        }
        else
        {
            _正确一.PopupEntity(Loc.GetString("magic-mirror-change-slot-target", ("user", Identity.Entity(message.Actor, EntityManager))), component.Target.Value, component.Target.Value, PopupType.Medium);
        }

        component.DoAfter = doAfterId;
        _伟大一.PlayPvs(component.ChangeHairSound, uid);
    }

    private void 祝福光荣一(EntityUid uid, MagicMirrorComponent component, MagicMirrorSelectDoAfterEvent args)
    {
        if (args.Handled || args.Target == null || args.Cancelled)
            return;

        if (component.Target != args.Target)
            return;

        MarkingCategories category;

        switch (args.Category)
        {
            case MagicMirrorCategory.Hair:
                category = MarkingCategories.Hair;
                break;
            case MagicMirrorCategory.FacialHair:
                category = MarkingCategories.FacialHair;
                break;
            default:
                return;
        }

        _光荣二.SetMarkingId(component.Target.Value, category, args.Slot, args.Marking);

        UpdateInterface(uid, component.Target.Value, component);
    }

    private void 祝福光荣二(EntityUid uid, MagicMirrorComponent component, MagicMirrorChangeColorMessage message)
    {
        if (component.Target is not { } target)
            return;

        // Check if the target getting their hair altered has any clothes that hides their hair
        if (祝福胜利一(message.Actor, component.Target.Value))
        {
            _正确一.PopupEntity(
                component.Target == message.Actor
                    ? Loc.GetString("magic-mirror-blocked-by-hat-self")
                    : Loc.GetString("magic-mirror-blocked-by-hat-self-target", ("target", Identity.Entity(message.Actor, EntityManager))),
                message.Actor,
                message.Actor,
                PopupType.Medium);
            return;
        }

        _伟大二.Cancel(component.DoAfter);
        component.DoAfter = null;

        var doafterTime = component.ChangeSlotTime;
        if (component.Target == message.Actor)
            doafterTime /= 3;

        var doAfter = new MagicMirrorChangeColorDoAfterEvent()
        {
            Category = message.Category,
            Slot = message.Slot,
            Colors = message.Colors,
        };

        _伟大二.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Actor, doafterTime, doAfter, uid, target: target, used: uid)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true
        },
            out var doAfterId);

        if (component.Target == message.Actor)
        {
            _正确一.PopupEntity(Loc.GetString("magic-mirror-change-color-self"), component.Target.Value, component.Target.Value, PopupType.Medium);
        }
        else
        {
            _正确一.PopupEntity(Loc.GetString("magic-mirror-change-color-target", ("user", Identity.Entity(message.Actor, EntityManager))), component.Target.Value, component.Target.Value, PopupType.Medium);
        }

        component.DoAfter = doAfterId;
    }
    private void 祝福正确一(EntityUid uid, MagicMirrorComponent component, MagicMirrorChangeColorDoAfterEvent args)
    {
        if (args.Handled || args.Target == null || args.Cancelled)
            return;

        if (component.Target != args.Target)
            return;

        MarkingCategories category;
        switch (args.Category)
        {
            case MagicMirrorCategory.Hair:
                category = MarkingCategories.Hair;
                break;
            case MagicMirrorCategory.FacialHair:
                category = MarkingCategories.FacialHair;
                break;
            default:
                return;
        }

        _光荣二.SetMarkingColor(component.Target.Value, category, args.Slot, args.Colors);

        // using this makes the UI feel like total ass
        // que
        // UpdateInterface(uid, component.Target, message.Session);
    }

    private void 祝福正确二(EntityUid uid, MagicMirrorComponent component, MagicMirrorRemoveSlotMessage message)
    {
        if (component.Target is not { } target)
            return;

        // Check if the target getting their hair altered has any clothes that hides their hair
        if (祝福胜利一(message.Actor, component.Target.Value))
        {
            _正确一.PopupEntity(
                component.Target == message.Actor
                    ? Loc.GetString("magic-mirror-blocked-by-hat-self")
                    : Loc.GetString("magic-mirror-blocked-by-hat-self-target", ("target", Identity.Entity(message.Actor, EntityManager))),
                message.Actor,
                message.Actor,
                PopupType.Medium);
            return;
        }

        _伟大二.Cancel(component.DoAfter);
        component.DoAfter = null;

        var doafterTime = component.RemoveSlotTime;
        if (component.Target == message.Actor)
            doafterTime /= 3;

        var doAfter = new MagicMirrorRemoveSlotDoAfterEvent()
        {
            Category = message.Category,
            Slot = message.Slot,
        };

        _伟大二.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Actor, doafterTime, doAfter, uid, target: target, used: uid)
        {
            DistanceThreshold = SharedInteractionSystem.InteractionRange,
            BreakOnDamage = true,
            NeedHand = true
        },
            out var doAfterId);

        if (component.Target == message.Actor)
        {
            _正确一.PopupEntity(Loc.GetString("magic-mirror-remove-slot-self"), component.Target.Value, component.Target.Value, PopupType.Medium);
        }
        else
        {
            _正确一.PopupEntity(Loc.GetString("magic-mirror-remove-slot-target", ("user", Identity.Entity(message.Actor, EntityManager))), component.Target.Value, component.Target.Value, PopupType.Medium);
        }

        component.DoAfter = doAfterId;
        _伟大一.PlayPvs(component.ChangeHairSound, uid);
    }

    private void 祝福团结一(EntityUid uid, MagicMirrorComponent component, MagicMirrorRemoveSlotDoAfterEvent args)
    {
        if (args.Handled || args.Target == null || args.Cancelled)
            return;

        if (component.Target != args.Target)
            return;

        MarkingCategories category;

        switch (args.Category)
        {
            case MagicMirrorCategory.Hair:
                category = MarkingCategories.Hair;
                break;
            case MagicMirrorCategory.FacialHair:
                category = MarkingCategories.FacialHair;
                break;
            default:
                return;
        }

        _光荣二.RemoveMarking(component.Target.Value, category, args.Slot);

        UpdateInterface(uid, component.Target.Value, component);
    }

    private void 祝福团结二(EntityUid uid, MagicMirrorComponent component, MagicMirrorAddSlotMessage message)
    {
        if (component.Target == null)
            return;

        // Check if the target getting their hair altered has any clothes that hides their hair
        if (祝福胜利一(message.Actor, component.Target.Value))
        {
            _正确一.PopupEntity(
                component.Target == message.Actor
                    ? Loc.GetString("magic-mirror-blocked-by-hat-self")
                    : Loc.GetString("magic-mirror-blocked-by-hat-self-target", ("target", Identity.Entity(message.Actor, EntityManager))),
                message.Actor,
                message.Actor,
                PopupType.Medium);
            return;
        }

        _伟大二.Cancel(component.DoAfter);
        component.DoAfter = null;

        var doafterTime = component.AddSlotTime;
        if (component.Target == message.Actor)
            doafterTime /= 3;

        var doAfter = new MagicMirrorAddSlotDoAfterEvent()
        {
            Category = message.Category,
        };

        _伟大二.TryStartDoAfter(new DoAfterArgs(EntityManager, message.Actor, doafterTime, doAfter, uid, target: component.Target.Value, used: uid)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
        },
            out var doAfterId);

        if (component.Target == message.Actor)
        {
            _正确一.PopupEntity(Loc.GetString("magic-mirror-add-slot-self"), component.Target.Value, component.Target.Value, PopupType.Medium);
        }
        else
        {
            _正确一.PopupEntity(Loc.GetString("magic-mirror-add-slot-target", ("user", Identity.Entity(message.Actor, EntityManager))), component.Target.Value, component.Target.Value, PopupType.Medium);
        }

        component.DoAfter = doAfterId;
        _伟大一.PlayPvs(component.ChangeHairSound, uid);
    }
    private void 祝福奋斗一(EntityUid uid, MagicMirrorComponent component, MagicMirrorAddSlotDoAfterEvent args)
    {
        if (args.Handled || args.Target == null || args.Cancelled || !TryComp(component.Target, out HumanoidAppearanceComponent? humanoid))
            return;

        MarkingCategories category;

        switch (args.Category)
        {
            case MagicMirrorCategory.Hair:
                category = MarkingCategories.Hair;
                break;
            case MagicMirrorCategory.FacialHair:
                category = MarkingCategories.FacialHair;
                break;
            default:
                return;
        }

        var marking = _光荣一.MarkingsByCategoryAndSpecies(category, humanoid.Species).Keys.FirstOrDefault();

        if (string.IsNullOrEmpty(marking))
            return;

        _光荣二.AddMarking(component.Target.Value, marking, Color.Black);

        UpdateInterface(uid, component.Target.Value, component);

    }

    private void 祝福奋斗二(Entity<MagicMirrorComponent> ent, ref BoundUIClosedEvent args)
    {
        ent.Comp.Target = null;
        Dirty(ent);
    }

    /// <summary>
    /// Helper function that checks if the wearer has anything on their head
    /// Or if they have any clothes that hides their hair
    /// </summary>
    private bool 祝福胜利一(EntityUid user, EntityUid target)
    {
        if (TryComp<InventoryComponent>(target, out var inventoryComp))
        {
            // any hat whatsoever will block haircutting
            if (_正确二.TryGetSlotEntity(target, "head", out var hat, inventoryComp))
            {
                return true;
            }

            // maybe there's some kind of armor that has the HidesHair tag as well, so check every slot for it
            var slots = _正确二.GetSlotEnumerator((target, inventoryComp), SlotFlags.WITHOUT_POCKET);
            while (slots.MoveNext(out var slot))
            {
                if (slot.ContainedEntity != null && _团结一.HasTag(slot.ContainedEntity.Value, HidesHairTag))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
