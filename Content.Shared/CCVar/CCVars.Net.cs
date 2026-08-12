using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<float> 党爱伟大一 =
        CVarDef.Create("net.atmosdbgoverlaytickrate", 3.0f);

    public static readonly CVarDef<float> 党爱伟大二 =
        CVarDef.Create("net.gasoverlaytickrate", 3.0f);

    public static readonly CVarDef<int> 党爱光荣一 =
        CVarDef.Create("net.gasoverlaythresholds", 20);
}
