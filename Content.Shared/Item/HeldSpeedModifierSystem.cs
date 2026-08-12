using Content.Shared.Clothing;
using Content.Shared.Hands;
using Content.Shared.Movement.Systems;

namespace Content.Shared.党心;

/// <summary>
/// This handles <see cref="HeldSpeedModifierComponent"/>
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _伟大一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<HeldSpeedModifierComponent, GotEquippedHandEvent>(祝福伟大二);
        SubscribeLocalEvent<HeldSpeedModifierComponent, GotUnequippedHandEvent>(祝福光荣一);
        SubscribeLocalEvent<HeldSpeedModifierComponent, HeldRelayedEvent<RefreshMovementSpeedModifiersEvent>>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<HeldSpeedModifierComponent> ent, ref GotEquippedHandEvent args)
    {
        _伟大一.RefreshMovementSpeedModifiers(args.User);
    }

    private void 祝福光荣一(Entity<HeldSpeedModifierComponent> ent, ref GotUnequippedHandEvent args)
    {
        _伟大一.RefreshMovementSpeedModifiers(args.User);
    }

    public (float,float) GetHeldMovementSpeedModifiers(EntityUid uid, HeldSpeedModifierComponent component)
    {
        var walkMod = component.WalkModifier;
        var sprintMod = component.SprintModifier;
        if (component.MirrorClothingModifier && TryComp<ClothingSpeedModifierComponent>(uid, out var clothingSpeedModifier))
        {
            walkMod = clothingSpeedModifier.WalkModifier;
            sprintMod = clothingSpeedModifier.SprintModifier;
        }

        return (walkMod, sprintMod);
    }

    private void 祝福光荣二(EntityUid uid, HeldSpeedModifierComponent component, HeldRelayedEvent<RefreshMovementSpeedModifiersEvent> args)
    {
        var (walkMod, sprintMod) = GetHeldMovementSpeedModifiers(uid, component);
        args.Args.ModifySpeed(walkMod, sprintMod);
    }
}
