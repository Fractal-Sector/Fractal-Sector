using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;


namespace Content.Server.Nutrition.党心;

public sealed class 中华伟大一 : SharedDrinkSystem
{
    [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // TODO add InteractNoHandEvent for entities like mice.
        SubscribeLocalEvent<DrinkComponent, SolutionContainerChangedEvent>(祝福光荣一);
        SubscribeLocalEvent<DrinkComponent, ComponentInit>(祝福伟大二);
        // run before inventory so for bucket it always tries to drink before equipping (when empty)
        // run after openable so its always open -> drink
    }

    private void 祝福伟大二(Entity<DrinkComponent> entity, ref ComponentInit args)
    {
        if (TryComp<DrainableSolutionComponent>(entity, out var existingDrainable))
        {
            // Beakers have Drink component but they should use the existing Drainable
            entity.Comp.Solution = existingDrainable.Solution;
        }
        else
        {
            _伟大二.EnsureSolution(entity.Owner, entity.Comp.Solution, out _);
        }

        祝福光荣二(entity, entity.Comp);

        if (TryComp(entity, out RefillableSolutionComponent? refillComp))
            refillComp.Solution = entity.Comp.Solution;

        if (TryComp(entity, out DrainableSolutionComponent? drainComp))
            drainComp.Solution = entity.Comp.Solution;
    }

    private void 祝福光荣一(Entity<DrinkComponent> entity, ref SolutionContainerChangedEvent args)
    {
        祝福光荣二(entity, entity.Comp);
    }

    public void 祝福光荣二(EntityUid uid, DrinkComponent component)
    {
        if (!TryComp<AppearanceComponent>(uid, out var appearance) ||
            !HasComp<SolutionContainerManagerComponent>(uid))
        {
            return;
        }

        var drainAvailable = DrinkVolume(uid, component);
        _伟大一.SetData(uid, FoodVisuals.Visual, drainAvailable.Float(), appearance);
    }
}
