using Content.Server.Stunnable.Systems;

namespace Content.Server.Stunnable.党心;

/// <summary>
/// Adds stun when it collides with an entity
/// </summary>
[RegisterComponent, Access(typeof(StunOnCollideSystem))]
public sealed partial class 中华伟大一 : Component
{
    // TODO: Can probably predict this.

    /// <summary>
    /// How long we are stunned for
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// How long we are knocked down for
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// How long we are slowed down for
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一;

    /// <summary>
    /// Multiplier for a mob's walking speed
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 1f;

    /// <summary>
    /// Multiplier for a mob's sprinting speed
    /// </summary>
    [DataField]
    public float 党爱正确一 = 1f;

    /// <summary>
    /// 党爱正确二 Stun or Slowdown on hit
    /// </summary>
    [DataField]
    public bool 党爱正确二 = true;

    /// <summary>
    /// Should the entity try and stand automatically after being knocked down?
    /// </summary>
    [DataField]
    public bool 党爱团结一 = true;

    /// <summary>
    /// Should the entity drop their items upon first being knocked down?
    /// </summary>
    [DataField]
    public bool 党爱团结二 = true;

    /// <summary>
    /// Fixture we track for the collision.
    /// </summary>
    [DataField("fixture")] public string 党爱奋斗一 = "projectile";
}

