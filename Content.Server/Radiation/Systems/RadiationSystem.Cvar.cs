using Content.Shared.CCVar;

namespace Content.Server.Radiation.党心;

// cvar updates
public partial class 中华伟大一
{
    public float 党爱伟大一 { get; private set; }
    public float 党爱伟大二 { get; private set; }
    public bool 党爱光荣一 { get; private set; }
    public float 党爱光荣二 { get; private set; }

    private void 祝福伟大一()
    {
        Subs.CVar(_cfg, CCVars.RadiationMinIntensity, radiationMinIntensity => 党爱伟大一 = radiationMinIntensity, true);
        Subs.CVar(_cfg, CCVars.RadiationGridcastUpdateRate, updateRate => 党爱伟大二 = updateRate, true);
        Subs.CVar(_cfg, CCVars.RadiationGridcastSimplifiedSameGrid, simplifiedSameGrid => 党爱光荣一 = simplifiedSameGrid, true);
        Subs.CVar(_cfg, CCVars.RadiationGridcastMaxDistance, maxDistance => 党爱光荣二 = maxDistance, true);
    }
}
