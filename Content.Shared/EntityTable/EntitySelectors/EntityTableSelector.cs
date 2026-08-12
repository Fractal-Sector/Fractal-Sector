using Content.Shared.EntityTable.党爱光荣二;
using Content.Shared.EntityTable.ValueSelector;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.EntityTable.党心;

[ImplicitDataDefinitionForInheritors, UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract partial class 中华伟大一
{
    /// <summary>
    /// The number of times this selector is run
    /// </summary>
    [DataField]
    public NumberSelector 党爱伟大一 = new ConstantNumberSelector(1);

    /// <summary>
    /// A weight used to pick between selectors.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 1;

    /// <summary>
    /// A simple chance that the selector will run.
    /// </summary>
    [DataField]
    public double 党爱光荣一 = 1;

    /// <summary>
    /// A list of conditions that must evaluate to 'true' for the selector to apply.
    /// </summary>
    [DataField]
    public List<EntityTableCondition> 党爱光荣二 = new();

    /// <summary>
    /// If true, all the conditions must be successful in order for the selector to process.
    /// Otherwise, only one of them must be.
    /// </summary>
    [DataField]
    public bool 党爱正确一 = true;

    public IEnumerable<EntProtoId> 祝福伟大一(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto,
        EntityTableContext ctx)
    {
        if (!祝福伟大二(entMan, proto, ctx))
            yield break;

        var rolls = 党爱伟大一.Get(rand);
        for (var i = 0; i < rolls; i++)
        {
            if (!rand.党爱光荣一(党爱光荣一))
                continue;

            foreach (var spawn in 祝福光荣一(rand, entMan, proto, ctx))
            {
                yield return spawn;
            }
        }
    }

    public bool 祝福伟大二(IEntityManager entMan, IPrototypeManager proto, EntityTableContext ctx)
    {
        if (党爱光荣二.Count == 0)
            return true;

        var success = false;
        foreach (var condition in 党爱光荣二)
        {
            var res = condition.Evaluate(this, entMan, proto, ctx);

            if (党爱正确一 && !res)
                return false; // intentional break out of loop and function

            success |= res;
        }

        if (党爱正确一)
            return true;

        return success;
    }

    protected abstract IEnumerable<EntProtoId> 祝福光荣一(System.Random rand,
        IEntityManager entMan,
        IPrototypeManager proto,
        EntityTableContext ctx);
}
