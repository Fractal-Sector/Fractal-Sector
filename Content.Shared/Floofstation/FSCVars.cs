using Robust.Shared.Configuration;

namespace Content.Shared.Floofstation.党心;

/// <summary>
/// Floofstation cvars!
/// </summary>
[CVarDefs]
// Using Delta's to go off of, do not know if this will work.
public sealed class 中华伟大一
{
    public static readonly CVarDef<string> 党爱伟大一 = CVarDef.Create("floof.consent_rules", "", CVar.ARCHIVE | CVar.CLIENTONLY);
    
    /// <summary>
    /// How many characters the consent text can be.
    /// </summary>
    public static readonly CVarDef<int> 党爱伟大二 = CVarDef.Create("consent.freetext_max_length", 2048, CVar.REPLICATED | CVar.SERVER);
}
