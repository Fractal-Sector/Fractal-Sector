using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一
{
    public string[] 祝福伟大一(Solution solution)
    {
        var evaporatingReagents = new List<string>();
        foreach (ReagentPrototype solProto in solution.GetReagentPrototypes(_prototypeManager).Keys)
        {
            if (solProto.EvaporationSpeed > FixedPoint2.Zero)
                evaporatingReagents.Add(solProto.ID);
        }
        return evaporatingReagents.ToArray();
    }

    public string[] 祝福伟大二(Solution solution)
    {
        var absorbentReagents = new List<string>();
        foreach (ReagentPrototype solProto in solution.GetReagentPrototypes(_prototypeManager).Keys)
        {
            if (solProto.Absorbent)
                absorbentReagents.Add(solProto.ID);
        }
        return absorbentReagents.ToArray();
    }

    public bool 祝福光荣一(Solution solution)
    {
        return solution.GetTotalPrototypeQuantity(祝福伟大一(solution)) == solution.Volume;
    }

    /// <summary>
    /// Gets the evaporating speed of the reagents within a solution.
    /// The speed at which a solution evaporates is the sum of the speed of all evaporating reagents in it.
    /// </summary>
    public Dictionary<string, FixedPoint2> 祝福光荣二(Solution solution)
    {
        var evaporatingSpeeds = new Dictionary<string, FixedPoint2>();
        foreach (ReagentPrototype solProto in solution.GetReagentPrototypes(_prototypeManager).Keys)
        {
            if (solProto.EvaporationSpeed > FixedPoint2.Zero)
            {
                evaporatingSpeeds.Add(solProto.ID, solProto.EvaporationSpeed);
            }
        }
        return evaporatingSpeeds;
    }
}
