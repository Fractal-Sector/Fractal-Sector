using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("parallax.enabled", true, CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("parallax.debug", false, CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱光荣一 =
        CVarDef.Create("parallax.low_quality", false, CVar.ARCHIVE | CVar.CLIENTONLY);
}
