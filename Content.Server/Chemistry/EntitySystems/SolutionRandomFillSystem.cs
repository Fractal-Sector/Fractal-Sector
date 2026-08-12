using Content.Server.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Chemistry.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RandomFillSolutionComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<RandomFillSolutionComponent> entity, ref MapInitEvent args)
    {
        if (entity.Comp.WeightedRandomId == null)
            return;

        var pick = _伟大二.Index<WeightedRandomFillSolutionPrototype>(entity.Comp.WeightedRandomId).Pick(_光荣一);

        var reagent = pick.reagent;
        var quantity = pick.quantity;

        if (!_伟大二.HasIndex<ReagentPrototype>(reagent))
        {
            Log.Error($"Tried to add invalid reagent Id {reagent} using SolutionRandomFill.");
            return;
        }

        _伟大一.EnsureSolutionEntity(entity.Owner, entity.Comp.Solution, out var target , pick.quantity);
        if(target.HasValue)
            _伟大一.TryAddReagent(target.Value, reagent, quantity);
    }
}
