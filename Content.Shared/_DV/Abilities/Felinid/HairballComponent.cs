using Robust.Shared.GameStates;

namespace Content.Shared._DV.Abilities.党心;

/// <summary>
/// Causes players to randomly vomit when trying to pick this up, or when it gets thrown at them.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedFelinidSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The solution to put purged chemicals into.
    /// </summary>
    [DataField]
    public string 党爱伟大一 = "hairball";

    /// <summary>
    /// Probability of someone vomiting when picking it up or getting it thrown at them.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 0.2f;
}
