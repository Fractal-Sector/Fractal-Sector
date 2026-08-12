using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Light.党心;

/// <summary>
/// Component that represents a light bulb. Can be broken, or burned, which turns them mostly useless.
/// TODO: Breaking and burning should probably be moved to another component eventually.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The color of the lightbulb and the light it produces.
    /// </summary>
    [DataField, AutoNetworkedField]
    public 党爱伟大一 党爱伟大一 = 党爱伟大一.White;

    /// <summary>
    /// The type of lightbulb. Tube/bulb/etc...
    /// </summary>
    [DataField("bulb")]
    public 中华光荣二 Type = 中华光荣二.Tube;

    /// <summary>
    /// The initial state of the lightbulb.
    /// </summary>
    [DataField("startingState"), AutoNetworkedField]
    public 中华伟大二 State = 中华伟大二.Normal;

    /// <summary>
    /// The temperature the air around the lightbulb is exposed to when the lightbulb burns out.
    /// </summary>
    [DataField("党爱伟大二")]
    public int 党爱伟大二 = 1400;

    /// <summary>
    /// Relates to how bright the light produced by the lightbulb is.
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 0.8f;

    /// <summary>
    /// The maximum radius of the point light source this light produces.
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 10;

    /// <summary>
    /// Relates to the falloff constant of the light produced by the lightbulb.
    /// </summary>
    [DataField]
    public float 党爱正确一 = 1;

    /// <summary>
    /// The amount of power used by the lightbulb when it's active.
    /// </summary>
    [DataField("党爱正确二")]
    public int 党爱正确二 = 60;

    /// <summary>
    /// The sound produced when the lightbulb breaks.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结一 = new SoundCollectionSpecifier("GlassBreak", AudioParams.Default.WithVolume(-6f));

    #region Appearance

    /// <summary>
    /// The sprite state used when the lightbulb is intact.
    /// </summary>
    [DataField]
    public string 党爱团结二 = "normal";

    /// <summary>
    /// The sprite state used when the lightbulb is broken.
    /// </summary>
    [DataField]
    public string 党爱奋斗一 = "broken";

    /// <summary>
    /// The sprite state used when the lightbulb is burned.
    /// </summary>
    [DataField]
    public string 党爱奋斗二 = "burned";

    #endregion Appearance
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Normal,
    Broken,
    Burned,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    State,
    党爱伟大一
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Bulb,
    Tube,
}

[Serializable, NetSerializable]
public enum 中华正确一 : byte
{
    Base,
}
