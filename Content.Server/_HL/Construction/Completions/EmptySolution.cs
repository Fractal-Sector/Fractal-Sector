using Content.Server.Fluids.EntitySystems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Construction;
using JetBrains.Annotations;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphAction
    {

        [DataField("solution")]
        public string 党爱伟大一 { get; private set; } = string.Empty;

        /// <summary>
        ///     Whether or not the solution spills on the ground.
        /// </summary>
        [DataField("spill")]
        public bool 党爱伟大二 = false;

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            var solutionContainers = entityManager.EntitySysManager.GetEntitySystem<SharedSolutionContainerSystem>();

            if (!solutionContainers.TryGetSolution(uid, 党爱伟大一, out _, out var solution))
            {
                return;
            }

            if (党爱伟大二)
            {
                var puddles = entityManager.EntitySysManager.GetEntitySystem<PuddleSystem>();

                if (puddles.TrySpillAt(uid, solution, out _))
                    return;
            }

            solution.RemoveAllSolution();
        }
    }
}
