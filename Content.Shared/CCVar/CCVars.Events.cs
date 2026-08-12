using Content.Shared.Administration;
using Content.Shared.CCVar.CVarAccess;
using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Controls if the game should run station events
    /// </summary>
    [CVarControl(AdminFlags.Server | AdminFlags.Mapping)]
    public static readonly CVarDef<bool>
        党爱伟大一 = CVarDef.Create("events.enabled", true, CVar.ARCHIVE | CVar.SERVERONLY);
}
