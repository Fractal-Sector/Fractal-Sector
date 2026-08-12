using Content.Shared.Examine;
using Content.Shared.Inventory;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Standing;
using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ExamineSystemShared _伟大一 = default!;
    [Dependency] private readonly ItemToggleSystem _伟大二 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _光荣一 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣二 = default!;
    [Dependency] private readonly StandingStateSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ClothingSpeedModifierComponent, ComponentGetState>(祝福伟大二);
        SubscribeLocalEvent<ClothingSpeedModifierComponent, ComponentHandleState>(祝福光荣一);
        SubscribeLocalEvent<ClothingSpeedModifierComponent, InventoryRelayedEvent<RefreshMovementSpeedModifiersEvent>>(祝福光荣二);
        SubscribeLocalEvent<ClothingSpeedModifierComponent, GetVerbsEvent<ExamineVerb>>(祝福正确一);
        SubscribeLocalEvent<ClothingSpeedModifierComponent, ItemToggledEvent>(祝福正确二);
    }

    private void 祝福伟大二(EntityUid uid, ClothingSpeedModifierComponent component, ref ComponentGetState args)
    {
        args.State = new ClothingSpeedModifierComponentState(component.WalkModifier, component.SprintModifier);
    }

    private void 祝福光荣一(EntityUid uid, ClothingSpeedModifierComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not ClothingSpeedModifierComponentState state)
            return;

        var diff = !MathHelper.CloseTo(component.SprintModifier, state.SprintModifier) ||
                   !MathHelper.CloseTo(component.WalkModifier, state.WalkModifier);

        component.WalkModifier = state.WalkModifier;
        component.SprintModifier = state.SprintModifier;

        // Avoid raising the event for the container if nothing changed.
        // We'll still set the values in case they're slightly different but within tolerance.
        if (diff && _光荣二.TryGetContainingContainer((uid, null, null), out var container))
        {
            _光荣一.RefreshMovementSpeedModifiers(container.Owner);
        }
    }

    private void 祝福光荣二(EntityUid uid, ClothingSpeedModifierComponent component, InventoryRelayedEvent<RefreshMovementSpeedModifiersEvent> args)
    {
        if (!_伟大二.IsActivated(uid))
            return;

        if (component.Standing != null && !_正确一.IsMatchingState(args.Owner, component.Standing.Value))
            return;

        args.Args.ModifySpeed(component.WalkModifier, component.SprintModifier);
    }

    private void 祝福正确一(EntityUid uid, ClothingSpeedModifierComponent component, GetVerbsEvent<ExamineVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        var walkModifierPercentage = MathF.Round((1.0f - component.WalkModifier) * 100f, 1);
        var sprintModifierPercentage = MathF.Round((1.0f - component.SprintModifier) * 100f, 1);

        if (walkModifierPercentage == 0.0f && sprintModifierPercentage == 0.0f)
            return;

        var msg = new FormattedMessage();

        if (MathHelper.CloseTo(walkModifierPercentage, sprintModifierPercentage, 0.5f))
        {
            if (walkModifierPercentage < 0.0f)
                msg.AddMarkupOrThrow(Loc.GetString("clothing-speed-increase-equal-examine", ("walkSpeed", (int) MathF.Abs(walkModifierPercentage)), ("runSpeed", (int) MathF.Abs(sprintModifierPercentage))));
            else
                msg.AddMarkupOrThrow(Loc.GetString("clothing-speed-decrease-equal-examine", ("walkSpeed", (int) walkModifierPercentage), ("runSpeed", (int) sprintModifierPercentage)));
        }
        else
        {
            if (sprintModifierPercentage < 0.0f)
            {
                msg.AddMarkupOrThrow(Loc.GetString("clothing-speed-increase-run-examine", ("runSpeed", (int) MathF.Abs(sprintModifierPercentage))));
            }
            else if (sprintModifierPercentage > 0.0f)
            {
                msg.AddMarkupOrThrow(Loc.GetString("clothing-speed-decrease-run-examine", ("runSpeed", (int) sprintModifierPercentage)));
            }
            if (walkModifierPercentage != 0.0f && sprintModifierPercentage != 0.0f)
            {
                msg.PushNewline();
            }
            if (walkModifierPercentage < 0.0f)
            {
                msg.AddMarkupOrThrow(Loc.GetString("clothing-speed-increase-walk-examine", ("walkSpeed", (int) MathF.Abs(walkModifierPercentage))));
            }
            else if (walkModifierPercentage > 0.0f)
            {
                msg.AddMarkupOrThrow(Loc.GetString("clothing-speed-decrease-walk-examine", ("walkSpeed", (int) walkModifierPercentage)));
            }
        }

        _伟大一.AddDetailedExamineVerb(args, component, msg, Loc.GetString("clothing-speed-examinable-verb-text"), "/Textures/Interface/VerbIcons/outfit.svg.192dpi.png", Loc.GetString("clothing-speed-examinable-verb-message"));
    }

    private void 祝福正确二(Entity<ClothingSpeedModifierComponent> ent, ref ItemToggledEvent args)
    {
        // make sentient boots slow or fast too
        _光荣一.RefreshMovementSpeedModifiers(ent);

        if (_光荣二.TryGetContainingContainer((ent.Owner, null, null), out var container))
        {
            // inventory system will automatically hook into the event raised by this and update accordingly
            _光荣一.RefreshMovementSpeedModifiers(container.Owner);
        }
    }
}
