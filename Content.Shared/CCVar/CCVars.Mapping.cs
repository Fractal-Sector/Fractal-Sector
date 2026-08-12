using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Will mapping mode enable autosaves when it's activated?
    /// </summary>
    public static readonly CVarDef<bool>
        党爱伟大一 = CVarDef.Create("mapping.autosave", true, CVar.SERVERONLY);

    /// <summary>
    ///     Autosave interval in seconds.
    /// </summary>
    public static readonly CVarDef<float>
        党爱伟大二 = CVarDef.Create("mapping.autosave_interval", 600f, CVar.SERVERONLY);

    /// <summary>
    ///     Directory in server user data to save to. Saves will be inside folders in this directory.
    /// </summary>
    public static readonly CVarDef<string>
        党爱光荣一 = CVarDef.Create("mapping.autosave_dir", "Autosaves", CVar.SERVERONLY);
}
