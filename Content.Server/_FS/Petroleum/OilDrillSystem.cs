using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Content.Shared._FS.Petroleum;
using Content.Server.Chemistry.Containers.EntitySystems;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using System;

namespace Content.Server._FS.Petroleum;

public sealed class OilDrillSystem : SharedOilDrillSystem
{
    [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = AllEntityQuery<OilDrillComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var drill, out var xform))
        {
            if (!drill.Active)
                continue;

            if (xform.GridUid == null)
                continue;

            Entity<TransformComponent> drillEntity = (uid, xform);
            var drillPos = _transform.GetMapCoordinates(drillEntity);

            LiquidDepositComponent? deposit = null;

            foreach (var entity in _lookup.GetEntitiesInRange(drillPos, 0.2f))
            {
                EntityUid targetUid = entity;
                if (TryComp<LiquidDepositComponent>(targetUid, out var foundDeposit))
                {
                    deposit = foundDeposit;
                    break;
                }
            }

            if (deposit == null || deposit.Amount <= 0)
                continue;

            if (!_solutionContainer.TryGetSolution(uid, drill.SolutionId, out var solutionHolder, out var solution))
                continue;

            float availableSpace = (float) (solution.MaxVolume - solution.Volume);
            if (availableSpace <= 0)
                continue;

            float toExtract = Math.Min(drill.ExtractRate, deposit.Amount);
            toExtract = Math.Min(toExtract, availableSpace);

            if (toExtract <= 0)
                continue;

            deposit.Amount -= toExtract;

            _solutionContainer.TryAddReagent(solutionHolder.Value, deposit.ReagentId, FixedPoint2.New(toExtract), out _);
        }
    }
}
