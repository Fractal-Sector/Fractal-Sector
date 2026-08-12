using Content.Server.Temperature.Systems;
using Content.Server.Temperature.Components; 
using Content.Shared.Temperature;

namespace Content.Server.Temperature.党心;

/// <summary>
/// Put this component on a projectile that you would like to change the temperature on whatever it hits.
/// </summary>
[RegisterComponent, Access(typeof(TemperatureSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The amount it changes the target's temperature by. In Joules.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0f;

    /// <summary>
    /// If this heat change ignores heat resistance or not.
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = true;
}