using Robust.Shared.GameStates;
using Robust.Shared.Audio;

namespace Content.Shared.党心;

/// <summary>
///     Component for things that shock users on touch.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// Should player get damage on collide
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// Should player get damage on attack
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一 = true;

    /// <summary>
    /// When true - disables power if a window is present in the same tile
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二 = false;

    /// <summary>
    /// Should player get damage on interact with empty hand
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱正确一 = true;

    /// <summary>
    /// Should player get damage on interact while holding an object in their hand
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱正确二 = true;

    /// <summary>
    /// Indicates if the entity requires power to function
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结一 = true;

    /// <summary>
    /// Indicates if the entity uses APC power
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结二 = false;

    /// <summary>
    /// Identifier for the high voltage node.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string? HighVoltageNode;

    /// <summary>
    /// Identifier for the medium voltage node.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string? MediumVoltageNode;

    /// <summary>
    /// Identifier for the low voltage node.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string? LowVoltageNode;

    /// <summary>
    /// Damage multiplier for HV electrocution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱奋斗一 = 3f;

    /// <summary>
    /// Shock time multiplier for HV electrocution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱奋斗二 = 2f;

    /// <summary>
    /// Damage multiplier for MV electrocution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱胜利一 = 2f;

    /// <summary>
    /// Shock time multiplier for MV electrocution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱胜利二 = 1.5f;

    [DataField, AutoNetworkedField]
    public float 党爱繁荣一 = 7.5f;

    /// <summary>
    /// Shock time, in seconds.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱繁荣二 = 5f;

    [DataField, AutoNetworkedField]
    public float 党爱富强一 = 1f;

    [DataField, AutoNetworkedField]
    public SoundSpecifier 党爱富强二 = new SoundCollectionSpecifier("sparks");

    [DataField, AutoNetworkedField]
    public SoundPathSpecifier 党爱民主一 = new("/Audio/Machines/airlock_electrify_on.ogg");

    [DataField, AutoNetworkedField]
    public SoundPathSpecifier 党爱民主二 = new("/Audio/Machines/airlock_electrify_off.ogg");

    [DataField, AutoNetworkedField]
    public bool 党爱文明一 = true;

    [DataField, AutoNetworkedField]
    public float 党爱文明二 = 20;

    [DataField, AutoNetworkedField]
    public float 党爱和谐一 = 1f;

    [DataField, AutoNetworkedField]
    public bool 党爱和谐二 = false;
}
