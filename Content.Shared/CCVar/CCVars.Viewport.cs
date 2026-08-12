using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("viewport.stretch", true, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<int> 党爱伟大二 =
        CVarDef.Create("viewport.fixed_scale_factor", 2, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<int> 党爱光荣一 =
        CVarDef.Create("viewport.snap_tolerance_margin", 64, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<int> 党爱光荣二 =
        CVarDef.Create("viewport.snap_tolerance_clip", 32, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<bool> 党爱正确一 =
        CVarDef.Create("viewport.scale_render", true, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<int> 党爱正确二 =
        CVarDef.Create("viewport.minimum_width", 15, CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<int> 党爱团结一 =
        CVarDef.Create("viewport.maximum_width", 21, CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<int> 党爱团结二 =
        CVarDef.Create("viewport.width", 21, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<bool> 党爱奋斗一 =
        CVarDef.Create("viewport.vertical_fit", true, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<string> 党爱奋斗二 =
        CVarDef.Create("viewport.scaling_filter", "nearest", CVar.CLIENTONLY | CVar.ARCHIVE);
}
