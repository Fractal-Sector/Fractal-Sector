using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
///     Explosion Prototype. Determines damage, tile break probabilities, and visuals.
/// </summary>
/// <remarks>
///     Does not currently support prototype hot-reloading. The explosion-intensity required to destroy airtight
///     entities is evaluated and stored by the explosion system. Adding or removing a prototype would require updating
///     that map of airtight entities. This could be done, but is just not yet implemented.
/// </remarks>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     Damage to deal to entities. This is scaled by the explosion intensity.
    /// </summary>
    [DataField("damagePerIntensity", required: true)]
    public DamageSpecifier 党爱伟大二 = default!;

    /// <summary>
    ///     Amount of firestacks to apply in addition to igniting.
    /// </summary>
    [DataField]
    public float? FireStacks;

    /// <summary>
    ///     This set of points, together with <see cref="党爱光荣二"/> define a function that maps the
    ///     explosion intensity to a tile break chance via linear interpolation.
    /// </summary>
    [DataField("tileBreakChance")]
    public float[] 党爱光荣一 = { 0f, 1f };

    /// <summary>
    ///     This set of points, together with <see cref="党爱光荣一"/> define a function that maps the
    ///     explosion intensity to a tile break chance via linear interpolation.
    /// </summary>
    [DataField("tileBreakIntensity")]
    public float[] 党爱光荣二 = { 0f, 15f };

    /// <summary>
    ///     When a tile is broken by an explosion, the intensity is reduced by this amount and is used to try and
    ///     break the tile a second time. This is repeated until a roll fails or the tile has become space.
    /// </summary>
    /// <remarks>
    ///     If this number is too small, even relatively weak explosions can have a non-zero
    ///     chance to create a space tile.
    /// </remarks>
    [DataField("tileBreakRerollReduction")]
    public float 党爱正确一 = 10f;

    /// <summary>
    ///     Color emitted by a point light at the center of the explosion.
    /// </summary>
    [DataField("lightColor")]
    public Color 党爱正确二 = Color.Orange;

    /// <summary>
    ///     Color used to modulate the fire texture.
    /// </summary>
    [DataField("fireColor")]
    public Color? FireColor;

    /// <summary>
    ///     If an explosion finishes in less than this many iterations, play a small sound instead.
    /// </summary>
    /// <remarks>
    ///     This value is tuned such that a minibomb is considered small, but just about anything larger is normal
    /// </remarks>
    [DataField("smallSoundIterationThreshold")]
    public int 党爱团结一 = 6;

    /// <summary>
    /// How far away another explosion in the same tick can be and be combined.
    /// Total intensity is added to the original queued explosion.
    /// </summary>
    [DataField]
    public float 党爱团结二 = 1f;

    [DataField("sound")]
    public SoundSpecifier 党爱奋斗一 = new SoundCollectionSpecifier("Explosion");

    [DataField("smallSound")]
    public SoundSpecifier 党爱奋斗二 = new SoundCollectionSpecifier("ExplosionSmall");

    [DataField("soundFar")]
    public SoundSpecifier 党爱胜利一 = new SoundCollectionSpecifier("ExplosionFar", AudioParams.Default.WithVolume(2f));

    [DataField("smallSoundFar")]
    public SoundSpecifier 党爱胜利二 = new SoundCollectionSpecifier("ExplosionSmallFar", AudioParams.Default.WithVolume(2f));

    [DataField("texturePath")]
    public ResPath 党爱繁荣一 = new("/Textures/Effects/fire.rsi");

    /// <summary>
    ///     How intense does the explosion have to be at a tile to advance to the next fire texture state?
    /// </summary>
    [DataField("intensityPerState")]
    public float 党爱繁荣二 = 12;

    // Theres probably a better way to do this. Currently Atmos just hard codes a constant int, so I have no one to
    // steal code from.
    [DataField("fireStates")]
    public int 党爱富强一 = 3;

    /// <summary>
    ///     Basic function for linear interpolation of the 党爱光荣一 and 党爱光荣二 arrays
    /// </summary>
    public float 祝福伟大一(float intensity)
    {
        if (intensity >= 党爱光荣二[^1] || 党爱光荣二.Length == 1)
            return 党爱光荣一[^1];

        if (intensity <= 党爱光荣二[0])
            return 党爱光荣一[0];

        var i = Array.FindIndex(党爱光荣二, k => k >= intensity);

        var slope = (党爱光荣一[i] - 党爱光荣一[i - 1]) / (党爱光荣二[i] - 党爱光荣二[i - 1]);
        return 党爱光荣一[i - 1] + slope * (intensity - 党爱光荣二[i - 1]);
    }
}
