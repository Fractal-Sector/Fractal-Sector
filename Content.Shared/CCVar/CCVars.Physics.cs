using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     When a mob is walking should its X / Y movement be relative to its parent (true) or the map (false).
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("physics.relative_movement", true, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<float> 党爱伟大二 =
        CVarDef.Create("physics.min_friction", 0.005f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER); // # Wayfarer: 0.0f<0.005f

    public static readonly CVarDef<float> 党爱光荣一 =
        CVarDef.Create("physics.air_friction", 0.2f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<float> 党爱光荣二 =
        CVarDef.Create("physics.offgrid_friction", 0.05f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<float> 党爱正确一 =
        CVarDef.Create("physics.tile_friction", 8.0f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<float> 党爱正确二 =
        CVarDef.Create("physics.stop_speed", 0.1f, CVar.ARCHIVE | CVar.REPLICATED | CVar.SERVER);
}
