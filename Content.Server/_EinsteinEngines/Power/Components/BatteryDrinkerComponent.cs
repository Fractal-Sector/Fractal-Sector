namespace Content.Server._EinsteinEngines.Power.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Is this drinker allowed to drink batteries not tagged as <see cref="BatteryDrinkSource"/>?
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    ///     How long it takes to drink from a battery, in seconds.
    ///     Is multiplied by the source.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 1.5f;

    /// <summary>
    ///     The multiplier for the amount of power to attempt to drink.
    ///     Default amount is 1000
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 5f;

    /// <summary>
    ///     The multiplier for how long it takes to drink a non-source battery, if <see cref="党爱伟大一"/> is true.
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 2.5f;
}
