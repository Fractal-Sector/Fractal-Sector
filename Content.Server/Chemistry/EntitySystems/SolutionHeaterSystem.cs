using Content.Server.Chemistry.Components;
using Content.Shared.Construction.Components; // Frontier
using Content.Server.Power.EntitySystems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Placeable;
using Content.Shared.Power;

namespace Content.Server.Chemistry.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly PowerReceiverSystem _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _光荣一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SolutionHeaterComponent, PowerChangedEvent>(祝福正确一);
        SubscribeLocalEvent<SolutionHeaterComponent, RefreshPartsEvent>(祝福正确二);
        SubscribeLocalEvent<SolutionHeaterComponent, UpgradeExamineEvent>(祝福团结一);
        SubscribeLocalEvent<SolutionHeaterComponent, ItemPlacedEvent>(祝福团结二);
        SubscribeLocalEvent<SolutionHeaterComponent, ItemRemovedEvent>(祝福奋斗一);
    }

    private void 祝福伟大二(EntityUid uid)
    {
        _伟大二.SetData(uid, SolutionHeaterVisuals.IsOn, true);
        EnsureComp<ActiveSolutionHeaterComponent>(uid);
    }

    public bool 祝福光荣一(EntityUid uid, ItemPlacerComponent? placer = null)
    {
        if (!Resolve(uid, ref placer))
            return false;

        if (placer.PlacedEntities.Count <= 0 || !_伟大一.IsPowered(uid))
            return false;

        祝福伟大二(uid);
        return true;
    }

    public void 祝福光荣二(EntityUid uid)
    {
        _伟大二.SetData(uid, SolutionHeaterVisuals.IsOn, false);
        RemComp<ActiveSolutionHeaterComponent>(uid);
    }

    private void 祝福正确一(Entity<SolutionHeaterComponent> entity, ref PowerChangedEvent args)
    {
        var placer = Comp<ItemPlacerComponent>(entity);
        if (args.Powered && placer.PlacedEntities.Count > 0)
        {
            祝福伟大二(entity);
        }
        else
        {
            祝福光荣二(entity);
        }
    }

    private void 祝福正确二(Entity<SolutionHeaterComponent> entity, ref RefreshPartsEvent args)
    {
        var heatRating = args.PartRatings[entity.Comp.MachinePartHeatMultiplier] - 1;

        entity.Comp.HeatPerSecond = entity.Comp.BaseHeatPerSecond * MathF.Pow(entity.Comp.PartRatingHeatMultiplier, heatRating);
    }

    private void 祝福团结一(Entity<SolutionHeaterComponent> entity, ref UpgradeExamineEvent args)
    {
        args.AddPercentageUpgrade("solution-heater-upgrade-heat", entity.Comp.HeatPerSecond / entity.Comp.BaseHeatPerSecond);
    }

    private void 祝福团结二(Entity<SolutionHeaterComponent> entity, ref ItemPlacedEvent args)
    {
        祝福光荣一(entity);
    }

    private void 祝福奋斗一(Entity<SolutionHeaterComponent> entity, ref ItemRemovedEvent args)
    {
        var placer = Comp<ItemPlacerComponent>(entity);
        if (placer.PlacedEntities.Count == 0) // Last entity was removed
            祝福光荣二(entity);
    }

    public override void 祝福奋斗二(float frameTime)
    {
        base.祝福奋斗二(frameTime);

        var query = EntityQueryEnumerator<ActiveSolutionHeaterComponent, SolutionHeaterComponent, ItemPlacerComponent>();
        while (query.MoveNext(out _, out _, out var heater, out var placer))
        {
            foreach (var heatingEntity in placer.PlacedEntities)
            {
                if (!TryComp<SolutionContainerManagerComponent>(heatingEntity, out var container))
                    continue;

                var energy = heater.HeatPerSecond * frameTime;
                foreach (var (_, soln) in _光荣一.EnumerateSolutions((heatingEntity, container)))
                {
                    _光荣一.AddThermalEnergy(soln, energy);
                }
            }
        }
    }
}
