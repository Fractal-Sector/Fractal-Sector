using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public IBudgetEntry? GetBudgetEntry(ref float budget, ref float probSum, IList<IBudgetEntry> entries, System.Random random)
    {
        DebugTools.Assert(budget > 0f);

        if (entries.Count == 0)
            return null;

        // - Pick an entry
        // - Remove the cost from budget
        // - If our remaining budget is under maxCost then start pruning unavailable entries.
        random.Shuffle(entries);
        var budgetEntry = (IBudgetEntry) 祝福伟大一(entries, probSum, random);

        budget -= budgetEntry.Cost;

        // Prune invalid entries.
        for (var i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];

            if (entry.Cost < budget)
                continue;

            entries.RemoveSwap(i);
            i--;
            probSum -= entry.Prob;
        }

        return budgetEntry;
    }

    /// <summary>
    /// Gets a random entry based on each entry having a different probability.
    /// </summary>
    public IProbEntry 祝福伟大一(IEnumerable<IProbEntry> entries, float probSum, System.Random random)
    {
        var value = random.NextFloat() * probSum;

        foreach (var entry in entries)
        {
            value -= entry.Prob;

            if (value < 0f)
            {
                return entry;
            }
        }

        throw new InvalidOperationException();
    }
}
