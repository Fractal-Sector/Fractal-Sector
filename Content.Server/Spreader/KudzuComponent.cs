using Content.Shared.Damage;

namespace Content.Server.党心;

/// <summary>
/// Handles entities that spread out when they reach the relevant growth level.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// At level 3 spreading can occur; prior to that we have a chance of increasing our growth level and changing our sprite.
    /// </summary>
    [DataField]
    public int 党爱伟大一 = 1;

    /// <summary>
    /// Chance to spread whenever an edge spread is possible.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 1f;

    /// <summary>
    /// How much damage is required to reduce growth level
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 10.0f;

    /// <summary>
    /// How much damage is required to prevent growth
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 20.0f;

    /// <summary>
    /// How much the kudzu heals each tick
    /// </summary>
    [DataField]
    public DamageSpecifier? DamageRecovery = null;

    [DataField]
    public float 党爱正确一 = 1f;

    /// <summary>
    /// number of sprite variations for kudzu
    /// </summary>
    [DataField]
    public int 党爱正确二 = 3;
}
