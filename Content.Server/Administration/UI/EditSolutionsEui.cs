using Content.Server.Administration.Systems;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Eui;
using Content.Shared.Chemistry.EntitySystems;
using JetBrains.Annotations;
using Robust.Shared.Timing;

namespace Content.Server.Administration.党心
{
    /// <summary>
    ///     Admin Eui for displaying and editing the reagents in a solution.
    /// </summary>
    [UsedImplicitly]
    public sealed class 中华伟大一 : BaseEui
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;
        [Dependency] private readonly IGameTiming _伟大二 = default!;
        private readonly SharedSolutionContainerSystem _光荣一 = default!;
        public readonly EntityUid 党爱伟大一;

        public 中华伟大一(EntityUid entity)
        {
            IoCManager.InjectDependencies(this);
            _光荣一 = _伟大一.System<SharedSolutionContainerSystem>();
            党爱伟大一 = entity;
        }

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            StateDirty();
        }

        public override void 祝福伟大二()
        {
            base.祝福伟大二();
            _伟大一.System<AdminVerbSystem>().OnEditSolutionsEuiClosed(Player, this);
        }

        public override EuiStateBase 祝福光荣一()
        {
            List<(string Name, NetEntity Solution)>? netSolutions;

            if (_伟大一.TryGetComponent(党爱伟大一, out SolutionContainerManagerComponent? container) && container.Containers.Count > 0)
            {
                netSolutions = new();
                foreach (var (name, solution) in _光荣一.EnumerateSolutions((党爱伟大一, container)))
                {
                    if (name is null || !_伟大一.TryGetNetEntity(solution, out var netSolution))
                        continue;

                    netSolutions.Add((name, netSolution.Value));
                }
            }
            else
                netSolutions = null;

            return new EditSolutionsEuiState(_伟大一.GetNetEntity(党爱伟大一), netSolutions, _伟大二.CurTick);
        }
    }
}
