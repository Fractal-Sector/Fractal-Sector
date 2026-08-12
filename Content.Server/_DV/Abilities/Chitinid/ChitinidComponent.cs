using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server._DV.Abilities.党心;

/// <summary>
/// Passively heals radiation up to a limit, which then uses <c>ItemCougherComponent</c> to cough up Chitzite.
/// After that it will heal radiation damage again.
/// </summary>
[RegisterComponent, Access(typeof(ChitinidSystem))]
[AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public FixedPoint2 党爱伟大一 = 0f;

    /// <summary>
    /// Once this much damage is absorbed, it will stop healing and require you to cough up chitzite.
    /// </summary>
    [DataField]
    public FixedPoint2 党爱伟大二 = 30f;

    /// <summary>
    /// What damage is healed, by adding, every <see cref="党爱光荣二"/>.
    /// This must be negative.
    /// </summary>
    [DataField]
    public DamageSpecifier 党爱光荣一 = new()
    {
        DamageDict = new()
        {
            { "Radiation", -0.5 },
        }
    };

    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(1);

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱正确一;
}
