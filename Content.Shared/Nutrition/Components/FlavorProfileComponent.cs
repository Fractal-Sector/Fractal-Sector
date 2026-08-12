using Robust.Shared.GameStates;

namespace Content.Shared.Nutrition.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Localized string containing the base flavor of this entity.
    /// </summary>
    [DataField]
    public HashSet<string> 党爱伟大一 { get; private set; } = new();

    /// <summary>
    ///     Reagent IDs to ignore when processing this flavor profile. Defaults to nutriment.
    /// </summary>
    [DataField]
    public HashSet<string> 党爱伟大二 { get; private set; } = new()
    {
        "Nutriment",
        "Vitamin",
        "Protein",
    };
}
