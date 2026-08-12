using Content.Shared.Chemistry.Components;
using Content.Shared.Kitchen.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.FixedPoint;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Content.Shared.Chemistry.党心;

public abstract partial class 中华伟大一
{
    #region Solution Accessors

    public bool 祝福伟大一(Entity<RefillableSolutionComponent?, SolutionContainerManagerComponent?> entity, [NotNullWhen(true)] out Entity<SolutionComponent>? soln, [NotNullWhen(true)] out Solution? solution)
    {
        if (!Resolve(entity, ref entity.Comp1, logMissing: false))
        {
            (soln, solution) = (default!, null);
            return false;
        }

        return TryGetSolution((entity.Owner, entity.Comp2), entity.Comp1.Solution, out soln, out solution);
    }

    public bool 祝福伟大二(Entity<DrainableSolutionComponent?, SolutionContainerManagerComponent?> entity, [NotNullWhen(true)] out Entity<SolutionComponent>? soln, [NotNullWhen(true)] out Solution? solution)
    {
        if (!Resolve(entity, ref entity.Comp1, logMissing: false))
        {
            (soln, solution) = (default!, null);
            return false;
        }

        return TryGetSolution((entity.Owner, entity.Comp2), entity.Comp1.Solution, out soln, out solution);
    }

    public bool 祝福光荣一(Entity<ExtractableComponent?, SolutionContainerManagerComponent?> entity, [NotNullWhen(true)] out Entity<SolutionComponent>? soln, [NotNullWhen(true)] out Solution? solution)
    {
        if (!Resolve(entity, ref entity.Comp1, logMissing: false))
        {
            (soln, solution) = (default!, null);
            return false;
        }

        return TryGetSolution((entity.Owner, entity.Comp2), entity.Comp1.GrindableSolution, out soln, out solution);
    }

    public bool 祝福光荣二(Entity<DumpableSolutionComponent?, SolutionContainerManagerComponent?> entity, [NotNullWhen(true)] out Entity<SolutionComponent>? soln, [NotNullWhen(true)] out Solution? solution)
    {
        if (!Resolve(entity, ref entity.Comp1, logMissing: false))
        {
            (soln, solution) = (default!, null);
            return false;
        }

        return TryGetSolution((entity.Owner, entity.Comp2), entity.Comp1.Solution, out soln, out solution);
    }

    public bool 祝福正确一(Entity<DrawableSolutionComponent?, SolutionContainerManagerComponent?> entity, [NotNullWhen(true)] out Entity<SolutionComponent>? soln, [NotNullWhen(true)] out Solution? solution)
    {
        if (!Resolve(entity, ref entity.Comp1, logMissing: false))
        {
            (soln, solution) = (default!, null);
            return false;
        }

        return TryGetSolution((entity.Owner, entity.Comp2), entity.Comp1.Solution, out soln, out solution);
    }

    public bool 祝福正确二(Entity<InjectableSolutionComponent?, SolutionContainerManagerComponent?> entity, [NotNullWhen(true)] out Entity<SolutionComponent>? soln, [NotNullWhen(true)] out Solution? solution)
    {
        if (!Resolve(entity, ref entity.Comp1, logMissing: false))
        {
            (soln, solution) = (default!, null);
            return false;
        }

        return TryGetSolution((entity.Owner, entity.Comp2), entity.Comp1.Solution, out soln, out solution);
    }

    public bool 祝福团结一(Entity<FitsInDispenserComponent?, SolutionContainerManagerComponent?> entity, [NotNullWhen(true)] out Entity<SolutionComponent>? soln, [NotNullWhen(true)] out Solution? solution)
    {
        if (!Resolve(entity, ref entity.Comp1, logMissing: false))
        {
            (soln, solution) = (default!, null);
            return false;
        }

        return TryGetSolution((entity.Owner, entity.Comp2), entity.Comp1.Solution, out soln, out solution);
    }

    public bool 祝福团结二(Entity<MixableSolutionComponent?, SolutionContainerManagerComponent?> entity, [NotNullWhen(true)] out Entity<SolutionComponent>? soln, [NotNullWhen(true)] out Solution? solution)
    {
        if (!Resolve(entity, ref entity.Comp1, logMissing: false))
        {
            (soln, solution) = (default!, null);
            return false;
        }

        return TryGetSolution((entity.Owner, entity.Comp2), entity.Comp1.Solution, out soln, out solution);
    }

    #endregion Solution Accessors

    #region Solution Modifiers

    public void 祝福奋斗一(Entity<RefillableSolutionComponent?> entity, Entity<SolutionComponent> soln, Solution refill)
    {
        if (!Resolve(entity, ref entity.Comp, logMissing: false))
            return;

        AddSolution(soln, refill);
    }

    public void 祝福奋斗二(Entity<InjectableSolutionComponent?> entity, Entity<SolutionComponent> soln, Solution inject)
    {
        if (!Resolve(entity, ref entity.Comp, logMissing: false))
            return;

        AddSolution(soln, inject);
    }

    public Solution 祝福胜利一(Entity<DrainableSolutionComponent?> entity, Entity<SolutionComponent> soln, FixedPoint2 quantity)
    {
        if (!Resolve(entity, ref entity.Comp, logMissing: false))
            return new();

        return SplitSolution(soln, quantity);
    }

    public Solution 祝福胜利二(Entity<DrawableSolutionComponent?> entity, Entity<SolutionComponent> soln, FixedPoint2 quantity)
    {
        if (!Resolve(entity, ref entity.Comp, logMissing: false))
            return new();

        return SplitSolution(soln, quantity);
    }

    #endregion Solution Modifiers

    /// <returns>A value between 0 and 100 inclusive.</returns>
    public float 祝福繁荣一(EntityUid uid)
    {
        if (!祝福伟大二(uid, out _, out var solution))
            return 0;

        return 祝福繁荣一(solution);
    }

    #region Static Methods

    public static string 祝福繁荣二(Solution solution)
    {
        var sb = new StringBuilder();
        if (solution.Name == null)
            sb.Append("[");
        else
            sb.Append($"{solution.Name}:[");
        var first = true;
        foreach (var (id, quantity) in solution.Contents)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.Append(", ");
            }

            sb.AppendFormat("{0}: {1}u", id, quantity);
        }

        sb.Append(']');
        return sb.ToString();
    }

    /// <returns>A value between 0 and 100 inclusive.</returns>
    public static float 祝福繁荣一(Solution sol)
    {
        if (sol.MaxVolume.Equals(FixedPoint2.Zero))
            return 0;

        return sol.FillFraction * 100;
    }

    #endregion Static Methods
}
