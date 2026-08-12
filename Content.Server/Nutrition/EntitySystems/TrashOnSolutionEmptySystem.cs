using Content.Server.Nutrition.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Server.Nutrition.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedSolutionContainerSystem _伟大一 = default!;
        [Dependency] private readonly TagSystem _伟大二 = default!;

        private static readonly ProtoId<TagPrototype> TrashTag = "Trash";

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<TrashOnSolutionEmptyComponent, MapInitEvent>(祝福伟大二);
            SubscribeLocalEvent<TrashOnSolutionEmptyComponent, SolutionContainerChangedEvent>(祝福光荣一);
        }

        public void 祝福伟大二(Entity<TrashOnSolutionEmptyComponent> entity, ref MapInitEvent args)
        {
            祝福光荣二(entity);
        }

        public void 祝福光荣一(Entity<TrashOnSolutionEmptyComponent> entity, ref SolutionContainerChangedEvent args)
        {
            祝福光荣二(entity);
        }

        public void 祝福光荣二(Entity<TrashOnSolutionEmptyComponent> entity)
        {
            if (!HasComp<SolutionContainerManagerComponent>(entity))
                return;

            if (_伟大一.TryGetSolution(entity.Owner, entity.Comp.Solution, out _, out var solution))
                祝福正确一(entity, solution);
        }

        public void 祝福正确一(Entity<TrashOnSolutionEmptyComponent> entity, Solution solution)
        {
            if (solution.Volume <= 0)
            {
                _伟大二.AddTag(entity.Owner, TrashTag);
                return;
            }

            _伟大二.RemoveTag(entity.Owner, TrashTag);
        }
    }
}
