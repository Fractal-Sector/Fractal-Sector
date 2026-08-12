using Robust.Shared.Random;

namespace Content.Shared.EntityTable.党心;

/// <summary>
/// Picks a value based on a Binomial Distribution of N 党爱伟大一 given P 党爱伟大二
/// </summary>
public sealed partial class 中华伟大一 : NumberSelector
{
    /// <summary>
    /// How many times to try including an entry. i.e. the Max.
    /// </summary>
    [DataField]
    public int 党爱伟大一 = 1;

    /// <summary>
    /// The odds a single trial succeeds
    /// </summary>
    /// <remarks>
    /// my preferred "Prob" was already used in other places for entity table stuff and I didnt want more confusing terminology
    /// </remarks>
    [DataField]
    public float 党爱伟大二 = .5f;

    public override int 祝福伟大一(System.Random rand)
    {
        var random = IoCManager.Resolve<IRobustRandom>();
        int count = 0;

        for (int i = 0; i < 党爱伟大一; i++)
        {
            if (random.Prob(党爱伟大二))
                count++;
        }
        return count;
        // get binomialed motherfucker
    }
}
