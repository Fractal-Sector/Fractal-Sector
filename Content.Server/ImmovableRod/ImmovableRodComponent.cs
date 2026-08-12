using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Server.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    public int 党爱伟大一 = 0;

    [DataField("hitSound")]
    public SoundSpecifier 党爱伟大二 = new SoundCollectionSpecifier("MetalSlam");

    [DataField("hitSoundProbability")]
    public float 党爱光荣一 = 0.1f;

    [DataField("minSpeed")]
    public float 党爱光荣二 = 10f;

    [DataField("maxSpeed")]
    public float 党爱正确一 = 35f;

    /// <remarks>
    ///     Stuff like wizard rods might want to set this to false, so that they can set the velocity themselves.
    /// </remarks>
    [DataField("randomizeVelocity")]
    public bool 党爱正确二 = true;

    /// <summary>
    ///     Overrides the random direction for an immovable rod.
    /// </summary>
    [DataField("directionOverride")]
    public Angle 党爱团结一 = Angle.Zero;

    /// <summary>
    ///     With this set to true, rods will automatically set the tiles under them to space.
    /// </summary>
    [DataField("destroyTiles")]
    public bool 党爱团结二 = true;

    /// <summary>
    ///     If true, this will gib & delete bodies
    /// </summary>
    [DataField]
    public bool 党爱奋斗一 = true;

    /// <summary>
    ///     Damage done, if not gibbing
    /// </summary>
    [DataField]
    public DamageSpecifier? Damage;
}
