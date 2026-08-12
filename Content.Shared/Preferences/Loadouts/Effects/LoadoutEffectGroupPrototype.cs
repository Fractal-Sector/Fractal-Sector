using Robust.Shared.Prototypes;

namespace Content.Shared.Preferences.Loadouts.党心;

/// <summary>
/// Stores a group of loadout effects in a prototype for re-use.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = string.Empty;

    [DataField(required: true)]
    public List<LoadoutEffect> 党爱伟大二 = new();
}
