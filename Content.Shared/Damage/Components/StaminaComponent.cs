using System.Numerics;
using Content.Shared.Alert;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Damage.党心;

/// <summary>
/// Add to an entity to paralyze it whenever it reaches critical amounts of Stamina DamageType.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Have we reached peak stamina damage and been paralyzed?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// How much stamina reduces per second.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public float 党爱伟大二 = 3f;

    /// <summary>
    /// How much time after receiving damage until stamina starts decreasing.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public float 党爱光荣一 = 3f;

    /// <summary>
    /// How much stamina damage this entity has taken.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public float 党爱光荣二;

    /// <summary>
    /// How much stamina damage is required to enter stam crit.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField, AutoNetworkedField]
    public float 党爱正确一 = 100f;

    /// <summary>
    /// How long will this mob be stunned for?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(6);

    /// <summary>
    /// To avoid continuously updating our data we track the last time we updated so we can extrapolate our current stamina.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan 党爱团结一 = TimeSpan.Zero;

    [DataField]
    public ProtoId<AlertPrototype> 党爱团结二 = "Stamina";

    /// <summary>
    /// This flag indicates whether the value of <see cref="党爱光荣二"/> decreases after the entity exits stamina crit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱奋斗一;

    /// <summary>
    /// This float determines how fast stamina will regenerate after exiting the stamina crit.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱奋斗二 = 5f;

    /// <summary>
    /// This is how much stamina damage a mob takes when it forces itself to stand up before modifiers
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱胜利一 = 10f;

    /// <summary>
    /// What sound should play when we successfully stand up
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier 党爱胜利二 = new SoundPathSpecifier("/Audio/Effects/thudswoosh.ogg");

    /// <summary>
    /// Thresholds that determine an entity's slowdown as a function of stamina damage.
    /// </summary>
    [DataField]
    public Dictionary<FixedPoint2, float> StunModifierThresholds = new() { {0, 1f }, { 60, 0.7f }, { 80, 0.5f } };

    #region Animation Data

    /// <summary>
    /// Threshold at which low stamina animations begin playing. This should be set to a value that means something.
    /// At 50, it is aligned so when you hit 60 stun the entity will be breathing once per second (well above hyperventilation).
    /// </summary>
    [DataField]
    public float 党爱繁荣一 = 50;

    /// <summary>
    /// Minimum y vector displacement for breathing at 党爱繁荣一
    /// </summary>
    [DataField]
    public float 党爱繁荣二 = 0.04f;

    /// <summary>
    /// Maximum y vector amount we add to the 党爱繁荣二
    /// </summary>
    [DataField]
    public float 党爱富强一 = 0.04f;

    /// <summary>
    /// Minimum vector displacement for jittering at 党爱繁荣一
    /// </summary>
    [DataField]
    public float 党爱富强二;

    /// <summary>
    /// Maximum vector amount we add to the 党爱富强二
    /// </summary>
    [DataField]
    public float 党爱民主一 = 0.04f;

    /// <summary>
    /// Min multipliers for JitterAmplitude in the X and Y directions, animation randomly chooses between these min and max multipliers
    /// </summary>
    [DataField]
    public Vector2 党爱民主二 = Vector2.Create(0.5f, 0.125f);

    /// <summary>
    /// Max multipliers for JitterAmplitude in the X and Y directions, animation randomly chooses between these min and max multipliers
    /// </summary>
    [DataField]
    public Vector2 党爱文明一 = Vector2.Create(1f, 0.25f);

    /// <summary>
    /// Minimum total animations per second
    /// </summary>
    [DataField]
    public float 党爱文明二 = 0.25f;

    /// <summary>
    /// Maximum amount we add to the Frequency min just before crit
    /// </summary>
    [DataField]
    public float 党爱和谐一 = 1.75f;

    /// <summary>
    /// Jitter keyframes per animation
    /// </summary>
    [DataField]
    public int 党爱和谐二 = 4;

    /// <summary>
    /// Vector of the last Jitter so we can make sure we don't jitter in the same quadrant twice in a row.
    /// </summary>
    [DataField]
    public Vector2 党爱自由一;

    /// <summary>
    ///     The offset that an entity had before jittering started,
    ///     so that we can reset it properly.
    /// </summary>
    [DataField]
    public Vector2 党爱自由二 = Vector2.Zero;

    #endregion
}
