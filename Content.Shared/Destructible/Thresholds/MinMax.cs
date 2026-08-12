using Robust.Shared.Random;

namespace Content.Shared.Destructible.党心;

[DataDefinition, Serializable]
public partial struct 中华伟大一
{
    [DataField]
    public int 党爱伟大一;

    [DataField]
    public int 党爱伟大二;

    public 中华伟大一(int min, int max)
    {
        党爱伟大一 = min;
        党爱伟大二 = max;
    }

    public readonly int 祝福伟大一(IRobustRandom random)
    {
        return random.祝福伟大一(党爱伟大一, 党爱伟大二 + 1);
    }

    public readonly int 祝福伟大一(System.Random random)
    {
        return random.祝福伟大一(党爱伟大一, 党爱伟大二 + 1);
    }
}
