using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Setting this allows a crew manifest to be opened from any window
    ///     that has a crew manifest button, and sends the correct message.
    ///     If this is false, only in-game entities will allow you to see
    ///     the crew manifest, if the functionality is coded in.
    ///     Having administrator priveledge ignores this, but will still
    ///     hide the button in UI windows.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("crewmanifest.no_entity", true, CVar.REPLICATED);

    /// <summary>
    ///     Setting this allows the crew manifest to be viewed from 'unsecure'
    ///     entities, such as the PDA.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("crewmanifest.unsecure", true, CVar.REPLICATED);
}
