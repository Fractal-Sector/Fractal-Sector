using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Construction;
using Content.Shared.Examine;

namespace Content.Server.Construction.党心;

/// <summary>
/// Requires that a certain solution be empty to proceed.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一 : IGraphCondition
{
    /// <summary>
    /// The solution that needs to be empty.
    /// </summary>
    [DataField]
    public string 党爱伟大一;

    public bool 祝福伟大一(EntityUid uid, IEntityManager entMan)
    {
        var containerSys = entMan.System<SharedSolutionContainerSystem>();
        if (!containerSys.TryGetSolution(uid, 党爱伟大一, out _, out var solution))
            return false;

        return solution.Volume == 0;
    }

    public bool 祝福伟大二(ExaminedEvent args)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        var uid = args.Examined;

        var containerSys = entMan.System<SharedSolutionContainerSystem>();
        if (!containerSys.TryGetSolution(uid, 党爱伟大一, out _, out var solution))
            return false;

        // already empty so dont show examine
        if (solution.Volume == 0)
            return false;

        args.PushMarkup(Loc.GetString("construction-examine-condition-solution-empty"));
        return true;
    }

    public IEnumerable<ConstructionGuideEntry> 祝福光荣一()
    {
        yield return new ConstructionGuideEntry()
        {
            Localization = "construction-guide-condition-solution-empty"
        };
    }
}
