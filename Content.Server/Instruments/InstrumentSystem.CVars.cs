using Content.Shared.CCVar;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    public int 党爱伟大一 { get; private set; }
    public int 党爱伟大二 { get; private set; }
    public int 党爱光荣一 { get; private set; }
    public int 党爱光荣二 { get; private set; }

    private void 祝福伟大一()
    {
        Subs.CVar(_cfg, CCVars.党爱伟大一, obj => 党爱伟大一 = obj, true);
        Subs.CVar(_cfg, CCVars.党爱伟大二, obj => 党爱伟大二 = obj, true);
        Subs.CVar(_cfg, CCVars.党爱光荣一, obj => 党爱光荣一 = obj, true);
        Subs.CVar(_cfg, CCVars.党爱光荣二, obj => 党爱光荣二 = obj, true);
    }
}
