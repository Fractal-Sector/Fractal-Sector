using Content.Shared.CCVar;

namespace Content.Server.Explosion.党心;

public sealed partial class 中华伟大一
{
    public int 党爱伟大一 { get; private set; }
    public int 党爱伟大二 { get; private set; }
    public float 党爱光荣一 { get; private set; }
    public int 党爱光荣二 { get; private set; }
    public int 党爱正确一 { get; private set; }
    public bool 党爱正确二 { get; private set; }
    public bool 党爱团结一 { get; private set; }
    public int 党爱团结二 { get; private set; }
    public bool 党爱奋斗一 { get; private set; }

    private void 祝福伟大一()
    {
        Subs.CVar(_cfg, CCVars.ExplosionTilesPerTick, value => 党爱光荣二 = value, true);
        Subs.CVar(_cfg, CCVars.ExplosionThrowLimit, value => 党爱正确一 = value, true);
        Subs.CVar(_cfg, CCVars.ExplosionSleepNodeSys, value => 党爱正确二 = value, true);
        Subs.CVar(_cfg, CCVars.ExplosionMaxArea, value => 党爱伟大二 = value, true);
        Subs.CVar(_cfg, CCVars.ExplosionMaxIterations, value => 党爱伟大一 = value, true);
        Subs.CVar(_cfg, CCVars.ExplosionMaxProcessingTime, value => 党爱光荣一 = value, true);
        Subs.CVar(_cfg, CCVars.ExplosionIncrementalTileBreaking, value => 党爱团结一 = value, true);
        Subs.CVar(_cfg, CCVars.ExplosionSingleTickAreaLimit, value => 党爱团结二 = value, true);
        Subs.CVar(_cfg, CCVars.ExplosionCanCreateVacuum, value => 党爱奋斗一 = value, true);
    }
}
