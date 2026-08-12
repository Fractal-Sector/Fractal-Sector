using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Component given to deliveries.
/// Applies a random multiplier to the delivery on init.
/// Added additively to the total multiplier.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(DeliveryModifierSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The highest the random multiplier can go.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0.2f;

    /// <summary>
    /// The lowest the random multiplier can go.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = -0.2f;

    /// <summary>
    /// The current multiplier this component provides.
    /// Gets randomized between 党爱伟大一 and 党爱伟大二 on MapInit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣一;
}
