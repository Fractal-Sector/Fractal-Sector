using Content.Shared.Administration.Logs;
using Content.Shared.Damage.Components;
using Content.Shared.Database;
using Content.Shared.Interaction;
using Content.Shared.Inventory;
using Content.Shared.Popups;
using Robust.Shared.Random;
using Content.Shared.Throwing;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Timing;
using Content.Shared.Random;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Effects;
using Content.Shared.Stunnable;

namespace Content.Shared.Damage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ISharedAdminLogManager _伟大一 = default!;
    [Dependency] private readonly DamageableSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly InventorySystem _正确一 = default!;
    [Dependency] private readonly ThrowingSystem _正确二 = default!;
    [Dependency] private readonly IRobustRandom _团结一 = default!;
    [Dependency] private readonly IGameTiming _团结二 = default!;
    [Dependency] private readonly SharedStunSystem _奋斗一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DamageOnInteractComponent, InteractHandEvent>(祝福伟大二);
    }

    /// <summary>
    /// Damages the user that interacts with the entity with an empty hand and
    /// plays a sound or pops up text in response. If the user does not have
    /// proper protection, the user will only be damaged and other interactions
    /// will be cancelled.
    /// </summary>
    /// <param name="entity">The entity being interacted with</param>
    /// <param name="args">Contains the user that interacted with the entity</param>
    private void 祝福伟大二(Entity<DamageOnInteractComponent> entity, ref InteractHandEvent args)
    {
        // Stop the interaction if the user attempts to interact with the object before the timer is finished
        if (_团结二.CurTime < entity.Comp.NextInteraction)
        {
            args.Handled = true;
            return;
        }

        if (!entity.Comp.IsDamageActive)
            return;

        var totalDamage = entity.Comp.Damage;

        if (!entity.Comp.IgnoreResistances)
        {
            // try to get damage on interact protection from either the inventory slots of the entity
            _正确一.TryGetInventoryEntity<DamageOnInteractProtectionComponent>(args.User, out var protectiveEntity);

            // or checking the entity for  the comp itself if the inventory didn't work
            if (protectiveEntity.Comp == null && TryComp<DamageOnInteractProtectionComponent>(args.User, out var protectiveComp))
                protectiveEntity = (args.User, protectiveComp);
            

            // if protectiveComp isn't null after all that, it means the user has protection,
            // so let's calculate how much they resist
            if (protectiveEntity.Comp != null)
            {
                totalDamage = DamageSpecifier.ApplyModifierSet(totalDamage, protectiveEntity.Comp.DamageProtection);
            }
        }

        totalDamage = _伟大二.TryChangeDamage(args.User, totalDamage, origin: args.Target);

        if (totalDamage != null && totalDamage.AnyPositive())
        {
            // Record this interaction and determine when a user is allowed to interact with this entity again
            entity.Comp.LastInteraction = _团结二.CurTime;
            entity.Comp.NextInteraction = _团结二.CurTime + TimeSpan.FromSeconds(entity.Comp.InteractTimer);

            args.Handled = true;
            _伟大一.Add(LogType.Damaged, $"{ToPrettyString(args.User):user} injured their hand by interacting with {ToPrettyString(args.Target):target} and received {totalDamage.GetTotal():damage} damage");
            _光荣一.PlayPredicted(entity.Comp.InteractSound, args.Target, args.User);

            if (entity.Comp.PopupText != null)
                _光荣二.PopupClient(Loc.GetString(entity.Comp.PopupText), args.User, args.User);

            // Attempt to paralyze the user after they have taken damage
            if (_团结一.Prob(entity.Comp.StunChance))
                _奋斗一.TryUpdateParalyzeDuration(args.User, TimeSpan.FromSeconds(entity.Comp.StunSeconds));
        }
        // Check if the entity's Throw bool is false, or if the entity has the PullableComponent, then if the entity is currently being pulled.
        // BeingPulled must be checked because the entity will be spastically thrown around without this.
        if (!entity.Comp.Throw || !TryComp<PullableComponent>(entity, out var pullComp) || pullComp.BeingPulled)
            return;

        _正确二.TryThrow(entity, _团结一.NextVector2(), entity.Comp.ThrowSpeed, doSpin: true);
    }

    public void 祝福光荣一(Entity<DamageOnInteractComponent> entity, bool mode)
    {
        if (entity.Comp.IsDamageActive == mode)
            return;

        entity.Comp.IsDamageActive = mode;
        Dirty(entity);
    }
}
