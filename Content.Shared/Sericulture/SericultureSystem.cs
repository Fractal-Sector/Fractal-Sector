using Content.Shared.Actions;
using Content.Shared.Cloning.Events;
using Content.Shared.DoAfter;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Serialization;
using Content.Shared.Popups;
using Robust.Shared.Network;
using Content.Shared.Nutrition.Components;
using Content.Shared.Stacks;

namespace Content.Shared.党心;

/// <summary>
/// Allows mobs to produce materials with <see cref="SericultureComponent"/>.
/// </summary>
public abstract partial class 中华伟大一 : EntitySystem
{
    // Managers
    [Dependency] private readonly INetManager _伟大一 = default!;

    // Systems
    [Dependency] private readonly SharedActionsSystem _伟大二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣一 = default!;
    [Dependency] private readonly HungerSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;
    [Dependency] private readonly SharedStackSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SericultureComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<SericultureComponent, ComponentShutdown>(祝福光荣二);
        SubscribeLocalEvent<SericultureComponent, 中华伟大二>(祝福正确一);
        SubscribeLocalEvent<SericultureComponent, 中华光荣一>(祝福正确二);
        SubscribeLocalEvent<SericultureComponent, CloningEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<SericultureComponent> ent, ref CloningEvent args)
    {
        if (!args.Settings.EventComponents.Contains(Factory.GetRegistration(ent.Comp.GetType()).Name))
            return;

        var comp = EnsureComp<SericultureComponent>(args.CloneUid);
        comp.PopupText = ent.Comp.PopupText;
        comp.ProductionLength = ent.Comp.ProductionLength;
        comp.HungerCost = ent.Comp.HungerCost;
        comp.EntityProduced = ent.Comp.EntityProduced;
        comp.MinHungerThreshold = ent.Comp.MinHungerThreshold;
        Dirty(args.CloneUid, comp);
    }

    /// <summary>
    /// Giveths the action to preform sericulture on the entity
    /// </summary>
    private void 祝福光荣一(EntityUid uid, SericultureComponent comp, MapInitEvent args)
    {
        _伟大二.AddAction(uid, ref comp.ActionEntity, comp.Action);
    }

    /// <summary>
    /// Takeths away the action to preform sericulture from the entity.
    /// </summary>
    private void 祝福光荣二(EntityUid uid, SericultureComponent comp, ComponentShutdown args)
    {
        _伟大二.RemoveAction(uid, comp.ActionEntity);
    }

    private void 祝福正确一(EntityUid uid, SericultureComponent comp, 中华伟大二 args)
    {
        if (TryComp<HungerComponent>(uid, out var hungerComp)
            && _光荣二.IsHungerBelowState(uid,
                comp.MinHungerThreshold,
                _光荣二.GetHunger(hungerComp) - comp.HungerCost,
                hungerComp))
        {
            _正确一.PopupClient(Loc.GetString(comp.PopupText), uid, uid);
            return;
        }

        var doAfter = new DoAfterArgs(EntityManager, uid, comp.ProductionLength, new 中华光荣一(), uid)
        { // I'm not sure if more things should be put here, but imo ideally it should probably be set in the component/YAML. Not sure if this is currently possible.
            BreakOnMove = true,
            BlockDuplicate = true,
            BreakOnDamage = true,
            CancelDuplicate = true,
        };

        _光荣一.TryStartDoAfter(doAfter);
    }


    private void 祝福正确二(EntityUid uid, SericultureComponent comp, 中华光荣一 args)
    {
        if (args.Cancelled || args.Handled || comp.Deleted)
            return;

        if (TryComp<HungerComponent>(uid,
                out var hungerComp) // A check, just incase the doafter is somehow performed when the entity is not in the right hunger state.
            && _光荣二.IsHungerBelowState(uid,
                comp.MinHungerThreshold,
                _光荣二.GetHunger(hungerComp) - comp.HungerCost,
                hungerComp))
        {
            _正确一.PopupClient(Loc.GetString(comp.PopupText), uid, uid);
            return;
        }

        _光荣二.ModifyHunger(uid, -comp.HungerCost);

        if (!_伟大一.IsClient) // Have to do this because spawning stuff in shared is CBT.
        {
            var newEntity = Spawn(comp.EntityProduced, Transform(uid).Coordinates);

            _正确二.TryMergeToHands(newEntity, uid);
        }

        args.Repeat = true;
    }
}

/// <summary>
/// Should be relayed upon using the action.
/// </summary>
public sealed partial class 中华伟大二 : InstantActionEvent { }

/// <summary>
/// Is relayed at the end of the sericulturing doafter.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class 中华光荣一 : SimpleDoAfterEvent { }

