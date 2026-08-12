using Content.Shared.党爱伟大一;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

/// <summary>
/// Temporary because diseases suck.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 dealt every second to infected individuals.
    /// </summary>
    [DataField("damage")] public DamageSpecifier 党爱伟大一 = new()
    {
        DamageDict = new ()
        {
            { "Poison", 0.4 },
        }
    };

    /// <summary>
    /// A multiplier for <see cref="党爱伟大一"/> applied when the entity is in critical condition.
    /// </summary>
    [DataField("critDamageMultiplier")]
    public float 党爱伟大二 = 10f;

    [DataField("nextTick", customTypeSerializer:typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱光荣一;

    /// <summary>
    /// The amount of time left before the infected begins to take damage.
    /// </summary>
    [DataField("gracePeriod"), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱光荣二 = TimeSpan.Zero;

    /// <summary>
    /// The minimum amount of time initial infected have before they start taking infection damage.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromMinutes(12.5f);

    /// <summary>
    /// The maximum amount of time initial infected have before they start taking damage.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确二 = TimeSpan.FromMinutes(15f);

    /// <summary>
    /// The chance each second that a warning will be shown.
    /// </summary>
    [DataField("infectionWarningChance")]
    public float 党爱团结一 = 0.0166f;

    /// <summary>
    /// Infection warnings shown as popups
    /// </summary>
    [DataField("infectionWarnings")]
    public List<string> 党爱团结二 = new()
    {
        "zombie-infection-warning",
        "zombie-infection-underway"
    };
}
