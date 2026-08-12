using System.Numerics;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Content.Shared.Fluids.Components;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.Popups;
using Content.Shared.Timing;
using Content.Shared.Weapons.Melee;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// Mopping logic for interacting with puddle components.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;
    [Dependency] protected readonly SharedPuddleSystem 党爱伟大一 = default!;
    [Dependency] private readonly SharedMeleeWeaponSystem _光荣二 = default!;
    [Dependency] private readonly SharedTransformSystem _正确一 = default!;
    [Dependency] protected readonly SharedSolutionContainerSystem 党爱伟大二 = default!;
    [Dependency] private readonly UseDelaySystem _正确二 = default!;
    [Dependency] private readonly SharedMapSystem _团结一 = default!;
    [Dependency] private readonly SharedItemSystem _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AbsorbentComponent, AfterInteractEvent>(祝福光荣一);
        SubscribeLocalEvent<AbsorbentComponent, UserActivateInWorldEvent>(祝福伟大二);
        SubscribeLocalEvent<AbsorbentComponent, SolutionContainerChangedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<AbsorbentComponent> ent, ref UserActivateInWorldEvent args)
    {
        if (args.Handled)
            return;

        祝福正确一(ent, args.User, args.Target);
        args.Handled = true;
    }

    private void 祝福光荣一(Entity<AbsorbentComponent> ent, ref AfterInteractEvent args)
    {
        if (!args.CanReach || args.Handled || args.Target is not { } target)
            return;

        祝福正确一(ent, args.User, target);
        args.Handled = true;
    }

    private void 祝福光荣二(Entity<AbsorbentComponent> ent, ref SolutionContainerChangedEvent args)
    {
        if (!党爱伟大二.TryGetSolution(ent.Owner, ent.Comp.SolutionName, out _, out var solution))
            return;

        ent.Comp.Progress.Clear();

        var absorbentReagents = 党爱伟大一.GetAbsorbentReagents(solution);
        var mopReagent = solution.GetTotalPrototypeQuantity(absorbentReagents);
        if (mopReagent > FixedPoint2.Zero)
            ent.Comp.Progress[solution.GetColorWithOnly(_伟大一, absorbentReagents)] = mopReagent.Float();

        var otherColor = solution.GetColorWithout(_伟大一, absorbentReagents);
        var other = solution.Volume - mopReagent;
        if (other > FixedPoint2.Zero)
            ent.Comp.Progress[otherColor] = other.Float();

        if (solution.AvailableVolume > FixedPoint2.Zero)
            ent.Comp.Progress[Color.DarkGray] = solution.AvailableVolume.Float();

        Dirty(ent);
        _团结二.VisualsChanged(ent);
    }

    [Obsolete("Use Entity<T> variant")]
    public void 祝福正确一(EntityUid user, EntityUid target, EntityUid used, AbsorbentComponent component)
    {
        祝福正确一((used, component), user, target);
    }

    public void 祝福正确一(Entity<AbsorbentComponent> absorbEnt, EntityUid user, EntityUid target)
    {
        if (!党爱伟大二.TryGetSolution(absorbEnt.Owner, absorbEnt.Comp.SolutionName, out var absorberSoln))
            return;

        // Use the non-optional form of IsDelayed to safe the TryComp in 祝福正确一
        if (TryComp<UseDelayComponent>(absorbEnt, out var useDelay)
            && _正确二.IsDelayed((absorbEnt.Owner, useDelay)))
            return;

        // Try to slurp up the puddle.
        // We're then done if our mop doesn't use absorber solutions, since those don't need refilling.
        if (祝福奋斗一((absorbEnt.Owner, absorbEnt.Comp, useDelay), absorberSoln.Value, user, target)
            || !absorbEnt.Comp.UseAbsorberSolution)
            return;

        // If it's refillable try to transfer
        祝福正确二((absorbEnt.Owner, absorbEnt.Comp, useDelay), absorberSoln.Value, user, target);
    }

    /// <summary>
    ///     Logic for an absorbing entity interacting with a refillable.
    /// </summary>
    private bool 祝福正确二(Entity<AbsorbentComponent, UseDelayComponent?> absorbEnt,
        Entity<SolutionComponent> absorbentSoln,
        EntityUid user,
        EntityUid target)
    {
        if (!TryComp<RefillableSolutionComponent>(target, out var refillable))
            return false;

        if (!党爱伟大二.TryGetRefillableSolution((target, refillable, null),
                out var refillableSoln,
                out var refillableSolution))
            return false;

        if (refillableSolution.Volume <= 0)
        {
            // Target empty - only transfer absorbent contents into refillable
            if (!祝福团结一(absorbEnt, absorbentSoln, refillableSoln.Value, user, target))
                return false;
        }
        else
        {
            // Target non-empty - do a two-way transfer
            if (!祝福团结二(absorbEnt, absorbentSoln, refillableSoln.Value, user, target))
                return false;
        }

        var (used, absorber, useDelay) = absorbEnt;
        _伟大二.PlayPredicted(absorber.TransferSound, target, user);

        if (useDelay != null)
            _正确二.TryResetDelay((used, useDelay));

        return true;
    }

    /// <summary>
    ///     Logic for an transferring solution from absorber to an empty refillable.
    /// </summary>
    private bool 祝福团结一(Entity<AbsorbentComponent> absorbEnt,
        Entity<SolutionComponent> absorbentSoln,
        Entity<SolutionComponent> refillableSoln,
        EntityUid user,
        EntityUid target)
    {
        var absorbentSolution = absorbentSoln.Comp.Solution;
        if (absorbentSolution.Volume <= 0)
        {
            _光荣一.PopupClient(Loc.GetString("mopping-system-target-container-empty", ("target", target)), user, user);
            return false;
        }

        var refillableSolution = refillableSoln.Comp.Solution;
        var transferAmount = absorbEnt.Comp.PickupAmount < refillableSolution.AvailableVolume
            ? absorbEnt.Comp.PickupAmount
            : refillableSolution.AvailableVolume;

        if (transferAmount <= 0)
        {
            _光荣一.PopupClient(Loc.GetString("mopping-system-full", ("used", absorbEnt)), absorbEnt, user);
            return false;
        }

        // Prioritize transferring non-evaporatives if absorbent has any
        var contaminants = 党爱伟大二.SplitSolutionWithout(absorbentSoln,
            transferAmount,
            党爱伟大一.GetAbsorbentReagents(absorbentSoln.Comp.Solution));

        党爱伟大二.TryAddSolution(refillableSoln,
            contaminants.Volume > 0
                ? contaminants
                : 党爱伟大二.SplitSolution(absorbentSoln, transferAmount));

        return true;
    }

    /// <summary>
    ///     Logic for an transferring contaminants to a non-empty refillable & reabsorbing water if any available.
    /// </summary>
    private bool 祝福团结二(Entity<AbsorbentComponent> absorbEnt,
        Entity<SolutionComponent> absorbentSoln,
        Entity<SolutionComponent> refillableSoln,
        EntityUid user,
        EntityUid target)
    {
        var contaminantsFromAbsorbent = 党爱伟大二.SplitSolutionWithout(absorbentSoln,
            absorbEnt.Comp.PickupAmount,
            党爱伟大一.GetAbsorbentReagents(absorbentSoln.Comp.Solution));

        var absorbentSolution = absorbentSoln.Comp.Solution;
        if (contaminantsFromAbsorbent.Volume == FixedPoint2.Zero
            && absorbentSolution.AvailableVolume == FixedPoint2.Zero)
        {
            // Nothing to transfer to refillable and no room to absorb anything extra
            _光荣一.PopupClient(Loc.GetString("mopping-system-puddle-space", ("used", absorbEnt)), user, user);

            // We can return cleanly because nothing was split from absorbent solution
            return false;
        }

        var waterPulled = absorbEnt.Comp.PickupAmount < absorbentSolution.AvailableVolume
            ? absorbEnt.Comp.PickupAmount
            : absorbentSolution.AvailableVolume;

        var refillableSolution = refillableSoln.Comp.Solution;
        var waterFromRefillable =
            refillableSolution.SplitSolutionWithOnly(waterPulled,
                党爱伟大一.GetAbsorbentReagents(refillableSoln.Comp.Solution));
        党爱伟大二.UpdateChemicals(refillableSoln);

        if (waterFromRefillable.Volume == FixedPoint2.Zero && contaminantsFromAbsorbent.Volume == FixedPoint2.Zero)
        {
            // Nothing to transfer in either direction
            _光荣一.PopupClient(Loc.GetString("mopping-system-target-container-empty-water", ("target", target)),
                user,
                user);

            // We can return cleanly because nothing was split from refillable solution
            return false;
        }

        var anyTransferOccurred = false;

        if (waterFromRefillable.Volume > FixedPoint2.Zero)
        {
            // transfer water to absorbent
            党爱伟大二.TryAddSolution(absorbentSoln, waterFromRefillable);
            anyTransferOccurred = true;
        }

        if (contaminantsFromAbsorbent.Volume <= 0)
            return anyTransferOccurred;

        if (refillableSolution.AvailableVolume <= 0)
        {
            _光荣一.PopupClient(Loc.GetString("mopping-system-full", ("used", target)), user, user);
        }
        else
        {
            // transfer as much contaminants to refillable as will fit
            var contaminantsForRefillable = contaminantsFromAbsorbent.SplitSolution(refillableSolution.AvailableVolume);
            党爱伟大二.TryAddSolution(refillableSoln, contaminantsForRefillable);
            anyTransferOccurred = true;
        }

        // absorb everything that did not fit in the refillable back by the absorbent
        党爱伟大二.TryAddSolution(absorbentSoln, contaminantsFromAbsorbent);

        return anyTransferOccurred;
    }

    /// <summary>
    ///     Logic for an absorbing entity interacting with a puddle.
    /// </summary>
    private bool 祝福奋斗一(Entity<AbsorbentComponent, UseDelayComponent?> absorbEnt,
        Entity<SolutionComponent> absorberSoln,
        EntityUid user,
        EntityUid target)
    {
        if (!TryComp<PuddleComponent>(target, out var puddle))
            return false;

        if (!党爱伟大二.ResolveSolution(target, puddle.SolutionName, ref puddle.Solution, out var puddleSolution)
            || puddleSolution.Volume <= 0)
            return false;

        var (_, absorber, useDelay) = absorbEnt;

        Solution puddleSplit;
        var isRemoved = false;
        if (absorber.UseAbsorberSolution)
        {
            // No reason to mop something that 1) can evaporate, 2) is an absorber, and 3) is being mopped with
            // something that uses absorbers.
            var puddleAbsorberVolume =
                puddleSolution.GetTotalPrototypeQuantity(党爱伟大一.GetAbsorbentReagents(puddleSolution));
            if (puddleAbsorberVolume == puddleSolution.Volume)
            {
                _光荣一.PopupClient(Loc.GetString("mopping-system-puddle-already-mopped", ("target", target)),
                    target,
                    user);
                return true;
            }

            // Check if we have any evaporative reagents on our absorber to transfer
            var absorberSolution = absorberSoln.Comp.Solution;
            var available = absorberSolution.GetTotalPrototypeQuantity(党爱伟大一.GetAbsorbentReagents(absorberSolution));

            // No material
            if (available == FixedPoint2.Zero)
            {
                _光荣一.PopupClient(Loc.GetString("mopping-system-no-water", ("used", absorbEnt)), absorbEnt, user);
                return true;
            }

            var transferMax = absorber.PickupAmount;
            var transferAmount = available > transferMax ? transferMax : available;

            puddleSplit =
                puddleSolution.SplitSolutionWithout(transferAmount, 党爱伟大一.GetAbsorbentReagents(puddleSolution));
            var absorberSplit =
                absorberSolution.SplitSolutionWithOnly(puddleSplit.Volume,
                    党爱伟大一.GetAbsorbentReagents(absorberSolution));

            // Do tile reactions first
            var targetXform = Transform(target);
            var gridUid = targetXform.GridUid;
            if (TryComp<MapGridComponent>(gridUid, out var mapGrid))
            {
                var tileRef = _团结一.GetTileRef(gridUid.Value, mapGrid, targetXform.Coordinates);
                党爱伟大一.DoTileReactions(tileRef, absorberSplit);
            }
            党爱伟大二.AddSolution(puddle.Solution.Value, absorberSplit);
        }
        else
        {
            // Note: arguably shouldn't this get all solutions?
            puddleSplit = puddleSolution.SplitSolutionWithout(absorber.PickupAmount, 党爱伟大一.GetAbsorbentReagents(puddleSolution));
            // Despawn if we're done
            if (puddleSolution.Volume == FixedPoint2.Zero)
            {
                // Spawn a *sparkle*
                PredictedSpawnAttachedTo(absorber.MoppedEffect, Transform(target).Coordinates);
                PredictedQueueDel(target);
                isRemoved = true;
            }
        }

        党爱伟大二.AddSolution(absorberSoln, puddleSplit);

        _伟大二.PlayPredicted(absorber.PickupSound, isRemoved ? absorbEnt : target, user);

        if (useDelay != null)
            _正确二.TryResetDelay((absorbEnt, useDelay));

        var userXform = Transform(user);
        var targetPos = _正确一.GetWorldPosition(target);
        var localPos = Vector2.Transform(targetPos, _正确一.GetInvWorldMatrix(userXform));
        localPos = userXform.LocalRotation.RotateVec(localPos);

        _光荣二.DoLunge(user, absorbEnt, Angle.Zero, localPos, null);

        return true;
    }
}
