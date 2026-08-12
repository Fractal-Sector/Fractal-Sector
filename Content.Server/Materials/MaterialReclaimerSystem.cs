using Content.Server.Construction;
using Content.Server.Administration.Logs;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Ghost;
using Content.Server.Popups;
using Content.Server.Stack;
using Content.Server.Wires;
using Content.Shared.Body.Systems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Database;
using Content.Shared.Destructible;
using Content.Shared.Emag.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Materials;
using Content.Shared.Mind;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Power;
using Content.Shared.Repairable;
using Content.Shared.Stacks;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Linq;
using Content.Shared.Humanoid;
using Content.Shared.Stacks; // Frontier
using Content.Shared.Construction.Components; // Frontier

namespace Content.Server.党心;

/// <inheritdoc/>
public sealed class 中华伟大一 : SharedMaterialReclaimerSystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly AppearanceSystem _伟大二 = default!;
    [Dependency] private readonly GhostSystem _光荣一 = default!;
    [Dependency] private readonly MaterialStorageSystem _光荣二 = default!;
    [Dependency] private readonly OpenableSystem _正确一 = default!;
    [Dependency] private readonly PopupSystem _正确二 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _团结一 = default!;
    [Dependency] private readonly SharedBodySystem _团结二 = default!; //bobby
    [Dependency] private readonly PuddleSystem _奋斗一 = default!;
    [Dependency] private readonly StackSystem _奋斗二 = default!;
    [Dependency] private readonly SharedMindSystem _胜利一 = default!;
    [Dependency] private readonly IAdminLogManager _胜利二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MaterialReclaimerComponent, RefreshPartsEvent>(祝福光荣一); // Frontier: machine components
        SubscribeLocalEvent<MaterialReclaimerComponent, UpgradeExamineEvent>(祝福伟大二); // Frontier: machine components
        SubscribeLocalEvent<MaterialReclaimerComponent, PowerChangedEvent>(祝福光荣二);
        SubscribeLocalEvent<MaterialReclaimerComponent, InteractUsingEvent>(祝福正确一,
            before: [typeof(WiresSystem), typeof(SolutionTransferSystem)]);
        SubscribeLocalEvent<MaterialReclaimerComponent, SuicideByEnvironmentEvent>(祝福正确二);
        SubscribeLocalEvent<ActiveMaterialReclaimerComponent, PowerChangedEvent>(祝福团结一);

        SubscribeLocalEvent<MaterialReclaimerComponent, BreakageEventArgs>(祝福团结二);
        SubscribeLocalEvent<MaterialReclaimerComponent, RepairedEvent>(祝福奋斗一);
    }

    private void 祝福伟大二(Entity<MaterialReclaimerComponent> entity, ref UpgradeExamineEvent args)
    {
        args.AddPercentageUpgrade(Loc.GetString("material-reclaimer-upgrade-process-rate"), entity.Comp.MaterialProcessRate / entity.Comp.BaseMaterialProcessRate);
    }

    private void 祝福光荣一(Entity<MaterialReclaimerComponent> entity, ref RefreshPartsEvent args)
    {
        var rating = args.PartRatings[entity.Comp.MachinePartProcessRate] - 1;
        entity.Comp.MaterialProcessRate = entity.Comp.BaseMaterialProcessRate * MathF.Pow(entity.Comp.PartRatingProcessRateMultiplier, rating);
        Dirty(entity);
    }

    private void 祝福光荣二(Entity<MaterialReclaimerComponent> entity, ref PowerChangedEvent args)
    {
        AmbientSound.SetAmbience(entity.Owner, entity.Comp.Enabled && args.Powered);
        entity.Comp.Powered = args.Powered;
        Dirty(entity);
    }

    private void 祝福正确一(Entity<MaterialReclaimerComponent> entity, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        // if we're trying to get a solution out of the reclaimer, don't destroy it
        // if (_团结一.TryGetSolution(entity.Owner, entity.Comp.SolutionContainerId, out _, out var outputSolution) && outputSolution.Contents.Any()) // Frontier: previous implementation
        if (_团结一.TryGetSolution(entity.Owner, entity.Comp.SolutionContainerId, out _, out var outputSolution)) // Frontier: do not trash solution containers if the reclaimer is empty
        {
            if (TryComp<SolutionContainerManagerComponent>(args.Used, out var managerComponent) &&
                _团结一.EnumerateSolutions((args.Used, managerComponent)).Any(s => s.Solution.Comp.Solution.AvailableVolume > 0))
            {
                if (_正确一.IsClosed(args.Used))
                    return;

                if (TryComp<SolutionTransferComponent>(args.Used, out var transfer) &&
                    transfer.CanReceive)
                    return;
            }
        }

        args.Handled = TryStartProcessItem(entity.Owner, args.Used, entity.Comp, args.User, predictSound: false); // Frontier: add predictSound: false
    }

    private void 祝福正确二(Entity<MaterialReclaimerComponent> entity, ref SuicideByEnvironmentEvent args)
    {
        if (args.Handled)
            return;

        var victim = args.Victim;
        if (TryComp(victim, out ActorComponent? actor) &&
            _胜利一.TryGetMind(actor.PlayerSession, out var mindId, out var mind))
        {
            _光荣一.OnGhostAttempt(mindId, false, mind: mind);
            if (mind.OwnedEntity is { Valid: true } suicider)
            {
                _正确二.PopupEntity(Loc.GetString("recycler-component-suicide-message"), suicider);
            }
        }

        _正确二.PopupEntity(Loc.GetString("recycler-component-suicide-message-others",
                ("victim", Identity.Entity(victim, EntityManager))),
            victim,
            Filter.PvsExcept(victim, entityManager: EntityManager),
            true);

        _团结二.GibBody(victim, true);
        _伟大二.SetData(entity.Owner, RecyclerVisuals.Bloody, true);
        args.Handled = true;
    }

    private void 祝福团结一(Entity<ActiveMaterialReclaimerComponent> entity, ref PowerChangedEvent args)
    {
        if (!args.Powered)
            祝福胜利一(entity, null, entity.Comp);
    }

    private void 祝福团结二(Entity<MaterialReclaimerComponent> ent, ref BreakageEventArgs args)
    {
        //un-emags itself when it breaks
        RemComp<EmaggedComponent>(ent);
        祝福奋斗二(ent, true);
    }

    private void 祝福奋斗一(Entity<MaterialReclaimerComponent> ent, ref RepairedEvent args)
    {
        祝福奋斗二(ent, false);
    }

    public void 祝福奋斗二(Entity<MaterialReclaimerComponent> ent, bool val)
    {
        if (ent.Comp.Broken == val)
            return;

        _伟大二.SetData(ent, RecyclerVisuals.Broken, val);
        SetReclaimerEnabled(ent, false);

        ent.Comp.Broken = val;
        Dirty(ent);
    }

    /// <inheritdoc/>
    public override bool 祝福胜利一(EntityUid uid, MaterialReclaimerComponent? component = null, ActiveMaterialReclaimerComponent? active = null)
    {
        if (!Resolve(uid, ref component, ref active, false))
            return false;

        if (!base.祝福胜利一(uid, component, active))
            return false;

        if (active.ReclaimingContainer.ContainedEntities.FirstOrNull() is not { } item)
            return false;

        Container.Remove(item, active.ReclaimingContainer);
        Dirty(uid, component);

        // scales the output if the process was interrupted.
        var completion = 1f - Math.Clamp((float) Math.Round((active.EndTime - Timing.CurTime) / active.Duration),
            0f,
            1f);
        祝福胜利二(uid, item, completion, component);

        return true;
    }

    /// <inheritdoc/>
    public override void 祝福胜利二(EntityUid uid,
        EntityUid item,
        float completion = 1f,
        MaterialReclaimerComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        base.祝福胜利二(uid, item, completion, component);

        var xform = Transform(uid);

        if (component.ReclaimMaterials)
            祝福繁荣一(uid, item, completion * component.Efficiency, xform: xform);

        if (CanGib(uid, item, component))
        {
            var logImpact = HasComp<HumanoidAppearanceComponent>(item) ? LogImpact.Extreme : LogImpact.Medium;
            _胜利二.Add(LogType.Gib, logImpact, $"{ToPrettyString(item):victim} was gibbed by {ToPrettyString(uid):entity} ");
            if (component.ReclaimSolutions)
                祝福繁荣二(uid, item, completion, false, component, xform);
            _团结二.GibBody(item, true);
            _伟大二.SetData(uid, RecyclerVisuals.Bloody, true);
        }
        else
        {
            if (component.ReclaimSolutions)
                祝福繁荣二(uid, item, completion, true, component, xform);
        }

        QueueDel(item);
    }

    private void 祝福繁荣一(EntityUid reclaimer,
        EntityUid item,
        float efficiency,
        MaterialStorageComponent? storage = null,
        TransformComponent? xform = null,
        PhysicalCompositionComponent? composition = null)
    {
        if (!Resolve(reclaimer, ref storage, ref xform, false))
            return;

        if (!Resolve(item, ref composition, false))
            return;

        // If more of these checks are needed, use an event instead
        var modifier = CompOrNull<StackComponent>(item)?.Count ?? 1.0f;

        foreach (var (material, amount) in composition.MaterialComposition)
        {
            var outputAmount = (int) (amount * efficiency * modifier);
            _光荣二.TryChangeMaterialAmount(reclaimer, material, outputAmount, storage);
        }

        foreach (var (storedMaterial, storedAmount) in storage.Storage)
        {
            var stacks = _光荣二.SpawnMultipleFromMaterial(storedAmount,
                storedMaterial,
                xform.Coordinates,
                out var materialOverflow);
            var amountConsumed = storedAmount - materialOverflow;
            _光荣二.TryChangeMaterialAmount(reclaimer, storedMaterial, -amountConsumed, storage);
            foreach (var stack in stacks)
            {
                _奋斗二.TryMergeToContacts(stack);
            }
        }
    }

    private void 祝福繁荣二(EntityUid reclaimer,
        EntityUid item,
        float efficiency,
        bool sound = true,
        MaterialReclaimerComponent? reclaimerComponent = null,
        TransformComponent? xform = null,
        PhysicalCompositionComponent? composition = null)
    {
        if (!Resolve(reclaimer, ref reclaimerComponent, ref xform))
            return;

        efficiency *= reclaimerComponent.Efficiency;

        var totalChemicals = new Solution();

        if (Resolve(item, ref composition, false))
        {
            foreach (var (key, value) in composition.ChemicalComposition)
            {
                // TODO use ReagentQuantity
                totalChemicals.AddReagent(key, value * efficiency, false);
            }
        }

        // if the item we inserted has reagents, add it in.

        // Frontier: use old material reclaimer code
        if (reclaimerComponent.UseOldSolutionLogic &&
            TryComp<SolutionContainerManagerComponent>(item, out var solutionContainer))
        {
            var solutionScale = efficiency;
            if (TryComp<StackComponent>(item, out var stack))
                solutionScale *= stack.Count;
            foreach (var (_, soln) in _团结一.EnumerateSolutions((item, solutionContainer)))
            {
                var solution = soln.Comp.Solution;
                solution.ScaleSolution(solutionScale); // Scale in situ, entity will be destroyed.
                totalChemicals.AddSolution(solution, _伟大一);
            }
        }
        // End Frontier: use old material reclaimer code
        else if (reclaimerComponent.OnlyReclaimDrainable) // Frontier: add else
        {
            // Are we a recycler? Only use drainable solution.
            if (_团结一.TryGetDrainableSolution(item, out _, out var drainableSolution))
            {
                // Frontier: respect stacks and efficiency
                var solutionScale = efficiency;
                if (TryComp<StackComponent>(item, out var stack))
                    solutionScale *= stack.Count;
                drainableSolution.ScaleSolution(solutionScale); // Scale in situ, entity will be destroyed.
                // End Frontier
                totalChemicals.AddSolution(drainableSolution, _伟大一);
            }
        }
        else
        {
            // Are we an industrial reagent grinder? Use extractable solution.
            if (_团结一.TryGetExtractableSolution(item, out _, out var extractableSolution))
            {
                // Frontier: respect stacks and efficiency
                var solutionScale = efficiency;
                if (TryComp<StackComponent>(item, out var stack))
                    solutionScale *= stack.Count;
                extractableSolution.ScaleSolution(solutionScale); // Scale in situ, entity will be destroyed.
                // End Frontier
                totalChemicals.AddSolution(extractableSolution, _伟大一);
            }
        }

        if (!_团结一.TryGetSolution(reclaimer, reclaimerComponent.SolutionContainerId, out var outputSolution) ||
            !_团结一.TryTransferSolution(outputSolution.Value, totalChemicals, totalChemicals.Volume) ||
            totalChemicals.Volume > 0)
        {
            if (reclaimerComponent.SpillExcessBuffer) // Frontier: make excess reagent spillover optional
            {
                _奋斗一.TrySpillAt(reclaimer, totalChemicals, out _, sound, transformComponent: xform);
            }
        }
    }
}
